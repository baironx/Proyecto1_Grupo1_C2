using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppEntrenamientoPersonal.Entidades;

namespace AppEntrenamientopersonal.Servicios
{
    /// <summary>
    /// Delegate para validaciones de rutina.
    /// </summary>
    public delegate bool RutinaValidadorDelegate(Rutina rutina);

    /// <summary>
    /// Interfaz genérica de persistencia de datos.
    /// </summary>
    /// <typeparam name="T">Tipo de dato a manejar</typeparam>
    public interface IDataPersistence<T>
    {
        void Save(List<T> data, string filePath);
        List<T> Load(string filePath);
    }

    /// <summary>
    /// Implementación de persistencia para atletas en CSV.
    /// </summary>
    internal class AtletaCsvPersistence : IDataPersistence<Atleta>
    {
        public void Save(List<Atleta> atletas, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var atleta in atletas)
                {
                    sw.WriteLine($"{atleta.Nombre},{atleta.Peso},{atleta.Altura},{atleta.Objetivos},{atleta.Nivel}");
                }
            }
        }

        public List<Atleta> Load(string filePath)
        {
            var atletas = new List<Atleta>();
            if (!File.Exists(filePath)) return atletas;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] partes = line.Split(',');
                if (partes.Length < 5) continue;
                if (!double.TryParse(partes[1], out double peso)) continue;
                if (!double.TryParse(partes[2], out double altura)) continue;
                atletas.Add(new Atleta(partes[0], peso, altura, partes[3], partes[4]));
            }
            return atletas;
        }
    }

    /// <summary>
    /// Implementación de persistencia para rutinas en CSV.
    /// </summary>
    internal class RutinaCsvPersistence : IDataPersistence<Rutina>
    {
        private readonly RutinaValidadorDelegate validador;

        public RutinaCsvPersistence(RutinaValidadorDelegate validador = null)
        {
            this.validador = validador;
        }

        public void Save(List<Rutina> rutinas, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                foreach (var rutina in rutinas)
                {
                    string fechaVencimientoStr = rutina.FechaVencimiento.HasValue
                        ? rutina.FechaVencimiento.Value.ToString("yyyy-MM-dd")
                        : "";
                    sw.WriteLine($"{rutina.Tipo},{rutina.Duracion},{rutina.Intensidad},{rutina.GrupoMuscular},{rutina.NombreAtleta},{rutina.FechaRealizacion:yyyy-MM-dd},{fechaVencimientoStr},{rutina.LesionesPostEntrenamiento}");
                }
            }
        }

        public List<Rutina> Load(string filePath)
        {
            var rutinas = new List<Rutina>();
            if (!File.Exists(filePath)) return rutinas;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string linea in lines)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                string[] partes = linea.Split(',');
                if (partes.Length < 8) continue;

                if (!int.TryParse(partes[1], out int duracion)) continue;
                if (!DateTime.TryParse(partes[5], out DateTime fechaRealizacion)) continue;

                DateTime? fechaVencimiento = null;
                if (!string.IsNullOrWhiteSpace(partes[6]) &&
                    DateTime.TryParse(partes[6], out DateTime parsedFechaVencimiento))
                {
                    fechaVencimiento = parsedFechaVencimiento;
                }

                string tipo = partes[0];
                string intensidad = partes[2];
                string grupoMuscular = partes[3];
                string nombreAtleta = partes[4];
                string lesiones = partes[7];

                Rutina rutina = tipo switch
                {
                    "Fuerza" => new RutinaFuerza(duracion, intensidad, grupoMuscular, nombreAtleta, fechaRealizacion, fechaVencimiento, lesiones),
                    "Cardio" => new RutinaCardio(duracion, intensidad, grupoMuscular, nombreAtleta, fechaRealizacion, fechaVencimiento, lesiones),
                    _ => null
                };

                if (rutina != null && (validador == null || validador(rutina)))
                    rutinas.Add(rutina);
            }

            return rutinas;
        }
    }

    /// <summary>
    /// Clase responsable de la carga y persistencia de datos.
    /// Usa principios SOLID sin cambiar estructura original.
    /// </summary>
    public static class GestorDatos
    {
        private static string rutaArchivoAtletas = "atletas.txt";
        private static string rutaArchivoRutinas = "rutinas.txt";

        private static IDataPersistence<Atleta> atletaPersistence = new AtletaCsvPersistence();
        private static IDataPersistence<Rutina> rutinaPersistence = new RutinaCsvPersistence(ValidarRutina);

        /// <summary>
        /// Guarda atletas y rutinas utilizando las implementaciones SOLID.
        /// </summary>
        public static void GuardarDatos(List<Atleta> atletas, List<Rutina> rutinas)
        {
            try
            {
                atletaPersistence.Save(atletas, rutaArchivoAtletas);
                rutinaPersistence.Save(rutinas, rutaArchivoRutinas);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar datos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Carga datos desde archivos y aplica migración si corresponde.
        /// </summary>
        public static void CargarDatos(out List<Atleta> atletas, out List<Rutina> rutinas)
        {
            atletas = atletaPersistence.Load(rutaArchivoAtletas);
            rutinas = rutinaPersistence.Load(rutaArchivoRutinas);
            MigrarDatosAntiguos(ref atletas, ref rutinas);
        }

        /// <summary>
        /// Valida que el atleta de la rutina exista antes de agregarla.
        /// </summary>
        private static bool ValidarRutina(Rutina rutina)
        {
            var atletas = atletaPersistence.Load(rutaArchivoAtletas);
            return atletas.Any(a => a.Nombre == rutina.NombreAtleta);
        }

        /// <summary>
        /// Migra datos antiguos si el archivo existe.
        /// </summary>
        private static void MigrarDatosAntiguos(ref List<Atleta> atletas, ref List<Rutina> rutinas)
        {
            string oldFilePath = "datos_antiguos.txt";
            try
            {
                if (File.Exists(oldFilePath))
                {
                    Console.WriteLine("Detectado archivo de datos antiguo. Migrando datos...");
                    string[] oldLines = File.ReadAllLines(oldFilePath);

                    foreach (string line in oldLines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length < 9) continue;

                        string name = parts[0];
                        if (!double.TryParse(parts[1], out double weight)) continue;
                        if (!double.TryParse(parts[2], out double height)) continue;
                        string goals = parts[3];
                        string level = parts[4];
                        string type = parts[5];
                        if (!int.TryParse(parts[6], out int duration)) continue;
                        string intensity = parts[7];
                        string muscleGroup = parts[8];

                        if (!atletas.Any(a => a.Nombre == name))
                        {
                            atletas.Add(new Atleta(name, weight, height, goals, level));
                        }

                        DateTime fechaRealizacion = DateTime.Now;
                        DateTime? fechaVencimiento = null;
                        string lesiones = "";

                        Rutina rutina = type switch
                        {
                            "Fuerza" => new RutinaFuerza(duration, intensity, muscleGroup, name, fechaRealizacion, fechaVencimiento, lesiones),
                            "Cardio" => new RutinaCardio(duration, intensity, muscleGroup, name, fechaRealizacion, fechaVencimiento, lesiones),
                            _ => null
                        };

                        if (rutina != null)
                            rutinas.Add(rutina);
                    }

                    GuardarDatos(atletas, rutinas);
                    File.Delete(oldFilePath);
                    Console.WriteLine("Datos migrados exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al migrar datos: {ex.Message}");
            }
        }
    }
}

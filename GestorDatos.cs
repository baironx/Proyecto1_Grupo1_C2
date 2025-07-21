using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AppEntrenamientoPersonal.Entidades;

namespace AppEntrenamientopersonal.Servicios
{
    /// <summary>
    /// Clase estática responsable de guardar y cargar datos desde archivos.
    /// Maneja la persistencia de deportistas y rutinas en archivos de texto.
    /// </summary>
    public static class GestorDatos
    {
        // Rutas de los archivos donde se guardan los datos
        private static string rutaArchivoAtletas = "atletas.txt"; // Archivo de Atletas
        private static string rutaArchivoRutinas = "rutinas.txt"; // Archivo de Rutinas

        /// <summary>
        /// Guarda las listas de deportistas y rutinas en archivos separados.
        /// Formato CSV para facilitar la lectura posterior.
        /// </summary>
        public static void GuardarDatos(List<Atleta> atletas, List<Rutina> rutinas)
        {
            try
            {
                // Guardar deportistas en formato CSV
                using (StreamWriter sw = new StreamWriter(rutaArchivoAtletas))
                {
                    foreach (var atleta in atletas)
                    {
                        // Formato: Nombre, Peso, Altura, Objetivos, Nivel
                        sw.WriteLine($"{atleta.Nombre},{atleta.Peso},{atleta.Altura},{atleta.Objetivos},{atleta.Nivel}");
                    }
                }

                // Guardar rutinas en formato CSV
                using (StreamWriter sw = new StreamWriter(rutaArchivoRutinas))
                {
                    foreach (var rutina in rutinas)
                    {
                        // Formato: Tipo, Duracion, Intensidad, GrupoMuscular, NombreAtleta, FechaRealizacion, FechaVencimiento (opcional), Lesiones
                        string fechaVencimientoStr = rutina.FechaVencimiento.HasValue ? rutina.FechaVencimiento.Value.ToString("yyyy-MM-dd") : "";
                        sw.WriteLine($"{rutina.Tipo},{rutina.Duracion},{rutina.Intensidad},{rutina.GrupoMuscular},{rutina.NombreAtleta},{rutina.FechaRealizacion.ToString("yyyy-MM-dd")},{fechaVencimientoStr},{rutina.LesionesPostEntrenamiento}");
                    }
                }
            }
            catch (Exception ex)
            {
                // En caso de error, lanzar excepción personalizada con contexto
                throw new InvalidOperationException($"Error al guardar datos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Carga datos desde archivos y reconstruye las listas de deportistas y rutinas.
        /// Usa parámetros 'out' para devolver múltiples valores.
        /// </summary>
        public static void CargarDatos(out List<Atleta> atletas, out List<Rutina> rutinas)
        {
            // Inicializar listas vacías
            atletas = new List<Atleta>();
            rutinas = new List<Rutina>();

            try
            {
                // Cargar deportistas si el archivo existe
                if (File.Exists(rutaArchivoAtletas))
                {
                    string[] lineasAtletas = File.ReadAllLines(rutaArchivoAtletas);

                    foreach (string linea in lineasAtletas)
                    {
                        // Saltar líneas vacías
                        if (string.IsNullOrWhiteSpace(linea)) continue;

                        // Dividir la línea por comas
                        string[] partes = linea.Split(',');

                        // Validar que tenga todos los campos necesarios
                        if (partes.Length < 5) continue;

                        // Intentar convertir peso y altura a números
                        if (!double.TryParse(partes[1], out double peso)) continue;
                        if (!double.TryParse(partes[2], out double altura)) continue;

                        // Crear y añadir el deportista
                        atletas.Add(new Atleta(partes[0], peso, altura, partes[3], partes[4]));
                    }
                }

                // Cargar rutinas si el archivo existe
                if (File.Exists(rutaArchivoRutinas))
                {
                    string[] lineasRutinas = File.ReadAllLines(rutaArchivoRutinas);

                    foreach (string linea in lineasRutinas)
                    {
                        // Saltar líneas vacías
                        if (string.IsNullOrWhiteSpace(linea)) continue;

                        // Dividir la línea por comas
                        string[] partes = linea.Split(',');

                        // Validar que tenga todos los campos necesarios (ahora son 8)
                        if (partes.Length < 8) continue;

                        string tipo = partes[0];
                        // Validar que la duración sea un número válido
                        if (!int.TryParse(partes[1], out int duracion)) continue;
                        string intensidad = partes[2];
                        string grupoMuscular = partes[3];
                        string nombreAtleta = partes[4];

                        // Validar y parsear FechaRealizacion
                        if (!DateTime.TryParse(partes[5], out DateTime fechaRealizacion)) continue;

                        // Validar y parsear FechaVencimiento (puede ser nula)
                        DateTime? fechaVencimiento = null;
                        if (!string.IsNullOrWhiteSpace(partes[6]) && DateTime.TryParse(partes[6], out DateTime parsedFechaVencimiento))
                        {
                            fechaVencimiento = parsedFechaVencimiento;
                        }
                        else if (string.IsNullOrWhiteSpace(partes[6]))
                        {
                            fechaVencimiento = null; // Asegurarse de que sea nulo si no se ingresa nada
                        }

                        string lesionesPostEntrenamiento = partes[7];

                        // Verificar que el atleta exista antes de crear la rutina
                        if (!atletas.Any(a => a.Nombre == nombreAtleta)) continue;

                        // Crear la rutina específica según su tipo
                        switch (tipo)
                        {
                            case "Fuerza":
                                rutinas.Add(new RutinaFuerza(duracion, intensidad, grupoMuscular, nombreAtleta, fechaRealizacion, fechaVencimiento, lesionesPostEntrenamiento));
                                break;
                            case "Cardio":
                                rutinas.Add(new RutinaCardio(duracion, intensidad, grupoMuscular, nombreAtleta, fechaRealizacion, fechaVencimiento, lesionesPostEntrenamiento));
                                break;
                        }
                    }
                }
                // Manejar migración de datos si se detecta un formato antiguo
                MigrarDatosAntiguos(ref atletas, ref rutinas);
            }
            catch (Exception ex)
            {
                // En caso de error, mostrar mensaje en consola
                Console.WriteLine($"Error al cargar datos: {ex.Message}");
            }
        }

        /// <summary>
        /// Migra datos de un formato antiguo a uno nuevo si el archivo de datos antiguos existe.
        /// Esto permite una transición suave sin pérdida de información para versiones anteriores.
        /// </summary>
        private static void MigrarDatosAntiguos(ref List<Atleta> atletas, ref List<Rutina> rutinas)
        {
            string oldFilePath = "datos_antiguos.txt"; // Ruta del archivo de datos antiguos
            try
            {
                if (File.Exists(oldFilePath))
                {
                    Console.WriteLine("Detectado archivo de datos antiguo. Migrando datos...");
                    string[] oldLines = File.ReadAllLines(oldFilePath);

                    foreach (string line in oldLines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length < 7) continue; // Mínimo de campos para datos antiguos

                        // Parsear datos antiguos
                        string name = parts[0];
                        if (!double.TryParse(parts[1], out double weight)) continue;
                        if (!double.TryParse(parts[2], out double height)) continue;
                        string goals = parts[3];
                        string level = parts[4];
                        string type = parts[5]; // Tipo de rutina ("Fuerza" o "Cardio")
                        if (!int.TryParse(parts[6], out int duration)) continue;
                        string intensity = parts[7];
                        string muscleGroup = parts[8];

                        // Añadir deportista si no existe
                        if (!atletas.Any(a => a.Nombre == name))
                        {
                            atletas.Add(new Atleta(name, weight, height, goals, level));
                        }
                        Atleta athlete = atletas.First(a => a.Nombre == name);

                        // Crear fechas por defecto para la migración
                        DateTime fechaRealizacion = DateTime.Now;
                        DateTime? fechaVencimiento = null;
                        string lesionesPostEntrenamiento = "";

                        // Crear rutinas y asignarlas al atleta
                        switch (type)
                        {
                            case "Fuerza":
                                rutinas.Add(new RutinaFuerza(duration, intensity, muscleGroup, athlete.Nombre, fechaRealizacion, fechaVencimiento, lesionesPostEntrenamiento));
                                break;
                            case "Cardio":
                                rutinas.Add(new RutinaCardio(duration, intensity, muscleGroup, athlete.Nombre, fechaRealizacion, fechaVencimiento, lesionesPostEntrenamiento));
                                break;
                        }
                    }

                    // Guardar en el nuevo formato
                    GuardarDatos(atletas, rutinas);

                    // Eliminar archivo antiguo después de una migración exitosa
                    File.Delete(oldFilePath);

                    Console.WriteLine("Datos migrados al nuevo formato exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al migrar datos: {ex.Message}");
            }
        }
    }
}
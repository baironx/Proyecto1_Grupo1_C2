using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entidades
{
    /// <summary>
    /// Servicio estático que proporciona sugerencias de rutinas personalizadas
    /// basadas en el nivel y los objetivos del deportista.
    /// </summary>
    public static class ServicioSugerenciaRutina
    {
        /// <summary>
        /// Método principal que devuelve rutinas sugeridas para un deportista específico.
        /// Analiza el nivel y los objetivos para generar recomendaciones apropiadas.
        /// </summary>
        public static List<string> ObtenerRutinasSugeridas(Atleta atleta)
        {
            var sugerencias = new List<string>();

            // Convertir a minúsculas para comparación sin distinción de mayúsculas y minúsculas
            string nivel = atleta.Nivel.ToLower();
            string objetivos = atleta.Objetivos.ToLower();

            // Seleccionar rutinas de acuerdo al nivel del deportista
            switch (nivel)
            {
                case "principiante":
                    sugerencias.AddRange(ObtenerRutinasPrincipiante(objetivos));
                    break;
                case "intermedio":
                    sugerencias.AddRange(ObtenerRutinasIntermedio(objetivos));
                    break;
                case "avanzado":
                    sugerencias.AddRange(ObtenerRutinasAvanzado(objetivos));
                    break;
                default:
                    sugerencias.AddRange(ObtenerRutinasGenerales(objetivos));
                    break;
            }
            return sugerencias;
        }

        /// <summary>
        /// Rutinas específicas para deportistas principiantes.
        /// Se enfocan en la técnica básica y la adaptación gradual.
        /// </summary>
        private static List<string> ObtenerRutinasPrincipiante(string objetivos)
        {
            var rutinas = new List<string>();

            // Rutinas de fuerza para principiantes
            if (objetivos.Contains("fuerza") || objetivos.Contains("muscular"))
            {
                rutinas.Add("Fuerza - 30 min - Baja - Pecho (Flexiones basicas, press de banca ligero)");
                rutinas.Add("Fuerza - 25 min - Baja - Piernas (Sentadillas con peso corporal)");
                rutinas.Add("Fuerza - 20 min - Baja - Brazos (Ejercicios con mancuernas ligeras)");
            }

            // Rutinas de cardio para principiantes
            if (objetivos.Contains("cardio") || objetivos.Contains("resistencia") || objetivos.Contains("peso"))
            {
                rutinas.Add("Cardio - 20 min - Baja - Cardio (Caminata rapida)");
                rutinas.Add("Cardio - 15 min - Baja - Cardio (Bicicleta estatica suave)");
                rutinas.Add("Cardio - 25 min - Media - Cardio (Trote ligero)");
            }

            // Rutinas generales básicas
            rutinas.Add("Fuerza - 30 min - Baja - Espalda (Ejercicios basicos de postura)");
            rutinas.Add("Cardio - 30 min - Baja - Cardio (Circuito de ejercicios basicos)");

            return rutinas;
        }

        /// <summary>
        /// Rutinas para deportistas intermedios.
        /// Mayor intensidad y complejidad técnica.
        /// </summary>
        private static List<string> ObtenerRutinasIntermedio(string objetivos)
        {
            var rutinas = new List<string>();

            // Rutinas de fuerza para intermedios
            if (objetivos.Contains("fuerza") || objetivos.Contains("muscular"))
            {
                rutinas.Add("Fuerza - 45 min - Media - Pecho (Press de banca, fondos)");
                rutinas.Add("Fuerza - 50 min - Media - Piernas (Sentadillas con peso, peso muerto)");
                rutinas.Add("Fuerza - 40 min - Media - Brazos (Superseries biceps/triceps)");
                rutinas.Add("Fuerza - 45 min - Media - Espalda (Dominadas, remo con barra)");
            }

            // Rutinas de cardio para intermedios
            if (objetivos.Contains("cardio") || objetivos.Contains("resistencia"))
            {
                rutinas.Add("Cardio - 35 min - Media - Cardio (Intervalos moderados)");
                rutinas.Add("Cardio - 40 min - Media - Cardio (Carrera continua)");
            }

            // Rutinas de pérdida de peso
            if (objetivos.Contains("peso"))
            {
                rutinas.Add("Cardio - 45 min - Media - Cardio (HIIT con sprints)");
                rutinas.Add("Fuerza - 40 min - Media - Cuerpo completo (Circuito con peso corporal y mancuernas)");
            }

            // Rutinas generales
            rutinas.Add("Fuerza - 50 min - Media - Hombros (Press militar, elevaciones laterales)");
            rutinas.Add("Cardio - 50 min - Media - Cardio (Natación de intensidad moderada)");

            return rutinas;
        }

        /// <summary>
        /// Rutinas para deportistas avanzados.
        /// Alta intensidad y especialización.
        /// </summary>
        private static List<string> ObtenerRutinasAvanzado(string objetivos)
        {
            var rutinas = new List<string>();

            // Rutinas de fuerza para avanzados
            if (objetivos.Contains("fuerza") || objetivos.Contains("muscular"))
            {
                rutinas.Add("Fuerza - 60 min - Alta - Pecho (Rutina de volumen con superseries)");
                rutinas.Add("Fuerza - 70 min - Alta - Piernas (Entrenamiento de fuerza máxima)");
                rutinas.Add("Fuerza - 55 min - Alta - Brazos (Entrenamiento de pico de intensidad)");
                rutinas.Add("Fuerza - 65 min - Alta - Espalda (Peso muerto pesado, remos con barra)");
            }

            // Rutinas de cardio para avanzados
            if (objetivos.Contains("cardio") || objetivos.Contains("resistencia"))
            {
                rutinas.Add("Cardio - 40 min - Alta - Cardio (Entrenamiento de intervalos de alta intensidad)");
                rutinas.Add("Cardio - 60 min - Alta - Cardio (Resistencia aerobica)");
            }

            // Rutinas de pérdida de peso para avanzados
            if (objetivos.Contains("peso"))
            {
                rutinas.Add("Cardio - 30 min - Alta - Cardio (HIIT avanzado)");
                rutinas.Add("Fuerza - 50 min - Alta - Piernas (Circuito metabolico intenso)");
            }

            // Rutinas especializadas para avanzados
            rutinas.Add("Fuerza - 70 min - Alta - Pecho (Rutina de powerlifting)");
            rutinas.Add("Cardio - 45 min - Alta - Cardio (Entrenamiento deportivo especifico)");

            return rutinas;
        }

        /// <summary>
        /// Rutinas generales cuando el nivel no está especificado.
        /// Intensidad media y enfoque equilibrado.
        /// </summary>
        private static List<string> ObtenerRutinasGenerales(string objetivos)
        {
            var rutinas = new List<string>();

            // Rutinas equilibradas de intensidad media
            rutinas.Add("Fuerza - 40 min - Media - Pecho (Rutina balanceada)");
            rutinas.Add("Fuerza - 40 min - Media - Piernas (Rutina completa de piernas)");
            rutinas.Add("Cardio - 30 min - Media - Cardio (Entrenamiento cardiovascular)");
            rutinas.Add("Fuerza - 35 min - Media - Brazos (Rutina de brazos completa)");
            rutinas.Add("Fuerza - 45 min - Media - Espalda (Rutina de espalda y hombros)");
            rutinas.Add("Cardio - 30 min - Media - Cardio (Eliptica o escaladora)");
            rutinas.Add("Fuerza - 40 min - Media - Cuerpo completo (Entrenamiento funcional)");

            return rutinas;
        }
    }
}

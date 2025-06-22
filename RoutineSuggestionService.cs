using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entities
{
    /// <summary>
    /// Static service that provides personalized routine suggestions
    /// based on the athlete's level and goals
    /// </summary>
    public static class RoutineSuggestionService
    {
        /// <summary>
        /// Main method that returns suggested routines for a specific athlete
        /// Analyzes the level and goals to generate appropriate recommendations
        /// </summary>
        public static List<string> GetSuggestedRoutines(Athlete athlete)
        {
            var suggestions = new List<string>();

            // Convert to lowercase for case-insensitive comparison
            string level = athlete.Level.ToLower();
            string goals = athlete.Goals.ToLower();

            // Select routines according to the athlete's level
            switch (level) 
            {
                case "principiante":
                    suggestions.AddRange(GetBeginnerRoutines(goals));
                    break;
                case "intermedio":
                    suggestions.AddRange(GetIntermediateRoutines(goals));
                    break;
                case "avanzado":
                    suggestions.AddRange(GetAdvancedRoutines(goals));
                    break;
                default:
                    suggestions.AddRange(GetGeneralRoutines(goals));
                    break;

            }
            return suggestions;

        }
        /// <summary>
        /// Specific routines for beginner athletes
        /// Focus on basic technique and gradual adaptation
        /// </summary>
        private static List<string> GetBeginnerRoutines(string goals)
        {
            var routines = new List<string>();

            // Strength routines for beginners
            if (goals.Contains("fuerza") || goals.Contains("muscular"))
            {
                routines.Add("Fuerza - 30 min - Baja - Pecho (Flexiones basicas, press de banca ligero)");
                routines.Add("Fuerza - 25 min - Baja - Piernas (Sentadillas con peso corporal)");
                routines.Add("Fuerza - 20 min - Baja - Brazos (Ejercicios con mancuernas ligeras)");
            }

            // Cardio routines for beginners
            if (goals.Contains("cardio") || goals.Contains("resistencia") || goals.Contains("peso"))
            {
                routines.Add("Cardio - 20 min - Baja - Cardio (Caminata rapida)");
                routines.Add("Cardio - 15 min - Baja - Cardio (Bicicleta estatica suave)");
                routines.Add("Cardio - 25 min - Media - Cardio (Trote ligero)");
            }

            // Basic general routines
            routines.Add("Fuerza - 30 min - Baja - Espalda (Ejercicios basicos de postura)");
            routines.Add("Cardio - 30 min - Baja - Cardio (Circuito de ejercicios basicos)");

            return routines;
        }

        /// <summary>
        /// Routines for intermediate athletes
        /// Greater intensity and technical complexity
        /// </summary>
        private static List<string> GetIntermediateRoutines(string goals)
        {
            var routines = new List<string>();

            // Strength routines for intermediate athletes
            if (goals.Contains("fuerza") || goals.Contains("muscular"))
            {
                routines.Add("Fuerza - 45 min - Media - Pecho (Press de banca, fondos)");
                routines.Add("Fuerza - 50 min - Media - Piernas (Sentadillas con peso, peso muerto)");
                routines.Add("Fuerza - 40 min - Media - Brazos (Superseries biceps/triceps)");
                routines.Add("Fuerza - 45 min - Media - Espalda (Dominadas, remo con barra)");
            }

            // Cardio routines for intermediate athletes
            if (goals.Contains("cardio") || goals.Contains("resistencia"))
            {
                routines.Add("Cardio - 35 min - Media - Cardio (Intervalos moderados)");
                routines.Add("Cardio - 40 min - Media - Cardio (Carrera continua)");
            }

            // Weight loss routines
            if (goals.Contains("peso"))
            {
                routines.Add("Cardio - 45 min - Alta - Cardio (HIIT intermedio)");
                routines.Add("Fuerza - 40 min - Media - Piernas (Circuito metabólico)");
            }

            return routines;
        }

        /// <summary>
        /// Routines for Advanced Athletes
        /// High Intensity and Specialized Techniques
        /// </summary>
        private static List<string> GetAdvancedRoutines(string goals)
        {
            var routines = new List<string>();

            // Strength routines for advanced athletes
            if (goals.Contains("fuerza") || goals.Contains("muscular"))
            {
                routines.Add("Fuerza - 60 min - Alta - Pecho (Press pesado, tecnicas avanzadas)");
                routines.Add("Fuerza - 65 min - Alta - Piernas (Sentadillas olimpicas, peso muerto)");
                routines.Add("Fuerza - 55 min - Alta - Brazos (Rutina de fuerza maxima)");
                routines.Add("Fuerza - 60 min - Alta - Espalda (Dominadas lastradas, remo pesado)");
            }

            // Cardio routines for advanced athletes
            if (goals.Contains("cardio") || goals.Contains("resistencia"))
            {
                routines.Add("Cardio - 50 min - Alta - Cardio (Intervalos de alta intensidad)");
                routines.Add("Cardio - 60 min - Media - Cardio (Resistencia aerobica)");
            }

            // Weight loss routines for advanced athletes
            if (goals.Contains("peso"))
            {
                routines.Add("Cardio - 30 min - Alta - Cardio (HIIT avanzado)");
                routines.Add("Fuerza - 50 min - Alta - Piernas (Circuito metabolico intenso)");
            }

            // Specialized routines for advanced athletes
            routines.Add("Fuerza - 70 min - Alta - Pecho (Rutina de powerlifting)");
            routines.Add("Cardio - 45 min - Alta - Cardio (Entrenamiento deportivo especifico)");

            return routines;
        }

        /// <summary>
        /// General routines when the level is not specified
        /// Medium intensity and balanced approach
        /// </summary>
        private static List<string> GetGeneralRoutines(string goals)
        {
            var routines = new List<string>();

            // Balanced routines of medium intensity
            routines.Add("Fuerza - 40 min - Media - Pecho (Rutina balanceada)");
            routines.Add("Fuerza - 40 min - Media - Piernas (Rutina completa de piernas)");
            routines.Add("Cardio - 30 min - Media - Cardio (Entrenamiento cardiovascular)");
            routines.Add("Fuerza - 35 min - Media - Brazos (Rutina de brazos completa)");
            routines.Add("Fuerza - 40 min - Media - Espalda (Fortalecimiento de espalda)");

            return routines;
        }
    }
}

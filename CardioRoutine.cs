using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Proyecto1_Grupo1_C2.Entities
{
    
        public class CardioRoutine : Routine // Hereda de la clase Routine
        {
            // Lista de tipos de ejercicios cardiovasculares que se trabajarán en la rutina
            public List<string> CardioTypes { get; set; }

            // Lista de ejercicios de cardio dentro de la rutina
            public List<CardioExercise> Exercises { get; set; }

            // Tiempo de descanso entre ejercicios de cardio
            public int RestTimeBetweenExercises { get; set; }

            // Frecuencia cardíaca objetivo (en pulsaciones por minuto)
            public int TargetHeartRate { get; set; }

            // Constructor que llama al constructor base de Routine
            public CardioRoutine() : base() // Usa el formato constructor de routine sin modificaciones
            {
                this.CardioTypes = new List<string>();
                this.Exercises = new List<CardioExercise>();
                this.RestTimeBetweenExercises = 60; // 1 minuto de descanso por defecto
                this.TargetHeartRate = 140; // Frecuencia cardíaca objetivo por defecto
            }

            public override void ExecuteRoutine() // Este método se ejecuta cuando se llama a la rutina de cardio
            {
                Console.WriteLine($"Ejecución de la rutina de cardio: {Name}"); // Imprime el nombre de la rutina
                Console.WriteLine($"Duración: {Duration} minutos"); // Imprime la duración de la rutina
                Console.WriteLine($"Intensidad: {Intensity}"); // Imprime la intensidad de la rutina
                Console.WriteLine($"Frecuencia cardíaca objetivo: {TargetHeartRate} ppm"); // Imprime la frecuencia cardíaca objetivo
                Console.WriteLine($"Tipos de cardio: {string.Join(", ", CardioTypes)}"); // Imprime los tipos de ejercicios cardiovasculares
            }

            public class CardioExercise
            {
                public string Name { get; set; } // Nombre del ejercicio
                public int DurationMinutes { get; set; } // Duración del ejercicio en minutos
                public string IntensityLevel { get; set; } // Nivel de intensidad (Baja, Media, Alta)
                public string CardioType { get; set; } // Tipo de cardio (Correr, Bicicleta, Natación, etc.)
                public int IntervalCount { get; set; } // Número de intervalos
                public int HighIntensitySeconds { get; set; } // Segundos de alta intensidad por intervalo
                public int LowIntensitySeconds { get; set; } // Segundos de baja intensidad por intervalo

                public override string ToString() // Este método se usa para imprimir la información del ejercicio de cardio
                {
                    if (IntervalCount > 0)
                    {
                        return $"{Name}: {DurationMinutes} min, {IntensityLevel} - Intervalos: {IntervalCount}x({HighIntensitySeconds}s/{LowIntensitySeconds}s) ({CardioType})";
                    }
                    else
                    {
                        return $"{Name}: {DurationMinutes} min, {IntensityLevel} ({CardioType})";
                    }
                }
            }
        }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Grupo1_C2.Entities
{
    public class StrengthRoutine : Routine//hereda clases de routine 
    {
        // Lista de grupos musculares que se trabajarán en la rutina
        public List<string> MuscleGroups { get; set; }

        // Lista de ejercicios de fuerza dentro de la rutina
        public List<StrengthExercise> Exercises { get; set; }

        public int RestTimeBetweenSets { get; set; }//Tiempo de resposo entre series
        public int RestTimeBetweenExercises { get; set; }// Tiempo de reposo entre ejercicios 

        // Constructor que llama al constructor base de Routine
        public StrengthRoutine() : base() //Usa el formato contructor de routine sin modificaciones 
        {
            this.MuscleGroups = new List<string>();  
            this.Exercises = new List<StrengthExercise>(); 
            this.RestTimeBetweenSets = 60; 
            this.RestTimeBetweenExercises = 120; 
        }
        public override void ExecuteRoutine()//Este metodo se ejecuta cuando se llama a la rutina de fuerza, imprime por consola la informacion de la rutina
        {
            Console.WriteLine($"Ejcecuion de la rutina de fuerza  {Name} ");//Imprime el nombre de la rutina
            Console.WriteLine($"Duracion: {Duration} minutos");// Imprime la duracion de la rutina
            Console.WriteLine($"Intensidad: {Intensity}");// Imprime la intensidad de la rutina
            Console.WriteLine($"Grupos musculares : {string.Join(", ", MuscleGroups)}");// Imprime los grupos musculares que se trabajaran en la rutina

        }
        public class StrengthExercise
        {
            public string Name { get; set; }
            public int Sets { get; set; }
            public int Reps { get; set; }
            public double Weight { get; set; }
            public string MuscleGroup { get; set; }

            public override string ToString()//Este metodo se usa para imprimir la informacion del ejercicio de fuerza, use el TOstring para que se imprima toda la informacion  automaticamente cuando se llama al objeto
            {
                return $"{Name}: {Sets}x{Reps}  {Weight}kg ({MuscleGroup})";// Devuelve un string con el nombre del ejercicio, las series, las repeticiones, el peso y el grupo muscular
            }
        
        }
    }

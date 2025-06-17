using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Grupo1_C2
{
    public static class RoutineSuggestionService
    {
        public static List<string> GetSuggestedRoutines(Athlete athlete)
        {

        }
    }
}


public void AddRoutine(Athlete currentAthlete)
{
    Console.Write("Tipo de rutina:");
    string type = Console.ReadLine();

    Console.Write("Duración (minutos):");
    if(!int.TryParse(Console.ReadLine(), out int duration) || duration <= 0)

    {
        Console.WriteLine("Duración inválida.");
        return;
    }

    Console.Write("Intensidad (Baja/Media/Alta):");
    string intensity = Console.ReadLine();

    Routine routine = new Routine
    {
        Type = type,
        Duration = duration,
        Intensity = intensity,
        MuscleGroup = muscleGroup
    };

    currentAthlete.Routines.Add(routine);
    Console.WriteLine("Rutina agregada exitosamente.");
}


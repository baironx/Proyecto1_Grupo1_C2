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



class Program
{
    static Athlete currentAthlete = new Athlete();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== GESTIÓN DE RUTINAS DE ENTRENAMIENTO =====");
            Console.WriteLine("1. Crear rutina sugerida para Principiante");
            Console.WriteLine("2. Crear rutina sugerida para Intermedio");
            Console.WriteLine("3. Crear rutina sugerida para Avanzado");
            Console.WriteLine("4. Crear rutina General");
            Console.WriteLine("5. Editar rutina actual");
            Console.WriteLine("6. Ver rutina actual");
            Console.WriteLine("7. Agregar rutina manualmente");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");

            string opcion = Console.ReadLine();
            Console.Clear();

            switch (opcion)
            {
                case "1":
                    CrearRutinaNivel("Principiante");
                    break;
                case "2":
                    CrearRutinaNivel("Intermedio");
                    break;
                case "3":
                    CrearRutinaNivel("Avanzado");
                    break;
                case "4":
                    CrearRutinaGeneral();
                    break;
                case "5":
                    EditRoutine();
                    break;
                case "6":
                    MostrarRutinaActual();
                    break;
                case "7":
                    AddRoutine(currentAthlete);
                    break;
                case "0":
                    Console.WriteLine("Saliendo...");
                    return;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }

            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }

    static void CrearRutinaNivel(string nivel)
    {
        Console.Write("Ingrese el nombre del atleta: ");
        string nombre = Console.ReadLine();

        Console.Write("Ingrese el objetivo (Perdida de peso / Ganancia muscular): ");
        string objetivo = Console.ReadLine();

        currentAthlete = new Athlete
        {
            Name = nombre,
            Level = nivel,
            Goal = objetivo,
            Routines = new List<Routine>()
        };

        currentAthlete.Routines.Add(GetSuggestionRoutine(currentAthlete));
        Console.WriteLine($"\nRutina sugerida creada para nivel {nivel}:");
        MostrarRutinaActual();
    }

    static void CrearRutinaGeneral()
    {
        currentAthlete = new Athlete
        {
            Name = "General",
            Level = "General",
            Goal = "General",
            Routines = new List<Routine> { DefaultRoutine() }
        };

        Console.WriteLine("\nRutina general creada:");
        MostrarRutinaActual();
    }

    static void MostrarRutinaActual()
    {
        if (currentAthlete?.Routines == null || currentAthlete.Routines.Count == 0)
        {
            Console.WriteLine("No hay rutinas asignadas.");
            return;
        }

        Console.WriteLine($"Atleta: {currentAthlete.Name}");
        Console.WriteLine($"Nivel: {currentAthlete.Level}");
        Console.WriteLine($"Objetivo: {currentAthlete.Goal}");

        for (int i = 0; i < currentAthlete.Routines.Count; i++)
        {
            var r = currentAthlete.Routines[i];
            Console.WriteLine($"\n--- Rutina #{i + 1} ---");
            Console.WriteLine($"Tipo: {r.Type}");
            Console.WriteLine($"Duración: {r.Duration} min");
            Console.WriteLine($"Intensidad: {r.Intensity}");
            Console.WriteLine($"Grupo Muscular: {r.MuscleGroup}");
        }
    }

    static void EditRoutine()
    {
        if (currentAthlete?.Routines == null || currentAthlete.Routines.Count == 0)
        {
            Console.WriteLine("No hay rutina asignada para editar.");
            return;
        }

        MostrarRutinaActual();
        Console.Write("\nSeleccione el número de rutina que desea editar: ");
        if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > currentAthlete.Routines.Count)
        {
            Console.WriteLine("Índice inválido.");
            return;
        }

        Routine r = currentAthlete.Routines[index - 1];

        Console.WriteLine("\nEditar Rutina - Presione Enter para mantener el valor actual.");

        Console.Write($"Tipo actual ({r.Type}): ");
        string input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) r.Type = input;

        Console.Write($"Duración actual en minutos ({r.Duration}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out int dur)) r.Duration = dur;

        Console.Write($"Intensidad actual ({r.Intensity}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) r.Intensity = input;

        Console.Write($"Grupo muscular actual ({r.MuscleGroup}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) r.MuscleGroup = input;

        Console.WriteLine("Rutina actualizada correctamente.");
    }

    static void AddRoutine(Athlete currentAthlete)
    {
        Console.Write("Tipo de rutina: ");
        string type = Console.ReadLine();

        Console.Write("Duración (minutos): ");
        if (!int.TryParse(Console.ReadLine(), out int duration) || duration <= 0)
        {
            Console.WriteLine("Duración inválida.");
            return;
        }

        Console.Write("Intensidad (Baja/Media/Alta): ");
        string intensity = Console.ReadLine().Trim().ToLower();
        if (intensity != "baja" && intensity != "media" && intensity != "alta")
        {
            Console.WriteLine("Intensidad inválida.");
            return;
        }

        Console.Write("Grupo muscular (ej: piernas, brazos, espalda, etc.): ");
        string muscleGroup = Console.ReadLine();

        if (currentAthlete.Routines == null)
        {
            currentAthlete.Routines = new List<Routine>();
        }

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

    static Routine GetSuggestionRoutine(Athlete athlete)
    {
        switch (athlete.Level.ToLower())
        {
            case "principiante":
                return GetBeginnerRoutine(athlete.Goal);
            case "intermedio":
                return GetIntermediateRoutine(athlete.Goal);
            case "avanzado":
                return GetAdvancedRoutine(athlete.Goal);
            default:
                Console.WriteLine("Nivel desconocido.");
                return DefaultRoutine();
        }
    }

    static Routine GetBeginnerRoutine(string goal)
    {
        if (goal.ToLower() == "perdida de peso")
        {
            return new Routine { Type = "Cardio", Duration = 30, Intensity = "Baja", MuscleGroup = "Cuerpo completo" };
        }
        else if (goal.ToLower() == "ganancia muscular")
        {
            return new Routine { Type = "Fuerza básica", Duration = 20, Intensity = "Moderada", MuscleGroup = "Piernas y torso" };
        }

        return DefaultRoutine();
    }

    static Routine GetIntermediateRoutine(string goal)
    {
        if (goal.ToLower() == "perdida de peso")
        {
            return new Routine { Type = "HIIT", Duration = 40, Intensity = "Alta", MuscleGroup = "Cuerpo completo" };
        }
        else if (goal.ToLower() == "ganancia muscular")
        {
            return new Routine { Type = "Fuerza progresiva", Duration = 45, Intensity = "Alta", MuscleGroup = "Parte superior" };
        }

        return DefaultRoutine();
    }

    static Routine GetAdvancedRoutine(string goal)
    {
        if (goal.ToLower() == "perdida de peso")
        {
            return new Routine { Type = "HIIT avanzado", Duration = 50, Intensity = "Muy alta", MuscleGroup = "Full body" };
        }
        else if (goal.ToLower() == "ganancia muscular")
        {
            return new Routine { Type = "Hipertrofia", Duration = 60, Intensity = "Muy alta", MuscleGroup = "División por días" };
        }

        return DefaultRoutine();
    }

     
    static Routine DefaultRoutine()
    {
        return new Routine { Type = "General", Duration = 30, Intensity = "Moderada", MuscleGroup = "General" };
    }
}

class Athlete
{
    public string Name { get; set; }
    public string Level { get; set; }
    public string Goal { get; set; }
    public List<Routine> Routines { get; set; } = new List<Routine>();
}

class Routine
{
    public string Type { get; set; }
    public int Duration { get; set; }
    public string Intensity { get; set; }
    public string MuscleGroup { get; set; }
}

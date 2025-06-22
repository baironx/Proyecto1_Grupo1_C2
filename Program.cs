using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppEntrenamientoPersonal.Entities;
using AppEntrenamientopersonal.Services;

namespace AppEntrenamientoPersonal
{
    /// <summary>
    /// Main class containing the program entry point
    /// and all console user interface logic
    /// </summary>
    class Program
    {
        // Global variables that maintain the state of the application
        static List<Athlete> athletes = new List<Athlete>();
        static List<Routine> routines = new List<Routine>();
        static Athlete? currentAthlete = null;

        /// <summary>
        /// Main program entry point
        /// Loads data and executes the main menu loop
        /// </summary>
        static void Main(string[] args)
        {
            DataManager.LoadData(out athletes, out routines);

            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n--- Menu Entrenamiento Personal ---");
                Console.WriteLine("1. Gestionar deportistas");
                Console.WriteLine("2. Agregar rutina de entrenamiento");
                Console.WriteLine("3. Mostrar rutinas de entrenamiento");
                Console.WriteLine("4. Editar rutina");
                Console.WriteLine("5. Sugerir rutinas compatibles");
                Console.WriteLine("6. Guardar y salir");
                Console.Write("Seleccione una opción: ");

                string option = Console.ReadLine() ?? "";

                switch (option)
                {
                    case "1": ManageAthletes(); break;
                    case "2": AddRoutine(); break;
                    case "3": ShowRoutines(); break;
                    case "4": EditRoutine(); break;
                    case "5": SuggestRoutines(); break;
                    case "6":
                        DataManager.SaveData(athletes, routines); // Save data and exit
                        Console.WriteLine("Datos guardados exitosamente.");
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida, intente de nuevo.");
                        break;
                }
            }
        }

        /// <summary>
        /// Submenu for complete athlete management
        /// Allows you to create, edit, delete, and select athletes
        /// </summary>
        static void ManageAthletes()
        {
            while (true)
            {
                Console.WriteLine("\n--- Gestión de Deportistas ---");
                Console.WriteLine("1. Mostrar deportistas");
                Console.WriteLine("2. Agregar nuevo deportista");
                Console.WriteLine("3. Seleccionar deportista activo");
                Console.WriteLine("4. Editar deportista");
                Console.WriteLine("5. Eliminar deportista");
                Console.WriteLine("6. Volver al menú principal");
                Console.Write("Seleccione una opción: ");

                string option = Console.ReadLine() ?? "";

                switch (option)
                {
                    case "1": ShowAthletes(); break;
                    case "2": AddAthlete(); break;
                    case "3": SelectActiveAthlete(); break;
                    case "4": EditAthlete(); break;
                    case "5": DeleteAthlete(); break;
                    case "6": return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the list of all registered athletes
        /// Indicates which athlete is currently active
        /// </summary>
        static void ShowAthletes()
        {
            Console.WriteLine("\n--- Lista de Deportistas ---");
            if (athletes.Count == 0)
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }
            // Display each athlete with their index number
            for (int i = 0; i < athletes.Count; i++)
            {
                // Mark the active athlete
                string active = athletes[i] == currentAthlete ? " (ACTIVO)" : "";
                Console.WriteLine($"{i + 1}. {athletes[i]}{active}");
            }
        }

        /// <summary>
        /// Process for adding a new athlete to the system
        /// Request all necessary data with validation
        /// </summary>
        static void AddAthlete()
        {
            Console.Write("Nombre del deportista: ");
            string name = Console.ReadLine() ?? "";

            // Validate weight with loop until obtaining a valid value
            Console.Write("Peso del deportista (kg): ");
            double weight;
            while (!double.TryParse(Console.ReadLine(), out weight) || weight <= 0)
            {
                Console.Write("Por favor ingrese un peso válido: ");
            }

            // Validate height with loop until obtaining a valid value
            Console.Write("Altura del deportista (m): ");
            double height;
            while (!double.TryParse(Console.ReadLine(), out height) || height <= 0)
            {
                Console.Write("Por favor ingrese una altura válida: ");
            }

            Console.Write("Objetivos del deportista (Fuerza/Resistencia/Perdida de peso/Ganancia muscular): ");
            string goals = Console.ReadLine() ?? "";

            Console.Write("Nivel del deportista (Principiante/Intermedio/Avanzado): ");
            string level = Console.ReadLine() ?? "";

            // Create new athlete and add it to the roster
            var newAthlete = new Athlete(name, weight, height, goals, level);
            athletes.Add(newAthlete);

            // If it is the first athlete, automatically select as active
            if (currentAthlete == null)
            {
                currentAthlete = newAthlete;
                Console.WriteLine($"Atleta registrado exitosamente y seleccionado como activo.");
            }
            else
            {
                Console.WriteLine("Atleta registrado exitosamente.");
            }
        }

        /// <summary>
        /// Allows you to select which athlete will be active for operations
        /// The active athlete is the one used to create/display routines
        /// </summary>
        static void SelectActiveAthlete()
        {
            if (athletes.Count == 0)
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }

            ShowAthletes();
            Console.Write("Seleccione el número del deportista: ");

            // Validate selection and assign active athlete
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= athletes.Count)
            {
                currentAthlete = athletes[index - 1];
                Console.WriteLine($"Deportista activo: {currentAthlete.Name}");
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Allows you to modify an existing athlete's data
        /// Keeps current values ​​if the user presses Enter without entering anything
        /// </summary>
        static void EditAthlete()
        {
            if (athletes.Count == 0) // Check if there are registered athletes
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }

            // Show list of available athletes
            ShowAthletes(); 
            Console.Write("Seleccione el número del deportista a editar: ");
            // Validate the user's selection
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= athletes.Count)
            {
                // Get the selected athlete
                var athlete = athletes[index - 1];
                Console.WriteLine($"\nEditando: {athlete.Name}");
                Console.WriteLine("Presione Enter para mantener el valor actual");

                // Edit name (keep current if Enter is pressed)
                Console.Write($"Nombre ({athlete.Name}): ");
                string name = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(name)) athlete.Name = name;

                // Edit weight with validation
                Console.Write($"Peso ({athlete.Weight} kg): ");
                string weightStr = Console.ReadLine()!;
                if (double.TryParse(weightStr, out double weight) && weight > 0) athlete.Weight = weight;

                // Edit height with validation
                Console.Write($"Altura ({athlete.Height} m): ");
                string heightStr = Console.ReadLine()!;
                if (double.TryParse(heightStr, out double height) && height > 0) athlete.Height = height;

                // Edit goals
                Console.Write($"Objetivos ({athlete.Goals}): ");
                string goals = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(goals)) athlete.Goals = goals;

                // Edit level
                Console.Write($"Nivel ({athlete.Level}): ");
                string level = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(level)) athlete.Level = level;

                Console.WriteLine("Deportista actualizado exitosamente.");
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Remove an athlete from the system with user confirmation
        /// </summary>
        static void DeleteAthlete()
        {
            // Check if there are athletes to eliminate
            if (athletes.Count == 0)
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }

            ShowAthletes();
            Console.Write("Seleccione el número del deportista a eliminar: ");

            // Validate the user's selection
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= athletes.Count)
            {
                // Check if there are athletes to eliminate
                var athleteToDelete = athletes[index - 1];
                Console.Write($"¿Está seguro de eliminar a {athleteToDelete.Name}? (s/n): ");

                if (Console.ReadLine()?.ToLower() == "s")
                {
                    // Remove the athlete from the list
                    athletes.RemoveAt(index - 1);
                    // If the deleted athlete was active, reset the current athlete
                    if (currentAthlete == athleteToDelete) 
                    {
                        currentAthlete = athletes.Count > 0 ? athletes[0] : null;
                    }
                    Console.WriteLine("Deportista eliminado exitosamente.");
                }
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Add a new routine for the active athlete
        /// Create strength or cardio routines based on the user's selection
        /// </summary>
        static void AddRoutine()
        {
            // Verify that there is an active athlete selected
            if (currentAthlete == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            // Request routine type
            Console.Write("Tipo de rutina (Fuerza/Cardio): ");
            string type = Console.ReadLine() ?? "";

            // Request duration with validation
            Console.Write("Duración de la rutina (min): ");
            int duration;
            while (!int.TryParse(Console.ReadLine(), out duration) || duration <= 0)
            {
                Console.Write("Por favor ingrese una duración válida: ");
            }
            // Request intensity
            Console.Write("Intensidad de la rutina (Baja/Media/Alta): ");
            string intensity = Console.ReadLine() ?? "";
            // Request muscle group for strength routines
            Console.Write("Grupo muscular (Pecho/Espalda/Piernas/Brazos/Cardio): ");
            string muscleGroup = Console.ReadLine() ?? "";

            // Create the routine according to the specified type
            switch (type.ToLower())
            {
                case "fuerza":
                    routines.Add(new StrengthRoutine(duration, intensity, muscleGroup, currentAthlete.Name));
                    Console.WriteLine("Rutina de fuerza agregada exitosamente.");
                    break;
                case "cardio":
                    routines.Add(new CardioRoutine(duration, intensity, muscleGroup, currentAthlete.Name));
                    Console.WriteLine("Rutina de cardio agregada exitosamente.");
                    break;
                default:
                    Console.WriteLine("Tipo de rutina no válido. Debe ser 'Fuerza' o 'Cardio'.");
                    break;
            }
        }

        /// <summary>
        /// Shows all the active athlete's routines
        /// </summary>
        static void ShowRoutines()
        {
            // Verify that there is an active athlete
            if (currentAthlete == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            Console.WriteLine($"\n--- Rutinas de {currentAthlete.Name} ---");
            var athleteRoutines = routines.Where(r => r.AthleteName == currentAthlete.Name).ToList(); // Filter active athlete routines

            if (athleteRoutines.Count == 0) // Check if the athlete has routines
            {
                Console.WriteLine("No hay rutinas registradas para este deportista.");
                return;
            }

            for (int i = 0; i < athleteRoutines.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {athleteRoutines[i].Describe()}");
            }
        }

        /// <summary>
        /// Allows editing of an existing routine for the active athlete
        /// Keeps current values ​​if Enter is pressed without entering data
        /// </summary>
        static void EditRoutine()
        {
            if (currentAthlete == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }
            // Get routines for active athletes
            var athleteRoutines = routines.Where(r => r.AthleteName == currentAthlete.Name).ToList();

            if (athleteRoutines.Count == 0)
            {
                Console.WriteLine("No hay rutinas registradas para este deportista.");
                return;
            }

            // Show routines available for editing
            Console.WriteLine($"\n--- Rutinas de {currentAthlete.Name} ---");
            for (int i = 0; i < athleteRoutines.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {athleteRoutines[i].Describe()}");
            }

            Console.Write("Seleccione el número de la rutina a editar: ");

            // Validate selection and proceed with editing
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= athleteRoutines.Count)
            {
                var routine = athleteRoutines[index - 1];
                Console.WriteLine($"\nEditando rutina: {routine.Describe()}");
                Console.WriteLine("Presione Enter para mantener el valor actual");

                // Edit duration with validation
                Console.Write($"Duración ({routine.Duration} min): ");
                string durationStr = Console.ReadLine()!;
                if (int.TryParse(durationStr, out int duration) && duration > 0) routine.Duration = duration;

                // Edit intensity
                Console.Write($"Intensidad ({routine.Intensity}): ");
                string intensity = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(intensity)) routine.Intensity = intensity;

                // Edit muscle group
                Console.Write($"Grupo muscular ({routine.MuscleGroup}): ");
                string muscleGroup = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(muscleGroup)) routine.MuscleGroup = muscleGroup;

                Console.WriteLine("Rutina actualizada exitosamente.");
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Shows suggested routines for the active athlete
        /// based on their level and goals
        /// </summary>
        static void SuggestRoutines()
        {
            if (currentAthlete == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            // Display athlete information
            Console.WriteLine($"\n--- Sugerencias para {currentAthlete.Name} ---");
            Console.WriteLine($"Nivel: {currentAthlete.Level}");
            Console.WriteLine($"Objetivos: {currentAthlete.Goals}");

            var suggestions = RoutineSuggestionService.GetSuggestedRoutines(currentAthlete); // Get suggestions from specialized service

            // Show suggested routines
            Console.WriteLine("\nRutinas sugeridas:");
            foreach (var suggestion in suggestions)
            {
                Console.WriteLine($"- {suggestion}");
            }
        }
    }
}
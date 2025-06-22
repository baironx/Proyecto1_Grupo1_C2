using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppEntrenamientoPersonal.Entities;

namespace AppEntrenamientopersonal.Services
{
    /// <summary>
    /// Static class responsible for saving and loading data from files
    /// Handles the persistence of athletes and routines in text files
    /// </summary>
    public static class DataManager
    {
        // Paths of the files where the data is saved
        private static string athletesFilePath = "athletes.txt"; //Athlete Archive
        private static string routinesFilePath = "routines.txt"; //Routine Archive

        /// <summary>
        /// Save athlete and routine lists in separate files
        /// CSV format for easier reading later
        /// </summary>
        public static void SaveData(List<Athlete> athletes, List<Routine> routines)
        {
            try
            {
                // Save athletes in CSV format
                using (StreamWriter sw = new StreamWriter(athletesFilePath))
                {
                    foreach (var athlete in athletes)
                    {
                        // Format: Name, Weight, Height, Goals, Level
                        sw.WriteLine($"{athlete.Name},{athlete.Weight},{athlete.Height},{athlete.Goals},{athlete.Level}");
                    }
                }

                // Save routines in CSV format
                using (StreamWriter sw = new StreamWriter(routinesFilePath))
                {
                    foreach (var routine in routines)
                    {
                        // Format: Type, Duration, Intensity, MuscleGroup, AthleteName
                        sw.WriteLine($"{routine.Type},{routine.Duration},{routine.Intensity},{routine.MuscleGroup},{routine.AthleteName}");
                    }
                }
            }
            catch (Exception ex)
            {
                // On error, throw custom exception with context
                throw new InvalidOperationException($"Error al guardar datos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads data from files and rebuilds athlete and routine lists
        /// Uses 'out' parameters to return multiple values
        /// </summary>
        public static void LoadData(out List<Athlete> athletes, out List<Routine> routines)
        {
            // Initialize empty lists
            athletes = new List<Athlete>();
            routines = new List<Routine>();

            try
            {
                // Load athletes if the file exists
                if (File.Exists(athletesFilePath))
                {
                    string[] athleteLines = File.ReadAllLines(athletesFilePath);

                    foreach (string line in athleteLines)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // Split the line by commas
                        string[] parts = line.Split(',');

                        // Validate that it has all the necessary fields
                        if (parts.Length < 5) continue;

                        // Try to convert weight and height to numbers
                        if (!double.TryParse(parts[1], out double weight)) continue;
                        if (!double.TryParse(parts[2], out double height)) continue;

                        // Create and add the athlete
                        athletes.Add(new Athlete(parts[0], weight, height, parts[3], parts[4]));
                    }
                }

                // Load routines if the file exists
                if (File.Exists(routinesFilePath))
                {
                    string[] routineLines = File.ReadAllLines(routinesFilePath);

                    foreach (string line in routineLines)
                    {
                        // Skip empty lines
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // Split the line by commas
                        string[] parts = line.Split(',');

                        // Validate that it has all the necessary fields
                        if (parts.Length < 5) continue;

                        string type = parts[0];

                        // Validate that the duration is a valid number
                        if (!int.TryParse(parts[1], out int duration)) continue;

                        string intensity = parts[2];
                        string muscleGroup = parts[3];
                        string athleteName = parts[4];

                        // Verify that the athlete exists before creating the routine
                        if (!athletes.Any(a => a.Name == athleteName)) continue;

                        // Create the specific routine according to its type
                        switch (type)
                        {
                            case "Fuerza":
                                routines.Add(new StrengthRoutine(duration, intensity, muscleGroup, athleteName));
                                break;
                            case "Cardio":
                                routines.Add(new CardioRoutine(duration, intensity, muscleGroup, athleteName));
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // On error, reset lists and throw exception
                athletes = new List<Athlete>();
                routines = new List<Routine>();
                throw new InvalidOperationException($"Error al cargar datos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Method for migrating data from the old format (a single file)
        /// to the new format (separate files)
        /// Maintains backward compatibility
        /// </summary>
        public static void MigrateOldData()
        {
            string oldFilePath = "datos.txt";

            // If the above file does not exist, there is nothing to migrate
            if (!File.Exists(oldFilePath)) return;

            try
            {
                string[] lines = File.ReadAllLines(oldFilePath);

                if (lines.Length == 0) return;

                var athletes = new List<Athlete>();
                var routines = new List<Routine>();

                // Parse first line as athlete (old format)
                string[] athleteData = lines[0].Split(',');
                if (athleteData.Length >= 5)
                {
                    if (double.TryParse(athleteData[1], out double weight) &&
                        double.TryParse(athleteData[2], out double height))
                    {
                        var athlete = new Athlete(athleteData[0], weight, height, athleteData[3], athleteData[4]);
                        athletes.Add(athlete);

                        
                        for (int i = 1; i < lines.Length; i++)
                        {
                            var parts = lines[i].Split(',');
                            if (parts.Length < 4) continue;

                            string type = parts[0];
                            if (!int.TryParse(parts[1], out int duration)) continue;

                            string intensity = parts[2];
                            string muscleGroup = parts[3];

                            // Create routines and assign them to the athlete
                            switch (type)
                            {
                                case "Fuerza":
                                    routines.Add(new StrengthRoutine(duration, intensity, muscleGroup, athlete.Name));
                                    break;
                                case "Cardio":
                                    routines.Add(new CardioRoutine(duration, intensity, muscleGroup, athlete.Name));
                                    break;
                            }
                        }

                        // Save in the new format
                        SaveData(athletes, routines);

                        // Delete old file after successful migration
                        File.Delete(oldFilePath);

                        Console.WriteLine("Datos migrados al nuevo formato exitosamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al migrar datos: {ex.Message}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppEntrenamientoPersonal.Entities;

namespace AppEntrenamientopersonal.Services
{
    public static class DataManager
    {
        private static string athletesFilePath = "athletes.txt";
        private static string routinesFilePath = "routines.txt";

        public static void SaveData(List<Athlete> athletes, List<Routine> routines)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(athletesFilePath))
                {
                    foreach (var athlete in athletes)
                    {
                        sw.WriteLine($"{athlete.Name},{athlete.Weight},{athlete.Height},{athlete.Goals},{athlete.Level}");
                    }
                }

                using (StreamWriter sw = new StreamWriter(routinesFilePath))
                {
                    foreach (var routine in routines)
                    {
                        sw.WriteLine($"{routine.Type},{routine.Duration},{routine.Intensity},{routine.MuscleGroup},{routine.AthleteName},{routine.RoutineName},{routine.PerformedDate:yyyy-MM-dd},{routine.ExpirationDate:yyyy-MM-dd},{routine.ExecutionTime.TotalMinutes},{routine.PostWorkoutInjuries}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al guardar datos: {ex.Message}", ex);
            }
        }

        public static void LoadData(out List<Athlete> athletes, out List<Routine> routines)
        {
            athletes = new List<Athlete>();
            routines = new List<Routine>();

            try
            {
                if (File.Exists(athletesFilePath))
                {
                    string[] athleteLines = File.ReadAllLines(athletesFilePath);

                    foreach (string line in athleteLines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split(',');
                        if (parts.Length < 5) continue;

                        if (!double.TryParse(parts[1], out double weight)) continue;
                        if (!double.TryParse(parts[2], out double height)) continue;

                        athletes.Add(new Athlete(parts[0], weight, height, parts[3], parts[4]));
                    }
                }

                if (File.Exists(routinesFilePath))
                {
                    string[] routineLines = File.ReadAllLines(routinesFilePath);

                    foreach (string line in routineLines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split(',');
                        if (parts.Length < 10) continue;

                        string type = parts[0];
                        if (!int.TryParse(parts[1], out int duration)) continue;
                        string intensity = parts[2];
                        string muscleGroup = parts[3];
                        string athleteName = parts[4];
                        string routineName = parts[5];
                        if (!DateTime.TryParse(parts[6], out DateTime performedDate)) continue;
                        if (!DateTime.TryParse(parts[7], out DateTime expirationDate)) continue;
                        if (!double.TryParse(parts[8], out double totalMinutes)) continue;
                        string postWorkoutInjuries = parts[9];
                        TimeSpan executionTime = TimeSpan.FromMinutes(totalMinutes);

                        if (!athletes.Any(a => a.Name == athleteName)) continue;

                        Routine routine = null;
                        switch (type)
                        {
                            case "Fuerza":
                                routine = new StrengthRoutine(duration, intensity, muscleGroup, athleteName);
                                break;
                            case "Cardio":
                                routine = new CardioRoutine(duration, intensity, muscleGroup, athleteName);
                                break;
                        }

                        if (routine != null)
                        {
                            routine.RoutineName = routineName;
                            routine.PerformedDate = performedDate;
                            routine.ExpirationDate = expirationDate;
                            routine.ExecutionTime = executionTime;
                            routine.PostWorkoutInjuries = postWorkoutInjuries;
                            routines.Add(routine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                athletes = new List<Athlete>();
                routines = new List<Routine>();
                throw new InvalidOperationException($"Error al cargar datos: {ex.Message}", ex);
            }
        }

        public static void MigrateOldData()
        {
            string oldFilePath = "datos.txt";
            if (!File.Exists(oldFilePath)) return;

            try
            {
                string[] lines = File.ReadAllLines(oldFilePath);
                if (lines.Length == 0) return;

                var athletes = new List<Athlete>();
                var routines = new List<Routine>();

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

                            Routine routine = null;
                            switch (type)
                            {
                                case "Fuerza":
                                    routine = new StrengthRoutine(duration, intensity, muscleGroup, athlete.Name);
                                    break;
                                case "Cardio":
                                    routine = new CardioRoutine(duration, intensity, muscleGroup, athlete.Name);
                                    break;
                            }

                            if (routine != null)
                            {
                                routine.RoutineName = $"Routine {i}";
                                routine.PerformedDate = DateTime.Now;
                                routine.ExpirationDate = DateTime.Now.AddMonths(1);
                                routine.ExecutionTime = TimeSpan.FromMinutes(duration);
                                routine.PostWorkoutInjuries = "None";
                                routines.Add(routine);
                            }
                        }

                        SaveData(athletes, routines);
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

        public static List<Routine> SearchRoutine(List<Routine> routines, string name = null, string type = null, int? duration = null)
        {
            return routines.Where(r =>
                (name == null || r.RoutineName.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                (type == null || r.Type.Equals(type, StringComparison.OrdinalIgnoreCase)) &&
                (duration == null || r.Duration == duration)).ToList();
        }

        public static int CountLastMonthRoutines(List<Routine> routines)
        {
            DateTime now = DateTime.Now;
            return routines.Count(r => r.PerformedDate >= now.AddMonths(-1) && r.PerformedDate <= now);
        }

        public static List<DateTime> GetInjuryDates(List<Routine> routines)
        {
            return routines
                .Where(r => !string.IsNullOrWhiteSpace(r.PostWorkoutInjuries) && r.PostWorkoutInjuries != "None")
                .Select(r => r.PerformedDate)
                .ToList();
        }

        public static List<Routine> GetTop3LongestRoutines(List<Routine> routines)
        {
            DateTime now = DateTime.Now;
            return routines
                .Where(r => r.PerformedDate >= now.AddMonths(-1) && r.PerformedDate <= now)
                .OrderByDescending(r => r.ExecutionTime)
                .Take(3)
                .ToList();
        }
    }
} 

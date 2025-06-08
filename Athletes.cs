using System;
using System.Collections.Generic;

namespace AppSistemaRegistroEntrenamientoPersonal.POO;
public enum LevelAthlete
{
    Amateur,
    SemiProfessional,
    Professional
}
public class Athlete
{
    public string IdAthlete { get; set; } // Unique identifier for the athlete
    public string Name { get; set; } // Name of the athlete
    public double Weight { get; set; } // Weight of the athlete in kilograms
    public double Height { get; set; } // Height of the athlete in meters
    public string Goals { get; set; } // Goals of the athlete
    public LevelAthlete Level { get; set; } // Level of the athlete (Amateur, SemiProfessional, Professional)

    public List<string> TrainingHistory { get; set; } // List of training sessions or history

    public Athlete(string name, double weight, double height, string goals, LevelAthlete level)
    {
        this.IdAthlete = Guid.NewGuid().ToString(); // Generate a unique identifier for the athlete
        this.Name = name;
        this.Weight = weight;
        this.Height = height;
        this.Goals = goals;
        this.Level = level;
        this.TrainingHistory = new List<string>(); // Initialize the training history list
    }





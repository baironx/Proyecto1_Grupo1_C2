﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entities
{
    /// <summary>
    /// Represents a training routine with details about type, duration, intensity, muscle group, and athlete name.
    /// </summary>
    public abstract class Routine
    {
        public string Type { get; set; }
        public int Duration { get; set; }
        public string Intensity { get; set; }
        public string MuscleGroup { get; set; }
        public string AthleteName { get; set; }

        /// <summary>
        /// Constructs a new instance of the Routine class.
        /// </summary>

        public Routine(string type, int duration, string intensity, string muscleGroup, string athleteName)
        {
            this.Type = type;
            this.Duration = duration;
            this.Intensity = intensity;
            this.MuscleGroup = muscleGroup;
            this.AthleteName = athleteName;
        }
        /// <summary>
        /// Abstract method that must be implemented by each specific type of routine
        /// Defines how each particular routine is described
        /// </summary>
        public abstract string Describe();

        /// <summary>
        /// General text representation of any routine
        /// </summary>
        public override string ToString()
        {
            return $"{Type} - {Duration} min - {Intensity} - Músculo: {MuscleGroup} - Atleta: {AthleteName}";
        }
    }
}
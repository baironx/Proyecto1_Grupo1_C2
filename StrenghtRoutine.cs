using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entities
{
    /// <summary>
    /// Class representing a strength training routine.
    /// Heritage from Routine class.
    /// </summary>
    public class StrengthRoutine : Routine
    {
        public StrengthRoutine(int duration, string intensity, string muscleGroup, string atleteName)
            : base("Fuerza", duration, intensity, muscleGroup, atleteName) { }

        /// <summary>
        /// Specific implementation of how a force routine is described
        /// Overrides the abstract method of the base class
        /// </summary>
        public override string Describe()
        {
            return $"[Fuerza] {Duration} min - Intensidad: {Intensity} - Grupo: {MuscleGroup}";
        }

    }
}


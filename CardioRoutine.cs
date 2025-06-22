using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entities
{
    /// <summary>
    /// Class representing a cardio training routine.
    /// Heritage from Routine class.
    /// </summary>
    public class CardioRoutine : Routine
    {
        public CardioRoutine(int duration, string intensity, string muscleGroup, string atleteName)
            : base("Cardio", duration, intensity, muscleGroup, atleteName) { }

        /// <summary>
        /// Specific implementation of how a cardio routine is described
        /// Overrides the abstract method of the base class
        /// </summary>
        public override string Describe()
        {
            return $"[Cardio] {Duration} min - Intensidad: {Intensity} - Grupo: {MuscleGroup}";
        }

    }
}
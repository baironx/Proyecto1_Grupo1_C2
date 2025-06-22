using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entities
{
    /// <summary>
    /// Represents an athlete with personal details and training goals.
    /// </summary>
    public class Athlete
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Goals { get; set; }
        public string Level { get; set; }

        /// <summary>
        /// Constructs a new instance of the Athlete class.
        /// </summary>
        public Athlete(string name, double weight, double height, string goals, string level)
        {
            this.Name = name;
            this.Weight = weight;
            this.Height = height;
            this.Goals = goals;
            this.Level = level;
        }
        public override string ToString()
        {
            return $"{Name} - {Weight} kg - {Height} m - {Goals} - {Level}";
        }
    }
}


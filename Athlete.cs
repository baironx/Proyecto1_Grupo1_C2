using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Grupo1_C2.Entities
{
    public class Athlete
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public string Goals { get; set; }
        public string Level { get; set; }

        public Athlete(string name, double weight, double height, string goals, string level)
        {
            this.Name = name;
            this.Height = height;
            this.Weight = weight;
            this.Goals = goals;
            this.Level = level;
        }
        public override string ToString()
        {
            return $"{Name} - {Weight} kg - {Height} m - {Goals} - {Level}";
        }
    }

}

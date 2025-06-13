using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Grupo1_C2.Entities
{
    public abstract class Routine
    {
        public string Name { get; set; }// nombre de la rutina
        public int Duration { get; set; } // Duracion de la rutina en minutos
        public string Intensity { get; set; } // si va ser baja , media o alta intensidad 
        public string Description { get; set; }// descripcion de la rutina
        public DateTime CreatedDate { get; set; }// fecha de creacion de la rutina
    }
     Routine()
        {
            CreatedDate = DateTime.Now;
        }
        protected Routine(string name, int duration, string intensity, string description)
        {
           this.Name = name;
            this.Duration = duration;
           this.Intensity = intensity;
            this.Description = description;
            CreatedDate = DateTime.Now;
        }
        public abstract void ExecuteRoutine();// Esto un metodo que actua diferente dependiendo de la rutina que lo invoque,  ademas puede ser sobreescrito en las clases hijas

        public virtual string GetRoutineInfo()//Este metodo devuele la informacion de las rutinas en un string, es virtual ya que las clases hijas lo pueden usar asi o sobreescribirlo a su gusto
        {
            return $"Routine: {Name}\nDuration: {Duration} minutes\nIntensity: {Intensity}\nDescription: {Description}";
        }

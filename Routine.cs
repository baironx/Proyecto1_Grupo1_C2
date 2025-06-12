using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto1_Grupo1_C2.Entities
{
    public abstract class Routine
    {    public string Name { get; set; }// nombre de la rutina
    public int Duration { get; set; } // en minutos
    public string Intensity { get; set; } // si va ser baja , media o alta 
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
    }
}

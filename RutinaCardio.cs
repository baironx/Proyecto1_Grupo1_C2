using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppEntrenamientoPersonal.Entidades
{
    /// <summary>
    /// Clase que representa una rutina de entrenamiento cardiovascular.
    /// Hereda de la clase Rutina.
    /// </summary>
    public class RutinaCardio : Rutina
    {
        public RutinaCardio(int duracion, string intensidad, string grupoMuscular, string nombreAtleta, DateTime fechaRealizacion, DateTime? fechaVencimiento, string lesionesPostEntrenamiento)
            : base("Cardio", duracion, intensidad, grupoMuscular, nombreAtleta, fechaRealizacion, fechaVencimiento, lesionesPostEntrenamiento) { }

        /// <summary>
<<<<<<< HEAD
        /// Implementaci�n espec�fica de c�mo se describe una rutina de cardio.
        /// Sobrescribe el m�todo abstracto de la clase base.
=======
        /// Implementación específica de cómo se describe una rutina de cardio.
        /// Sobrescribe el método abstracto de la clase base.
>>>>>>> f6641db85a1d75dcb5e8b8757d3177e790129063
        /// </summary>
        public override string Describir()
        {
            return $"[Cardio] {Duracion} min - Intensidad: {Intensidad} - Grupo: {GrupoMuscular} - Realizada: {FechaRealizacion.ToShortDateString()}";
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> f6641db85a1d75dcb5e8b8757d3177e790129063

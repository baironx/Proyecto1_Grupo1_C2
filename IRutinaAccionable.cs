using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entidades
{
    /// <summary>
    /// Interfaz que define un contrato para cualquier entidad que pueda ser descrita como una rutina.
    /// </summary>
    public interface IRutinaAccionable
    {
        /// <summary>
        /// Método que debe ser implementado para describir las características de la rutina.
        /// </summary>
        /// <returns>Una cadena que describe la rutina.</returns>
        string Describir();
    }
}

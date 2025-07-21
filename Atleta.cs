using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEntrenamientoPersonal.Entidades
{
    /// <summary>
    /// Representa un deportista con detalles personales y objetivos de entrenamiento.
    /// </summary>
    public class Atleta
    {
        public string Nombre { get; set; }
        public double Peso { get; set; }
        public double Altura { get; set; }
        public string Objetivos { get; set; }
        public string Nivel { get; set; }

        /// <summary>
        /// Constructor de una nueva instancia de la clase Atleta.
        /// </summary>
        public Atleta(string nombre, double peso, double altura, string objetivos, string nivel)
        {
            this.Nombre = nombre;
            this.Peso = peso;
            this.Altura = altura;
            this.Objetivos = objetivos;
            this.Nivel = nivel;
        }

        public override string ToString()
        {
            return $"{Nombre} - {Peso} kg - {Altura} m - {Objetivos} - {Nivel}";
        }
    }
}
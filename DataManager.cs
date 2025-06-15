using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proyecto1_Grupo1_C2.Entities;

namespace Proyecto1_Grupo1_C2.Services
{
    public static class DataManager
    {
        private static string athletesFilePath = "athletes.txt";
        private static string routinesFilePath = "routines.txt";
        // esta clase guarda,carga y migra datos
        public static void SaveData(list<Athlete> athletes, list<Routine>routines)
        {
            using (StreamWriter writer = new StreamWriter(athletesFilePath))// guarda los atletas en el txt
            {
                foreach(var athlete in athletes)
                {
                    writer.WriteLine($"{atlete.id}/{atlete.Name}/{athlete.Age}"); // Escribe los datos y los separa con una /
                }
            }
    }
        public static void LoadData(out List<Athlete>athletes, out List<Routine>routines)//Metodo de carga de txt
        {
            athletes=new List<Athlete>();
            routines=new List<Routine>();

            if (file.Exists(athleteFilePath))//carga los datos desde el athletes.txt si los archivos existen para procurar que no se caiga el programa
            {
                foreach (var line in File.ReadAllLines(athletesFilePath))
                {
                    var parts =line.Split("/");//se separan los datos con simbolo
                    if (parts.length >=3)
                    {
                        athletesAdd(new Athlete
                        {
                            Id = int.Parse(parts[0]), Name = parts[1], Age = int.Parse(parts[2])//Creamos un nuevo atleta y se agrega a la lista existente
                        });
                    }
                }
            }  
        }
    // falta migrar datos sugerencias aceptables
                                
}
            


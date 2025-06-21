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
   
        public static void MigrateOldData()
        {
            string oldDataFilePath= "datos.txt"; //Ruta para los datos que no estan migrados ahun
            
            if (!File.Exists(oldDataFilePath))//se necesita que el archivo exista, en todo caso de que no entonces sale
                return;
            var athletes = new List<Athlete>();//Listas vacias para almacenar datos
            var routines = new List<Routine>();

            foreach (var line in File.ReadAllLines(oldDataFilePath))//se leen todas las lineas
            {
                var parts = line.Split('/'); //Se dividen las lineas con slash
                if (parts.Length < 4) // si la linea no tiene 4 partes la ignoramos
                    continue;

                if (parts[0].Trim().ToLower() == "athlete")//Se valida que veridicamente sea un registro de athlete
                {
                    athletes.Add(new Athlete//Se agrega el nuevo atleta y se convierten los datos a enteros y el nombre como cadena
                                 {
                                     Id = int.Parse(parts[1]),
                                     Name = parts[2],
                                     Age = int.Parse(parts[3])
                                     });
                {
                    else if (parts[0].Trim().ToLower() == "routine")
                    {
                        routines.Add(new Routine//Se crea routine y se agrega a la lista igual entero, nombre de rutina y minutos
                                     {
                                         Id = int.Parse(parts[1]),
                                         Name = parts[2],
                                         Duration = int.Parse(parts[3])
                                         });
                    }
            SaveData(athletes, routines);//Se guadan los datos que se migraron
            File.Move(oldDataFilePath, oldDataFilePath + ".bak", overwrite: true);// renombramos el archivo para que no se migre otra vez 
                        
}
            


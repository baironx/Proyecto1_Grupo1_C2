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


using System;
using System.Collections.Generic;
using Proyecto1_Grupo1_C2.Entities;
namespace Proyecto1_Grupo1_C2.Services
{
    public static class AthleteManager // clase estática para gestionar deportistas
    {
        public static Athlete? currentAthlete = null; // deportista actualmente activo (puede ser null)
        public static void ManageAthletes(List<Athlete> athletes) // menú principal de gestión
        {
            bool continuar = true; // controla el ciclo del menú

            while (continuar) // ciclo del menú principal
            {
                Console.Clear(); // limpia la pantalla
                Console.WriteLine("==== GESTIÓN DE DEPORTISTAS ====");
                Console.WriteLine("1. Mostrar\n2. Agregar\n3. Seleccionar activo\n4. Editar\n5. Eliminar\n6. Volver");
                Console.Write("Opción: ");
                switch (Console.ReadLine()) // lee opción del usuario
                {
                    case "1": ShowAthletes(athletes); break;         // Muestra lista de deportistas
                    case "2": AddAthlete(athletes); break;           // Agrega nuevo deportista
                    case "3": SelectActiveAthlete(athletes); break;  // Selecciona activo
                    case "4": EditAthlete(athletes); break;          // Edita datos
                    case "5": DeleteAthlete(athletes); break;        // Elimina deportista
                    case "6": continuar = false; break;              // Sale del menú
                    default: Console.WriteLine("Opción inválida."); Esperar(); break; // Manejo de error
                }
            }
        }
        private static void ShowAthletes(List<Athlete> athletes) // muestra la lista de deportistas
        {
            Console.Clear();
            Console.WriteLine("==== LISTA DE DEPORTISTAS ====");
            if (athletes.Count == 0) // si no hay deportistas
                Console.WriteLine("No hay deportistas.");
            else
                for (int i = 0; i < athletes.Count; i++) // recorre y muestra cada deportista
                    Console.WriteLine($"{i + 1}. {athletes[i]}{(athletes[i] == currentAthlete ? " (Activo)" : "")}");
            Esperar(); // espera para que el usuario lea
        }
        private static void AddAthlete(List<Athlete> athletes) // agrega nuevo deportista
        {
            Console.Clear();
            Console.WriteLine("==== NUEVO DEPORTISTA ====");
            Console.Write("Nombre: "); var name = Console.ReadLine() ?? ""; // lee nombre
            double weight = ReadDouble("Peso (kg): ", 1); // Lee peso
            double height = ReadDouble("Altura (m): ", 0.1); // Lee altura
            Console.Write("Objetivo: "); var goals = Console.ReadLine() ?? ""; // Lee objetivo
            Console.Write("Nivel: "); var level = Console.ReadLine() ?? ""; // Lee nivel

            athletes.Add(new Athlete(name, weight, height, goals, level)); // Agrega nuevo objeto
            Console.WriteLine("Deportista agregado.");
            Esperar();
        }
        private static void SelectActiveAthlete(List<Athlete> athletes) // Selecciona deportista activo
        {
            Console.Clear();
            Console.WriteLine("==== SELECCIONAR ACTIVO ====");
            if (!MostrarListaYValidar(athletes)) return; // si la lista está vacía, termina

            int index = ReadInt("Seleccione número: ", 1, athletes.Count); // lee número
            currentAthlete = athletes[index - 1]; 
            Console.WriteLine($"Activo: {currentAthlete.Name}");
            Esperar();
        }
        private static void EditAthlete(List<Athlete> athletes) // edita datos del deportista
        {
            Console.Clear();
            Console.WriteLine("==== EDITAR DEPORTISTA ====");
            if (!MostrarListaYValidar(athletes)) return; 
            int index = ReadInt("Número a editar: ", 1, athletes.Count); // selecciona índice
            Athlete a = athletes[index - 1]; // obtiene deportista
            Console.Write($"Nombre ({a.Name}): "); var name = Console.ReadLine(); // edita nombre
            if (!string.IsNullOrWhiteSpace(name)) a.Name = name;
            double w = ReadDouble($"Peso ({a.Weight}): ", 1, true); // Edita peso
            if (w > 0) a.Weight = w;
            double h = ReadDouble($"Altura ({a.Height}): ", 0.1, true); // Edita altura
            if (h > 0) a.Height = h;
            Console.Write($"Objetivo ({a.Goals}): "); var goals = Console.ReadLine(); // Edita objetivo
            if (!string.IsNullOrWhiteSpace(goals)) a.Goals = goals
            Console.Write($"Nivel ({a.Level}): "); var level = Console.ReadLine(); // Edita nivel
            if (!string.IsNullOrWhiteSpace(level)) a.Level = level;
            Console.WriteLine("Deportista actualizado.");
            Esperar();
        }
        private static void DeleteAthlete(List<Athlete> athletes) // elimina deportista
        {
            Console.Clear();
            Console.WriteLine("==== ELIMINAR DEPORTISTA ====");
            if (!MostrarListaYValidar(athletes)) return; // si lista vacía, sale
            int index = ReadInt("Número a eliminar: ", 1, athletes.Count); // selecciona índice
            Athlete a = athletes[index - 1]; // Deportista  elimina
            Console.Write($"¿Eliminar a {a.Name}? (s/n): "); 
            if ((Console.ReadLine() ?? "").ToLower() == "s")
            {
                if (currentAthlete == a) currentAthlete = null; // Si era  activo, lo desactiva
                athletes.Remove(a); // lo elimina de la lista
                Console.WriteLine("Eliminado.");
            }
            else Console.WriteLine("Cancelado."); // si no confirma, cancela
            Esperar();
        }
        // ---------------- MÉTODOS DE APOYO ----------------
        private static void Esperar() // Pausa hasta que el usuario presione una tecla
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
        private static bool MostrarListaYValidar(List<Athlete> athletes) // muestra lista y valida que no esté vacía
        {
            if (athletes.Count == 0)
            {
                Console.WriteLine("No hay deportistas.");
                Esperar();
                return false;
            }
            for (int i = 0; i < athletes.Count; i++) // muestra cada deportista
            {
                Console.WriteLine($"{i + 1}. {athletes[i]}");
            }
            return true;
        }    
        
    }
}

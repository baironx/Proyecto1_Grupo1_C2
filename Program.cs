using System;
using System.Collections.Generic;
using System.Linq;
using AppEntrenamientoPersonal.Entidades;
using AppEntrenamientopersonal.Servicios;

namespace AppEntrenamientoPersonal
{
    /// <summary>
    /// Clase principal que contiene el punto de entrada del programa
    /// y toda la lógica de la interfaz de usuario de la consola.
    /// </summary>
    class Programa
    {
        // Variables globales que mantienen el estado de la aplicación
        static List<Atleta> atletas = new List<Atleta>();
        static List<Rutina> rutinas = new List<Rutina>();
        static Atleta? atletaActual = null; 

        /// <summary>
        /// Punto de entrada principal del programa.
        /// Carga datos y ejecuta el bucle del menú principal.
        /// </summary>
        static void Main(string[] args)
        {
            GestorDatos.CargarDatos(out atletas, out rutinas); 

            bool continuar = true;
            while (continuar)
            {
                Console.WriteLine("\n--- Menú Entrenamiento Personal ---");
                Console.WriteLine("1. Gestionar deportistas");
                Console.WriteLine("2. Agregar rutina de entrenamiento");
                Console.WriteLine("3. Mostrar rutinas de entrenamiento");
                Console.WriteLine("4. Editar rutina");
                Console.WriteLine("5. Sugerir rutinas compatibles");
                Console.WriteLine("6. Buscar rutina"); // Nueva funcionalidad
                Console.WriteLine("7. Estadísticas de rutinas"); // Nueva funcionalidad
                Console.WriteLine("8. Guardar y salir");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": GestionarDeportistas(); break;
                    case "2": AgregarRutina(); break;
                    case "3": MostrarRutinas(); break;
                    case "4": EditarRutina(); break;
                    case "5": SugerirRutinas(); break;
                    case "6": BuscarRutina(); break; // Nueva llamada
                    case "7": MostrarEstadisticas(); break; // Nueva llamada
                    case "8":
                        GestorDatos.GuardarDatos(atletas, rutinas); // Guardar datos y salir
                        Console.WriteLine("Datos guardados exitosamente.");
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida, intente de nuevo.");
                        break;
                }
            }
        }
        static void GestionarDeportistas() => ManejarDeportistas();
        static void MostrarDeportistas() => MostrarDeportistasActuales();
        static void AgregarDeportista() => AñadirDeportista();
        static void SeleccionarDeportistaActivo() => SeleccionarDeportistaEnActivo();
        static void EditarDeportista() => EditarInformacionDeportista();
        static void EliminarDeportista() => BorrarDeportista();
        static void AgregarRutina() => AñadirRutina();
        static void MostrarRutinas() => MostrarRutinasDisponibles();
        static void EditarRutina() => EditarInformacionRutina();
        static void SugerirRutinas() => SugerirRutinasCompatibles();


        /// <summary>
        /// Submenú para la gestión completa de deportistas.
        /// Permite crear, editar, eliminar y seleccionar deportistas.
        /// </summary>
        static void ManejarDeportistas()
        {
            while (true)
            {
                Console.WriteLine("\n--- Gestión de Deportistas ---");
                Console.WriteLine("1. Mostrar deportistas");
                Console.WriteLine("2. Agregar nuevo deportista");
                Console.WriteLine("3. Seleccionar deportista activo");
                Console.WriteLine("4. Editar deportista");
                Console.WriteLine("5. Eliminar deportista");
                Console.WriteLine("6. Volver al menú principal");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine() ?? "";

                switch (opcion)
                {
                    case "1": MostrarDeportistasActuales(); break;
                    case "2": AñadirDeportista(); break;
                    case "3": SeleccionarDeportistaEnActivo(); break;
                    case "4": EditarInformacionDeportista(); break;
                    case "5": BorrarDeportista(); break;
                    case "6": return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        /// <summary>
        /// Muestra la lista de todos los deportistas registrados.
        /// Indica qué deportista está actualmente activo.
        /// </summary>
        static void MostrarDeportistasActuales()
        {
            Console.WriteLine("\n--- Lista de Deportistas ---");
            if (atletas.Count == 0)
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }
            // Mostrar cada deportista con su número de índice
            for (int i = 0; i < atletas.Count; i++)
            {
                // Marcar el deportista activo
                string activo = atletas[i] == atletaActual ? " (ACTIVO)" : "";
                Console.WriteLine($"{i + 1}. {atletas[i]}{activo}");
            }
        }

        /// <summary>
        /// Proceso para añadir un nuevo deportista al sistema.
        /// Solicita todos los datos necesarios con validación.
        /// </summary>
        static void AñadirDeportista()
        {
            Console.Write("Nombre del deportista: ");
            string nombre = Console.ReadLine() ?? "";

            // Validar peso con bucle hasta obtener un valor válido
            Console.Write("Peso del deportista (kg): ");
            double peso;
            while (!double.TryParse(Console.ReadLine(), out peso) || peso <= 0)
            {
                Console.Write("Por favor ingrese un peso válido: ");
            }

            // Validar altura con bucle hasta obtener un valor válido
            Console.Write("Altura del deportista (m): ");
            double altura;
            while (!double.TryParse(Console.ReadLine(), out altura) || altura <= 0)
            {
                Console.Write("Por favor ingrese una altura válida: ");
            }

            Console.Write("Objetivos del deportista (Fuerza/Resistencia/Pérdida de peso/Ganancia muscular): ");
            string objetivos = Console.ReadLine() ?? "";

            Console.Write("Nivel del deportista (Principiante/Intermedio/Avanzado): ");
            string nivel = Console.ReadLine() ?? "";

            // Crear nuevo deportista y añadirlo a la lista
            var nuevoAtleta = new Atleta(nombre, peso, altura, objetivos, nivel);
            atletas.Add(nuevoAtleta);

            // Si es el primer deportista, seleccionarlo automáticamente como activo
            if (atletaActual == null)
            {
                atletaActual = nuevoAtleta;
                Console.WriteLine($"Atleta registrado exitosamente y seleccionado como activo.");
            }
            else
            {
                Console.WriteLine("Atleta registrado exitosamente.");
            }
        }

        /// <summary>
        /// Permite seleccionar qué deportista estará activo para las operaciones.
        /// El deportista activo es el que se usa para crear/mostrar rutinas.
        /// </summary>
        static void SeleccionarDeportistaEnActivo()
        {
            if (atletas.Count == 0)
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }

            MostrarDeportistasActuales();
            Console.Write("Seleccione el número del deportista: ");

            // Validar selección y asignar deportista activo
            if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= atletas.Count)
            {
                atletaActual = atletas[indice - 1];
                Console.WriteLine($"Deportista activo: {atletaActual.Nombre}");
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Permite modificar los datos de un deportista existente.
        /// Mantiene los valores actuales si el usuario presiona Enter sin ingresar nada.
        /// </summary>
        static void EditarInformacionDeportista()
        {
            if (atletas.Count == 0) // Verificar si hay deportistas registrados
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }

            // Mostrar lista de deportistas disponibles
            MostrarDeportistasActuales();
            Console.Write("Seleccione el número del deportista a editar: ");
            // Validar la selección del usuario
            if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= atletas.Count)
            {
                // Obtener el deportista seleccionado
                var atleta = atletas[indice - 1];
                Console.WriteLine($"\nEditando: {atleta.Nombre}");
                Console.WriteLine("Presione Enter para mantener el valor actual");

                // Editar nombre (mantener actual si se presiona Enter)
                Console.Write($"Nombre ({atleta.Nombre}): ");
                string nombre = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(nombre)) atleta.Nombre = nombre;

                // Editar peso con validación
                Console.Write($"Peso ({atleta.Peso} kg): ");
                string pesoStr = Console.ReadLine()!;
                if (double.TryParse(pesoStr, out double peso) && peso > 0) atleta.Peso = peso;

                // Editar altura con validación
                Console.Write($"Altura ({atleta.Altura} m): ");
                string alturaStr = Console.ReadLine()!;
                if (double.TryParse(alturaStr, out double altura) && altura > 0) atleta.Altura = altura;

                // Editar objetivos
                Console.Write($"Objetivos ({atleta.Objetivos}): ");
                string objetivos = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(objetivos)) atleta.Objetivos = objetivos;

                // Editar nivel
                Console.Write($"Nivel ({atleta.Nivel}): ");
                string nivel = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(nivel)) atleta.Nivel = nivel;

                Console.WriteLine("Deportista actualizado exitosamente.");
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Elimina un deportista del sistema con confirmación del usuario.
        /// </summary>
        static void BorrarDeportista()
        {
            // Verificar si hay deportistas para eliminar
            if (atletas.Count == 0)
            {
                Console.WriteLine("No hay deportistas registrados.");
                return;
            }

            MostrarDeportistasActuales();
            Console.Write("Seleccione el número del deportista a eliminar: ");

            // Validar la selección del usuario
            if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= atletas.Count)
            {
                // Obtener el deportista a eliminar
                var atletaAEliminar = atletas[indice - 1];
                Console.Write($"¿Está seguro de eliminar a {atletaAEliminar.Nombre}? (s/n): ");

                if (Console.ReadLine()?.ToLower() == "s")
                {
                    // Eliminar el deportista de la lista
                    atletas.RemoveAt(indice - 1);
                    // Si el deportista eliminado era el activo, reiniciar el deportista actual
                    if (atletaActual == atletaAEliminar)
                    {
                        atletaActual = atletas.Count > 0 ? atletas[0] : null;
                    }
                    Console.WriteLine("Deportista eliminado exitosamente.");
                }
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Agrega una nueva rutina para el deportista activo.
        /// Crea rutinas de fuerza o cardio según la selección del usuario.
        /// </summary>
        static void AñadirRutina()
        {
            // Verificar que haya un deportista activo seleccionado
            if (atletaActual == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            // Solicitar tipo de rutina
            Console.Write("Tipo de rutina (Fuerza/Cardio): ");
            string tipo = Console.ReadLine() ?? "";

            // Solicitar duración con validación
            Console.Write("Duración de la rutina (min): ");
            int duracion;
            while (!int.TryParse(Console.ReadLine(), out duracion) || duracion <= 0)
            {
                Console.Write("Por favor ingrese una duración válida: ");
            }

            // Solicitar intensidad
            Console.Write("Intensidad de la rutina (Baja/Media/Alta): ");
            string intensidad = Console.ReadLine() ?? "";

            // Solicitar grupo muscular para rutinas de fuerza
            Console.Write("Grupo muscular (Pecho/Espalda/Piernas/Brazos/Cardio): ");
            string grupoMuscular = Console.ReadLine() ?? "";

            // Nuevo: Fecha en que se realizó la rutina
            Console.Write("Fecha de realización (AAAA-MM-DD, dejar vacío para hoy): ");
            DateTime fechaRealizacion = DateTime.Today; // Inicializar con valor por defecto
            string fechaRealizacionStr = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(fechaRealizacionStr))
            {
                if (!DateTime.TryParse(fechaRealizacionStr, out fechaRealizacion))
                {
                    Console.WriteLine("Formato de fecha inválido. Se usará la fecha actual.");
                    fechaRealizacion = DateTime.Today;
                }
            }

            // Nuevo: Fecha en que vence la rutina
            Console.Write("Fecha de vencimiento (AAAA-MM-DD, dejar vacío si no aplica): ");
            DateTime? fechaVencimiento = null;
            string fechaVencimientoStr = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(fechaVencimientoStr) && DateTime.TryParse(fechaVencimientoStr, out DateTime parsedFechaVencimiento))
            {
                fechaVencimiento = parsedFechaVencimiento;
            }
            else if (string.IsNullOrWhiteSpace(fechaVencimientoStr))
            {
                fechaVencimiento = null; // Asegurarse de que sea nulo si no se ingresa nada
            }

            // Nuevo: Lesiones post-entrenamiento
            Console.Write("Lesiones producidas post-entrenamiento (dejar vacío si no hay): ");
            string lesionesPostEntrenamiento = Console.ReadLine() ?? "";

            // Crear la rutina según el tipo especificado
            switch (tipo.ToLower())
            {
                case "fuerza":
                    rutinas.Add(new RutinaFuerza(duracion, intensidad, grupoMuscular, atletaActual.Nombre, fechaRealizacion, fechaVencimiento, lesionesPostEntrenamiento));
                    Console.WriteLine("Rutina de fuerza agregada exitosamente.");
                    break;
                case "cardio":
                    rutinas.Add(new RutinaCardio(duracion, intensidad, grupoMuscular, atletaActual.Nombre, fechaRealizacion, fechaVencimiento, lesionesPostEntrenamiento));
                    Console.WriteLine("Rutina de cardio agregada exitosamente.");
                    break;
                default:
                    Console.WriteLine("Tipo de rutina no válido. Debe ser 'Fuerza' o 'Cardio'.");
                    break;
            }
        }

        /// <summary>
        /// Muestra todas las rutinas del deportista activo.
        /// </summary>
        static void MostrarRutinasDisponibles()
        {
            // Verificar que haya un deportista activo
            if (atletaActual == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            Console.WriteLine($"\n--- Rutinas de {atletaActual.Nombre} ---");
            var rutinasAtleta = rutinas.Where(r => r.NombreAtleta == atletaActual.Nombre).ToList(); // Filtrar rutinas del atleta activo

            if (rutinasAtleta.Count == 0) // Verificar si el atleta tiene rutinas
            {
                Console.WriteLine($"No hay rutinas registradas para {atletaActual.Nombre}.");
                return;
            }

            // Mostrar cada rutina con su número de índice
            for (int i = 0; i < rutinasAtleta.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {rutinasAtleta[i].Describir()}");
            }
        }

        /// <summary>
        /// Permite modificar una rutina existente para el deportista activo.
        /// </summary>
        static void EditarInformacionRutina()
        {
            // Verificar si hay un deportista activo y si tiene rutinas
            if (atletaActual == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            var rutinasAtleta = rutinas.Where(r => r.NombreAtleta == atletaActual.Nombre).ToList();
            if (rutinasAtleta.Count == 0)
            {
                Console.WriteLine($"No hay rutinas para editar para {atletaActual.Nombre}.");
                return;
            }

            MostrarRutinasDisponibles();
            Console.Write("Seleccione el número de la rutina a editar: ");

            // Validar la selección de la rutina
            if (int.TryParse(Console.ReadLine(), out int indice) && indice > 0 && indice <= rutinasAtleta.Count)
            {
                var rutina = rutinasAtleta[indice - 1]; // Obtener la rutina seleccionada
                Console.WriteLine($"\nEditando: {rutina.Describir()}");
                Console.WriteLine("Presione Enter para mantener el valor actual");

                // Editar duración con validación
                Console.Write($"Duración ({rutina.Duracion} min): ");
                string duracionStr = Console.ReadLine()!;
                if (int.TryParse(duracionStr, out int duracion) && duracion > 0) rutina.Duracion = duracion;

                // Editar intensidad
                Console.Write($"Intensidad ({rutina.Intensidad}): ");
                string intensidad = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(intensidad)) rutina.Intensidad = intensidad;

                // Editar grupo muscular
                Console.Write($"Grupo Muscular ({rutina.GrupoMuscular}): ");
                string grupoMuscular = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(grupoMuscular)) rutina.GrupoMuscular = grupoMuscular;

                // Editar fecha de realización
                Console.Write($"Fecha de Realización ({rutina.FechaRealizacion.ToShortDateString()} - AAAA-MM-DD): ");
                string fechaRealizacionStr = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(fechaRealizacionStr) && DateTime.TryParse(fechaRealizacionStr, out DateTime fechaRealizacion))
                {
                    rutina.FechaRealizacion = fechaRealizacion;
                }

                // Editar fecha de vencimiento
                Console.Write($"Fecha de Vencimiento ({(rutina.FechaVencimiento.HasValue ? rutina.FechaVencimiento.Value.ToShortDateString() : "No aplica")} - AAAA-MM-DD, dejar vacío si no aplica): ");
                string fechaVencimientoStr = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(fechaVencimientoStr))
                {
                    if (DateTime.TryParse(fechaVencimientoStr, out DateTime parsedFechaVencimiento))
                    {
                        rutina.FechaVencimiento = parsedFechaVencimiento;
                    }
                    else
                    {
                        Console.WriteLine("Formato de fecha de vencimiento inválido. No se actualizará.");
                    }
                }
                else
                {
                    rutina.FechaVencimiento = null; // Si se deja vacío, se establece como nulo
                }

                // Editar lesiones post-entrenamiento
                Console.Write($"Lesiones Post-Entrenamiento ({rutina.LesionesPostEntrenamiento}): ");
                string lesionesPostEntrenamiento = Console.ReadLine()!;
                if (!string.IsNullOrWhiteSpace(lesionesPostEntrenamiento)) rutina.LesionesPostEntrenamiento = lesionesPostEntrenamiento;

                Console.WriteLine("Rutina actualizada exitosamente.");
            }
            else
            {
                Console.WriteLine("Número inválido.");
            }
        }

        /// <summary>
        /// Sugiere rutinas de entrenamiento basadas en los objetivos y el nivel del deportista activo.
        /// </summary>
        static void SugerirRutinasCompatibles()
        {
            // Verificar si hay un deportista activo
            if (atletaActual == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero para obtener sugerencias.");
                return;
            }

            Console.WriteLine($"\n--- Sugerencias de Rutinas para {atletaActual.Nombre} ({atletaActual.Nivel} - {atletaActual.Objetivos}) ---");
            // Obtener sugerencias usando el servicio
            var sugerencias = ServicioSugerenciaRutina.ObtenerRutinasSugeridas(atletaActual); // Usar la clase traducida

            if (sugerencias.Any())
            {
                foreach (var sugerencia in sugerencias)
                {
                    Console.WriteLine($"- {sugerencia}");
                }
            }
            else
            {
                Console.WriteLine("No se encontraron sugerencias de rutinas para este deportista.");
            }
        }

        /// <summary>
        /// Permite al usuario buscar rutinas por un término dado.
        /// </summary>
        static void BuscarRutina()
        {
            if (atletaActual == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            Console.Write("Ingrese un término de búsqueda para las rutinas (ej. 'Cardio', 'Fuerza', 'Piernas'): ");
            string terminoBusqueda = Console.ReadLine()?.ToLower() ?? "";

            var rutinasEncontradas = rutinas.Where(r => r.NombreAtleta == atletaActual.Nombre &&
                                                        (r.Tipo.ToLower().Contains(terminoBusqueda) ||
                                                         r.Intensidad.ToLower().Contains(terminoBusqueda) ||
                                                         r.GrupoMuscular.ToLower().Contains(terminoBusqueda) ||
                                                         r.Describir().ToLower().Contains(terminoBusqueda)))
                                            .ToList();

            if (rutinasEncontradas.Any())
            {
                Console.WriteLine($"\n--- Rutinas encontradas para '{terminoBusqueda}' para {atletaActual.Nombre} ---");
                foreach (var rutina in rutinasEncontradas)
                {
                    Console.WriteLine($"- {rutina.Describir()}");
                }
            }
            else
            {
                Console.WriteLine($"No se encontraron rutinas para '{terminoBusqueda}' para {atletaActual.Nombre}.");
            }
        }

        /// <summary>
        /// Muestra estadísticas generales y específicas sobre las rutinas del deportista activo.
        /// </summary>
        static void MostrarEstadisticas()
        {
            if (atletaActual == null)
            {
                Console.WriteLine("Debe seleccionar un deportista activo primero.");
                return;
            }

            var rutinasAtleta = rutinas.Where(r => r.NombreAtleta == atletaActual.Nombre).ToList();
            if (!rutinasAtleta.Any())
            {
                Console.WriteLine($"No hay rutinas registradas para {atletaActual.Nombre} para mostrar estadísticas.");
                return;
            }

            Console.WriteLine($"\n--- Estadísticas de Rutinas para {atletaActual.Nombre} ---");

            // 1. Número total de rutinas
            Console.WriteLine($"\nTotal de rutinas registradas: {rutinasAtleta.Count}");

            // 2. Duración promedio de las rutinas
            double duracionPromedio = rutinasAtleta.Average(r => r.Duracion);
            Console.WriteLine($"Duración promedio de las rutinas: {duracionPromedio:F2} minutos");

            // 3. Conteo de rutinas por tipo
            Console.WriteLine("\nConteo de rutinas por tipo:");
            var conteoPorTipo = rutinasAtleta.GroupBy(r => r.Tipo)
                                            .Select(g => new { Tipo = g.Key, Cantidad = g.Count() });
            foreach (var item in conteoPorTipo)
            {
                Console.WriteLine($"- {item.Tipo}: {item.Cantidad}");
            }

            // 4. Fechas de lesiones post-entrenamiento
            Console.WriteLine("\nFechas de lesiones post-entrenamiento:");
            var rutinasConLesion = rutinasAtleta.Where(r => !string.IsNullOrEmpty(r.LesionesPostEntrenamiento)).ToList();
            if (rutinasConLesion.Any())
            {
                foreach (var rutina in rutinasConLesion)
                {
                    Console.WriteLine($"- Fecha: {rutina.FechaRealizacion.ToShortDateString()}, Lesión: {rutina.LesionesPostEntrenamiento}");
                }
            }
            else
            {
                Console.WriteLine("No se registraron lesiones post-entrenamiento.");
            }

            // 5. Rutinas con vencimiento en los próximos 30 días
            Console.WriteLine("\nRutinas con vencimiento en los próximos 30 días:");
            DateTime hoy = DateTime.Today;
            DateTime fechaLimite = hoy.AddDays(30);
            var rutinasProximoVencimiento = rutinasAtleta.Where(r => r.FechaVencimiento.HasValue &&
                                                                   r.FechaVencimiento.Value >= hoy &&
                                                                   r.FechaVencimiento.Value <= fechaLimite)
                                                       .OrderBy(r => r.FechaVencimiento)
                                                       .ToList();
            if (rutinasProximoVencimiento.Any())
            {
                foreach (var rutina in rutinasProximoVencimiento)
                {
                    Console.WriteLine($"- {rutina.Describir()} - Vence: {rutina.FechaVencimiento.Value.ToShortDateString()}");
                }
            }
            else
            {
                Console.WriteLine("No hay rutinas que venzan en los próximos 30 días.");
            }


            // 6. Listar las 3 rutinas que le tomó más tiempo realizar en el último mes.
            Console.WriteLine("\nLas 3 rutinas más largas en el último mes:");
            var unMesAtras = DateTime.Today.AddMonths(-1);
            var rutinasUltimoMes = rutinasAtleta.Where(r => r.FechaRealizacion >= unMesAtras).ToList();

            var top3RutinasMasLargas = rutinasUltimoMes
                                        .OrderByDescending(r => r.Duracion)
                                        .Take(3)
                                        .ToList();
            if (top3RutinasMasLargas.Any())
            {
                foreach (var rutina in top3RutinasMasLargas)
                {
                    Console.WriteLine($"- {rutina.Describir()}");
                }
            }
            else
            {
                Console.WriteLine("No hay suficientes rutinas en el último mes para mostrar las 3 más largas.");
            }
        }
    }
}
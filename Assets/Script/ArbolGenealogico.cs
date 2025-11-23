/*
    Representa la estructura principal del árbol genealógico.

    Mantiene la referencia a la persona raíz y permite agregar
        nuevos familiares enlazando padre y/o madre.

    Incluye métodos para buscar personas por cédula y
        mostrar el árbol completo en consola.
*/
using System;
using System.Collections.Generic;

namespace ArbolGenealogico.Domain 
{
    public static class GeoUtils  
    {
        // Fórmula de Haversine para calcular distancia en kilómetros
        public static double CalcularDistancia(List<double> coord1, List<double> coord2)
        {
            const double RadioTierra = 6371; // km

            double lat1Rad = coord1[0] * Math.PI / 180.0;
            double lon1Rad = coord1[1] * Math.PI / 180.0;
            double lat2Rad = coord2[0] * Math.PI / 180.0;
            double lon2Rad = coord2[1] * Math.PI / 180.0;

            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return RadioTierra * c;
        }
    }

    public class ArbolGenealogico    // Clase que representa el árbol genealógico completo.
    {
        public NodoGenealogico Raiz { get; set; } // Nodo raíz del árbol.

        /// Constructor para inicializar el árbol con una persona raíz.
        public ArbolGenealogico(Persona personaRaiz)
        {
            Raiz = new NodoGenealogico(personaRaiz, null, null); // Crear el nodo raíz sin padres.
        }

        /// Agrega un nuevo familiar al árbol.
        /// Busca por cédula al padre y/o madre y enlaza al nuevo nodo.

        public void AgregarFamiliar(Persona nuevaPersona, string cedulaPadre = null, string cedulaMadre = null)
        {
            NodoGenealogico nuevoNodo = new NodoGenealogico(nuevaPersona, null, null); // Crear nodo para la nueva persona.

            if (cedulaPadre != null) // Si se especifica padre
            {
                NodoGenealogico padre = BuscarPorCedula(Raiz, cedulaPadre); // Buscar nodo padre.
                if (padre != null)
                {
                    padre.AgregarHijo(nuevoNodo); // Enlazar hijo al padre.
                    nuevoNodo.Padre = padre; // Enlazar padre al hijo.
                }
            }

            if (cedulaMadre != null) // Si se especifica madre
            {
                NodoGenealogico madre = BuscarPorCedula(Raiz, cedulaMadre); // Buscar nodo madre.
                if (madre != null)
                {
                    madre.AgregarHijo(nuevoNodo); // Enlazar hijo a la madre.
                    nuevoNodo.Madre = madre; // Enlazar madre al hijo.
                }
            }
        }

        /// Busca un nodo en el árbol por cédula.
        public NodoGenealogico BuscarPorCedula(NodoGenealogico nodo, string cedula)
        {
            if (nodo == null) return null; // Caso base: nodo vacío.
            if (nodo.Persona.Cedula == cedula) return nodo; // Si coincide la cédula, devolver nodo.

            foreach (var hijo in nodo.Hijos) // Recorrer hijos recursivamente.
            {
                var resultado = BuscarPorCedula(hijo, cedula);
                if (resultado != null) return resultado;
            }

            return null; // No encontrado.
        }

        /// Muestra el árbol en consola en recorrido preorden.
        public void MostrarArbol(NodoGenealogico nodo = null, int nivel = 0)
        {
            if (nodo == null) nodo = Raiz; // Si no se pasa nodo, iniciar en la raíz.

            Console.WriteLine(new string('-', nivel * 2) + nodo.Persona.Nombre); // Imprimir con indentación.

            foreach (var hijo in nodo.Hijos) // Recorrer hijos.
            {
                MostrarArbol(hijo, nivel + 1); // Llamada recursiva con mayor nivel.
            }
        }

        /// Método para listar personas con su ubicación.
        public List<string> ListarPersonasConUbicacion() 
        {
            List<string> resultado = new List<string>();
            Recorrer(Raiz, resultado);
            return resultado;
        }

        
        private void Recorrer(NodoGenealogico nodo, List<string> lista)
        {
            if (nodo == null) return;

            var p = nodo.Persona;
            string info = $"{p.Nombre} - {p.Parentesco}";
            lista.Add(info);

            foreach (var hijo in nodo.Hijos)
            {
                Recorrer(hijo, lista);
            }
        }

        /// Metodo para mostrar el árbol genealógico completo.
        public void MostrarArbolGenealogico() 
        {
            MostrarNodo(Raiz, 0);
        }

        private void MostrarNodo(NodoGenealogico nodo, int nivel)   // Nivel indica la profundidad en el árbol
        {
            if (nodo == null) return;

            var p = nodo.Persona;
            string indentacion = new string(' ', nivel * 4); // 4 espacios por nivel
            string linea = $"{indentacion}- {p.Nombre}, Edad: {p.Edad})";
            linea += $" - {p.Parentesco}";
            Console.WriteLine(linea);

            foreach (var hijo in nodo.Hijos)
            {
                MostrarNodo(hijo, nivel + 1);
            }
        }
    }
}

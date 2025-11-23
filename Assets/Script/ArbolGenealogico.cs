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
        /// Calcula la distancia entre dos coordenadas (x, y) usando la fórmula de distancia euclidiana.
        public static double CalcularDistancia(List<double> coord1, List<double> coord2)
        {
            double x1 = coord1[0];
            double y1 = coord1[1];
            double x2 = coord2[0];
            double y2 = coord2[1];
            
            double dx = x2 - x1;
            double dy = y2 - y1;

            double result = (float)Math.Sqrt(dx * dx + dy * dy);

            return result*0.04f*1000; // Aproximación a escala en km, 0,04 viene del tamaño de los pixeles maximos en x 
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

/*
    Permite calcular métricas como cantidad de personas vivas/fallecidas,
    promedio de edad y total de individuos registrados.
    Se apoya en los nodos del árbol para recorrer y obtener datos.
*/

using System;
using System.Linq;
using ArbolGenealogico.Domain;
using UnityEngine;


namespace ArbolGenealogico.Services
{
    public class ModuloEstadisticas : MonoBehaviour // Clase que calcula estadísticas sobre el árbol.
    {
        private ArbolGenealogico.Domain.ArbolGenealogico arbol; // Referencia al árbol genealógico.

        /// Constructor para inicializar el módulo con un árbol.
        public ModuloEstadisticas(ArbolGenealogico.Domain.ArbolGenealogico arbol)
        {
            this.arbol = arbol; // Asigna el árbol recibido.
        }

        public int TotalPersonas()
        {
            return ContarPersonas(arbol.Raiz); // Llama al método recursivo para contar.
        }
        public double PromedioEdad()
        {
            var edades = ObtenerEdades(arbol.Raiz); // Obtiene todas las edades.
            return edades.Count > 0 ? edades.Average() : 0; // Calcula el promedio si hay datos.
        }

        private int ContarPersonas(NodoGenealogico nodo)
        {
            if (nodo == null) return 0; // Caso base: nodo vacío.
            int total = 1; // Contar la persona actual.
            foreach (var hijo in nodo.Hijos) total += ContarPersonas(hijo); // Sumar hijos recursivamente.
            return total;
        }


        private System.Collections.Generic.List<int> ObtenerEdades(NodoGenealogico nodo)
        {
            var edades = new System.Collections.Generic.List<int>(); // Lista de edades.
            if (nodo == null) return edades; // Caso base: nodo vacío.

            edades.Add(nodo.Persona.Edad); // Agregar edad de la persona actual.
            foreach (var hijo in nodo.Hijos) edades.AddRange(ObtenerEdades(hijo)); // Agregar edades de hijos.
            return edades;
        }
    }
}

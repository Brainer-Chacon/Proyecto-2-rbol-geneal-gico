/*
        Controlador que gestiona las distancias entre personas.

        Utiliza el grafo de residencias para calcular la distancia
            geográfica entre dos individuos del árbol genealógico.
            
        Permite registrar personas en el grafo y consultar
            distancias de manera sencilla.
    */

using System;
using ArbolGenealogico.Domain;
using UnityEngine;

namespace ArbolGenealogico.Services
{
    public class ControladorDistancias : MonoBehaviour    // Clase que controla el cálculo de distancias entre personas.
    {
        private GrafoResidencias grafo; // Instancia del grafo de residencias.

        /// Constructor para inicializar el controlador con un grafo vacío.
        public ControladorDistancias()
        {
            grafo = new GrafoResidencias(); // Inicializa el grafo.
        }

        /// Registra una persona en el grafo usando su información.
        public void RegistrarPersona(Persona persona)
        {
            grafo.AgregarPersona(persona.Cedula, persona.Coordenadas[0], persona.Coordenadas[1]); // Agrega la persona al grafo.
        }

        /// Calcula la distancia entre dos personas usando sus cédulas.
    }
}

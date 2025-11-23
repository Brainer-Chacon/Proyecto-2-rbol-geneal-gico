/*
        Representa un grafo de residencias.

        Cada persona se conecta mediante sus coordenadas de ubicación
            y se calculan las distancias entre ellas.
            
        Permite agregar personas como nodos y calcular la distancia
            geográfica entre dos individuos.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArbolGenealogico.Services
{
    public class GrafoResidencias : MonoBehaviour   // Grafo que modela las conexiones espaciales entre personas.
    {
        private Dictionary<string, (double Latitud, double Longitud)> nodos; // Diccionario de cédula → coordenadas.

        /// Constructor para inicializar el grafo vacío.
        public GrafoResidencias()
        {
            nodos = new Dictionary<string, (double Latitud, double Longitud)>(); // Inicializa el diccionario vacío.
        }

        /// Agrega una persona al grafo usando su cédula y coordenadas.
        public void AgregarPersona(string cedula, double latitud, double longitud)
        {
            nodos[cedula] = (latitud, longitud); // Inserta o actualiza las coordenadas de la persona.
        }

        /// Método auxiliar para calcular distancia geográfica usando fórmula de Haversine.
        private double DistanciaGeografica(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // Radio de la Tierra en km.
            double dLat = (lat2 - lat1) * Math.PI / 180; // Diferencia de latitud en radianes.
            double dLon = (lon2 - lon1) * Math.PI / 180; // Diferencia de longitud en radianes.

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2); // Fórmula de Haversine.

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)); // Ángulo central.

            return R * c; // Distancia en kilómetros.
        }
    }
}

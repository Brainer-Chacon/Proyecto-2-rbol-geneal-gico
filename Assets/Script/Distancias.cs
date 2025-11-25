
using System.Collections.Generic;
using System;

public static class Distancias
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
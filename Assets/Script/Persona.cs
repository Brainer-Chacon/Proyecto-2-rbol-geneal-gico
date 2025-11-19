/*
    Contiene datos esenciales como cédula, nombre,
        fecha de nacimiento, edad, estado y coordenadas.

    También incluye la ruta de la imagen asociada a la persona.
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ArbolGenealogico.Domain
{
    public class Persona  // Clase que representa a una persona en el árbol genealógico.
    {
        public string Nombre { get; set; }  // Nombre de la persona.
        public string Cedula { get; set; } // Cédula única de la persona.
        public string PaisResidencia { get; set; }  // País de residencia.
        public DateTime FechaNacimiento { get; set; } // Fecha de nacimiento.
        public int Edad { get; set; } // Edad calculada automáticamente.
        public List<double> Coordenadas { get; set; } // Latitud y longitud de la residencia.
        public string RutaImagen { get; set; } // Ruta de la imagen asociada a la persona.
    
        

        /// Constructor para inicializar una nueva instancia de Persona.
        /// Recibe cédula, nombre, fecha de nacimiento, coordenadas, ruta de imagen y estado vital.
        public Persona(string nombre, int edad, string cedula, DateTime fechaNacimiento, 
        List<double> coordenadas, string rutaImagen, string paisResidencia)
        {
            Nombre = nombre;
            Edad = edad;
            Cedula = cedula;
            FechaNacimiento = fechaNacimiento;
            Coordenadas = coordenadas;
            RutaImagen = rutaImagen;
            PaisResidencia = paisResidencia;
        }



        /// Devuelve una representación textual de la persona.
        /// Incluye nombre, cédula, edad, estado y ubicación.
        public override string ToString()
        {
            return $"{Nombre} ({Cedula}) - Edad: {Edad} años - Ubicación: {Coordenadas[0]}, {Coordenadas[1]}";
        }
    }

}

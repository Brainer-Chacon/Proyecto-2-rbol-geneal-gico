using UnityEngine;
using TMPro;
using System.Globalization;
using ArbolGenealogico.Domain;
using System;
using System.Collections.Generic;

public class FormularioPersona : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public TMP_InputField inputImagen;
    public TMP_InputField inputCedula;
    public TMP_InputField inputEdad;
    public TMP_InputField inputCumple;
    public TMP_InputField inputParentesco;
    public TMP_InputField inputCoords;

    private Persona personaActual;

    public static List<Persona> personas = new List<Persona>(); 

    // Este método lo llamas con el boton 
    public void CrearPersonaDesdeUI()
    {
        string nombre = inputNombre.text;

        string cedula = inputCedula.text;

        string rutaImagen = inputImagen.text;

        string paisResidencia = inputParentesco.text;

        // Leer entero
        if (!int.TryParse(inputEdad.text, out int edad))
        {
            Debug.LogWarning("Edad inválida");
        }

        if(edad < 0)
        {
            Debug.LogWarning("Edad no puede ser negativa");
        }

        // Leer lista de coordenadas (ejemplo: "12.34,56.78")
        System.Collections.Generic.List<double> coordenadas = new System.Collections.Generic.List<double>();
        string coordsInput = inputCoords?.text ?? "";

        if (string.IsNullOrWhiteSpace(coordsInput))
        {
            Debug.LogWarning("Coords inválidos (vacío)");
        }

        
        var parts = coordsInput.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (!double.TryParse(part.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                Debug.LogWarning($"Coord inválida: '{part}'");
            }
            coordenadas.Add(value);
        }

        if (coordenadas.Count == 0)
        {
            Debug.LogWarning("No se encontraron coordenadas válidas");
        }

        string[] formatos = { "dd/MM/yyyy", "dd-MM-yyyy","yyyy/MM/dd", "yyyy-MM-dd" };
   
        // Leer DateTime (fecha)
        if (!DateTime.TryParseExact(
                inputCumple.text,
                formatos,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime fechaNacimiento))
        {
            Debug.LogWarning("Fecha inválida");
        }

        // Crear instancia de la clase
        personaActual = new Persona(nombre, edad, cedula, fechaNacimiento, coordenadas, rutaImagen, paisResidencia);

        personas.Add(personaActual);

        Debug.Log("Total personas: " + personas.Count);
       
        Debug.Log("Persona creada: " + personaActual);

    }
    public static List<(string name1, string name2, double dis)> TodasDistancias()
    {
        if(personas.Count < 2)
        {
            Debug.LogWarning("No hay suficientes personas para calcular distancias.");
            return new List<(string name1, string name2, double dis)> {("null", "null", 0)};
        }
        List<(string name1, string name2, double dis)> resultado = 
        new List<(string name1, string name2, double dis)>();
        for (int i = 0; i < personas.Count; i++)
        {
            for (int j = i + 1; j < personas.Count; j++)
            {
                double distancia = GeoUtils.CalcularDistancia(
                    personas[i].Coordenadas,
                    personas[j].Coordenadas);
                resultado.Add((personas[i].Nombre, personas[j].Nombre, distancia));
            }
        }
        return resultado;
    }
}

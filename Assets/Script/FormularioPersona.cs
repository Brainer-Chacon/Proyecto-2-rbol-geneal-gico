using UnityEngine;
using TMPro;
using System.Globalization;
using ArbolGenealogico.Domain;
using System;
using System.ComponentModel;

public class FormularioPersona : MonoBehaviour
{
    public TMP_InputField inputNombre;
    public TMP_InputField inputImagen;
    public TMP_InputField inputCedula;
    public TMP_InputField inputEdad;
    public TMP_InputField inputCumple;
    public TMP_InputField inputPais;
    public TMP_InputField inputCoords;
    public TextMeshProUGUI textoResultado;   // opcional

    private Persona personaActual;

    // Este método lo vas a llamar desde el botón
    public void CrearPersonaDesdeUI()
    {
        string nombre = inputNombre.text;

        string cedula = inputCedula.text;

        string rutaImagen = inputImagen.text;

        string paisResidencia = inputPais.text;

        // Leer entero
        if (!int.TryParse(inputEdad.text, out int edad))
        {
            Debug.LogWarning("Edad inválida");
            if (textoResultado != null)
                textoResultado.text = "Edad inválida";
            return;
        }

        // Leer lista de coordenadas (ejemplo: "12.34,56.78")
        System.Collections.Generic.List<double> coordenadas = new System.Collections.Generic.List<double>();
        string coordsInput = inputCoords?.text ?? "";

        if (string.IsNullOrWhiteSpace(coordsInput))
        {
            Debug.LogWarning("Coords inválidos (vacío)");
            if (textoResultado != null)
                textoResultado.text = "Coords inválidos";
            return;
        }

        
        var parts = coordsInput.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            if (!double.TryParse(part.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                Debug.LogWarning($"Coord inválida: '{part}'");
                if (textoResultado != null)
                    textoResultado.text = "Coords inválidos";
                return;
            }
            coordenadas.Add(value);
        }

        if (coordenadas.Count == 0)
        {
            Debug.LogWarning("No se encontraron coordenadas válidas");
            if (textoResultado != null)
                textoResultado.text = "Coords inválidos";
            return;
        }       

        // Leer DateTime (fecha)
        if (!DateTime.TryParse(
                inputCumple.text,
                CultureInfo.InvariantCulture,   // IFormatProvider primero
                DateTimeStyles.None,            // luego DateTimeStyles
                out DateTime fechaNacimiento))
        {
            Debug.LogWarning("Fecha inválida");
            if (textoResultado != null)
                textoResultado.text = "Fecha inválida";
            return;
        }

        // Crear instancia de la clase
        personaActual = new Persona(nombre, edad, cedula, fechaNacimiento, coordenadas, rutaImagen, paisResidencia);

        Debug.Log("Persona creada: " + personaActual);

        if (textoResultado != null)
            textoResultado.text = "Creado: " + personaActual.ToString();
    }
}

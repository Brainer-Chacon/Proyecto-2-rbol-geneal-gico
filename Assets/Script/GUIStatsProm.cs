using UnityEngine;
using TMPro;
using System.Collections.Generic; // alias para desambiguar

public class GUIStatsProm : MonoBehaviour // Clase para mostrar el promedio de distancias en la UI.
{
    public TextMeshProUGUI myText;  

    void Start()
    {
        // Llama a la función

        List<(string name1, string name2, double dis)> Distancias =
        FormularioPersona.TodasDistancias();
        
        double resultado = ModuloEstadisticas.Promedio(Distancias);
        
        // Cambia texto según resultado
        CambiarTexto(resultado);
    }

    void CambiarTexto(double valor)
    {
        if (myText == null) { Debug.LogWarning("myText is null"); return; }
        myText.text = $"{valor.ToString("0.00")} km";
        
    }
}

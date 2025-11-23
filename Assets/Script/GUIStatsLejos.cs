using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIStatsLejos : MonoBehaviour // Clase para mostrar el par más lejano en la UI.
{
    public TextMeshProUGUI myText;  

    void Start()
    {
        // Llama a la función

        List<(string name1, string name2, double dis)> Distancias =
            FormularioPersona.TodasDistancias();

        (string , string, double) resultado = ModuloEstadisticas.ParMasLejos(Distancias);
        
        // Cambia texto según resultado
        CambiarTexto(resultado);
    }
    void CambiarTexto((string name1 , string name2 , double dis) valor)
    {
        if (myText == null) { Debug.LogWarning("myText is null"); return; }
        myText.text = $"{valor.name1} y {valor.name2}";
        
    }
}

using UnityEngine;
using TMPro;
using Random = UnityEngine.Random; // alias para desambiguar

public class TextChanger : MonoBehaviour
{
    public TextMeshProUGUI myText;  

    void Start()
    {
        // Llama a tu función
        int resultado = MiFuncion();
        
        // Cambia texto según resultado
        CambiarTexto(resultado);
    }

    int MiFuncion()
    {
        // Aquí va tu lógica
        return Random.Range(0, 100);
    }

    void CambiarTexto(int valor)
    {
        if (myText == null) { Debug.LogWarning("myText is null"); return; }
        myText.text = "Polo y Ale";
        
    }
}

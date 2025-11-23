using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using ArbolGenealogico.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIMAPA : MonoBehaviour
{
    public Button buttonPrefab;
    public Transform parentUI;

    public Image linePrefab;
    
    public static Dictionary<string, RectTransform> Nodos = new();

    public static Dictionary<string, List<GameObject>> lineasPorNodo 
    = new Dictionary<string, List<GameObject>>();

    public static Dictionary<string, List<GameObject>> DistanciaNodo 
    = new Dictionary<string, List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        List<Persona> Arbol = FormularioPersona.personas;

        // Spawnear nodos en el mapa
        SpawnearNodo(Arbol);
    }

    // Lógica para spawnear el nodo en el mapa
    void SpawnearNodo(List<Persona> dato)
    {
        for(int i = 0; i < dato.Count; i++)
        {
            Persona personaActual = dato[i];
            string ruta = $"Images/{personaActual.RutaImagen}";
            
            // Instanciar como hijo del canvas/panel
            Button nuevoBoton = Instantiate(buttonPrefab, parentUI);
            nuevoBoton.onClick.AddListener(() => OnNodoClicked(personaActual));
            
            // Cambiar la imagen del botón
            Image img = nuevoBoton.GetComponentInChildren<Image>();
            Sprite sprite = Resources.Load<Sprite>(ruta);
            
            if (sprite == null)
            {
                Debug.LogWarning($"No se encontró la imagen en Resources: {ruta}");
                img.sprite = Resources.Load<Sprite>("Images/user"); // Cargar una imagen por defecto si no se encuentra
            }
            else
            {
                img.sprite = sprite;
            }
            
            float x = (float)dato[i].Coordenadas[0];
            float y = (float)dato[i].Coordenadas[1];

            // Asegurar escala correcta
            nuevoBoton.transform.localScale = Vector3.one;

            // Cambiar el texto del botón (Text o TMP_Text)
            var label = nuevoBoton.GetComponentInChildren<TMPro.TMP_Text>();
            if (label != null)
                label.text = dato[i].Nombre; // o lo que quieras mostrar

            // Si quieres controlar posición manual:
            RectTransform rt = nuevoBoton.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(x, y);

            Nodos[personaActual.Nombre] = rt;
            
            Debug.Log($"Spawneando nodo {personaActual.Nombre}, en coordenadas {personaActual.Coordenadas} ");
        }
    }

    public void OnNodoClicked(Persona persona)
    {
        string nombre = persona.Nombre;

        // If lines are already drawn → delete them and exit
        if (lineasPorNodo.ContainsKey(nombre))
        {
            foreach (var linea in lineasPorNodo[nombre])
                Destroy(linea);

            lineasPorNodo.Remove(nombre);

            Debug.Log($"Lines for {nombre} removed.");
            return; // 👈 prevents drawing again immediately
        }

        // If lines are NOT drawn yet → draw them now
        List<(string name1, string name2, double distancia)> enlaces =
            BuscarNDistancias(persona.Nombre, FormularioPersona.TodasDistancias());

        List<GameObject> lineasCreadas = new List<GameObject>();

        foreach (var item in enlaces)
        {
            if (!Nodos.TryGetValue(item.name1, out RectTransform nodo1) ||
                !Nodos.TryGetValue(item.name2, out RectTransform nodo2))
                continue;

            GameObject linea = CreateLine(nodo1, nodo2, item.distancia);
            lineasCreadas.Add(linea);
            
            Debug.Log($"Connecting {item.name1} with {item.name2}");
        }

        lineasPorNodo[nombre] = lineasCreadas;
    }

    public List<(string name1, string name2, double distancia)> BuscarNDistancias( 
    string nombre, List<(string name1, string name2, double distancia)> enlaces)// funcion para buscar todas las distancias que tiene un nodo con los demas 
    {
        List<(string name1, string name2, double distancia)> resultado = new();

        foreach (var item in enlaces)
        {
            if (item.name1 == nombre || item.name2 == nombre)
            {
                resultado.Add(item);
            }
        }
        return resultado;
    } 

    public GameObject CreateLine(RectTransform a, RectTransform b, double distancia)
    {
        Image line = Instantiate(linePrefab, parentUI);   // linePrefab = Image con hijo TMP_Text
        RectTransform rt = line.rectTransform;

        rt.SetParent(parentUI, false);
        rt.localScale = Vector3.one;

        Vector2 posA = a.anchoredPosition;
        Vector2 posB = b.anchoredPosition;

        Vector2 center = (posA + posB) * 0.5f;
        rt.anchoredPosition = center;

        Vector2 dir = posB - posA;
        float length = dir.magnitude;

        float thickness = 1f; // 👈 grosor fijo
        rt.sizeDelta = new Vector2(length, thickness);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        // Texto de la distancia
        var label = line.GetComponentInChildren<TMPro.TMP_Text>();
       
        if (label != null)   
        {
            label.text = distancia.ToString("F2") + "km";
            label.fontSize = 15;
            label.color = Color.white; 
            label.enableAutoSizing = false;
            label.alignment = TMPro.TextAlignmentOptions.Center;
            label.rectTransform.localRotation = Quaternion.Euler(0, 0, -angle);
            label.rectTransform.anchoredPosition = new Vector2(0, 20);
        }

        return line.gameObject;
    }
}
    
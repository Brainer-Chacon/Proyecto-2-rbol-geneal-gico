using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using ArbolGenealogico.Domain;

public class ArbolTests
{
    // ------------------------------------------------------------------
    // 1. Persona: constructor vacío permite asignar propiedades
    // ------------------------------------------------------------------
    [Test]
    public void Persona_ConstructorVacio_PermiteAsignarPropiedades()
    {
        var persona = new Persona();

        persona.Nombre = "Ana";
        persona.Cedula = "123456789";
        persona.Parentesco = "Hija";
        persona.FechaNacimiento = new DateTime(2000, 5, 10);
        persona.Edad = 24;
        persona.Coordenadas = new List<double> { 9.93, -84.08 };
        persona.RutaImagen = "ana.png";

        Assert.AreEqual("Ana", persona.Nombre);
        Assert.AreEqual("123456789", persona.Cedula);
        Assert.AreEqual("Hija", persona.Parentesco);
        Assert.AreEqual(24, persona.Edad);
        Assert.AreEqual(9.93, persona.Coordenadas[0]);
        Assert.AreEqual(-84.08, persona.Coordenadas[1]);
        Assert.AreEqual("ana.png", persona.RutaImagen);
    }

    // ------------------------------------------------------------------
    // 2. Persona: constructor con parámetros asigna todo correctamente
    // ------------------------------------------------------------------
    [Test]
    public void Persona_ConstructorConParametros_AsignaTodosLosCampos()
    {
        var coords = new List<double> { 9.93, -84.08 };

        var persona = new Persona(
            nombre: "Luis",
            edad: 30,
            cedula: "987654321",
            fechaNacimiento: new DateTime(1994, 1, 1),
            coordenadas: coords,
            rutaImagen: "luis",
            parentesco: "Padre"
        );

        Assert.AreEqual("Luis", persona.Nombre);
        Assert.AreEqual(30, persona.Edad);
        Assert.AreEqual("987654321", persona.Cedula);
        Assert.AreEqual(new DateTime(1994, 1, 1), persona.FechaNacimiento);
        CollectionAssert.AreEqual(coords, persona.Coordenadas);
        Assert.AreEqual("luis", persona.RutaImagen);
        Assert.AreEqual("Padre", persona.Parentesco);
    }

    // ------------------------------------------------------------------
    // 3. Persona: ToString incluye nombre y cédula
    // ------------------------------------------------------------------
    [Test]
    public void Persona_ToString_IncluyeNombreYCedula()
    {
        var persona = new Persona
        {
            Nombre = "María",
            Cedula = "111222333",
            Edad = 28,
            Coordenadas = new List<double> { 10.00, -84.00 }
        };

        string texto = persona.ToString();

        StringAssert.Contains("María", texto);
        StringAssert.Contains("111222333", texto);
        StringAssert.Contains("Edad: 28", texto);
    }

    // ------------------------------------------------------------------
    // 4. Persona: Se respetan límites negativos y positivos en coordenadas
    // ------------------------------------------------------------------
    [Test]
    public void Persona_Coordenadas_RespetanLimitesNegativosYPositivos()
    {
        var persona = new Persona
        {
            Coordenadas = new List<double> { 600, 999 }
        };

        Assert.AreEqual(400, persona.Coordenadas[0]);   // max X
        Assert.AreEqual(210, persona.Coordenadas[1]);   // max Y

        persona.Coordenadas = new List<double> { -800, -500 };

        Assert.AreEqual(-400, persona.Coordenadas[0]);  // min X
        Assert.AreEqual(-210, persona.Coordenadas[1]);  // min Y
    }

    // ------------------------------------------------------------------
    // 5. BuscarNDistancias: devuelve solo enlaces que involucran a la persona
    // ------------------------------------------------------------------
    [Test]
    public void BuscarNDistancias_DevuelveSoloEnlacesQueInvolucranANombre()
    {
        var go = new GameObject("GUIMAPA");
        var guiMapa = go.AddComponent<GUIMAPA>();

        var enlaces = new List<(string name1, string name2, double distancia)>
        {
            ("Ana",   "Luis",  10.0),
            ("Maria", "Ana",   20.0),
            ("Pedro", "Luis",  30.0)
        };

        var resultado = guiMapa.BuscarNDistancias("Ana", enlaces);

        Assert.AreEqual(2, resultado.Count, "Debería devolver solo los enlaces donde aparece Ana.");

        foreach (var item in resultado)
        {
            bool involucraAna = item.name1 == "Ana" || item.name2 == "Ana";
            Assert.IsTrue(involucraAna,
                $"El enlace ({item.name1}, {item.name2}) no debería estar en el resultado porque no involucra a Ana.");
        }
    }

    // ------------------------------------------------------------------
    // 6. BuscarNDistancias: devuelve lista vacía si el nombre no aparece
    // ------------------------------------------------------------------
    [Test]
    public void BuscarNDistancias_NombreNoPresente_DevuelveListaVacia()
    {
        var go = new GameObject("GUIMAPA");
        var guiMapa = go.AddComponent<GUIMAPA>();

        var enlaces = new List<(string name1, string name2, double distancia)>
        {
            ("Ana",   "Luis",  10.0),
            ("Maria", "Ana",   20.0),
            ("Pedro", "Luis",  30.0)
        };

        var resultado = guiMapa.BuscarNDistancias("Juan", enlaces);

        Assert.AreEqual(0, resultado.Count,
            "Si el nombre no aparece en ningún enlace, el resultado debería ser una lista vacía.");
    }

    // ------------------------------------------------------------------
    // 7. OnNodoClicked: segundo click elimina líneas y entrada del diccionario
    // ------------------------------------------------------------------
    [Test]
    public void OnNodoClicked_SegundoClick_EliminaLineasYEntradaDelDiccionario()
    {
        // Estado limpio
        GUIMAPA.lineasPorNodo.Clear();

        var go = new GameObject("GUIMAPA");
        var guiMapa = go.AddComponent<GUIMAPA>();

        var persona = new Persona { Nombre = "Ana" };

        // Act: primer click
        guiMapa.OnNodoClicked(persona);
        // Act: segundo click
        guiMapa.OnNodoClicked(persona);

        Assert.IsFalse(GUIMAPA.lineasPorNodo.ContainsKey(persona.Nombre),
            "Después del segundo click, las líneas asociadas al nodo deberían eliminarse del diccionario.");
    }

    // ------------------------------------------------------------------
    // Helpers para pruebas de CreateLine
    // ------------------------------------------------------------------
    private GUIMAPA CrearGUIMAPAConLinePrefabYParent(out RectTransform parentRT)
    {
        var parentGo = new GameObject("ParentUI", typeof(RectTransform));
        parentRT = parentGo.GetComponent<RectTransform>();

        var linePrefabGo = new GameObject("LinePrefab", typeof(RectTransform), typeof(UnityEngine.UI.Image));
        var lineImage = linePrefabGo.GetComponent<UnityEngine.UI.Image>();

        var go = new GameObject("GUIMAPA");
        var guiMapa = go.AddComponent<GUIMAPA>();
        guiMapa.parentUI = parentRT;
        guiMapa.linePrefab = lineImage;

        return guiMapa;
    }

    // ------------------------------------------------------------------
    // 8. CreateLine: el objeto creado es hijo de parentUI
    // ------------------------------------------------------------------
    [Test]
    public void CreateLine_ObjetoCreadoEsHijoDeParentUI()
    {
        var guiMapa = CrearGUIMAPAConLinePrefabYParent(out RectTransform parentRT);

        var aGo = new GameObject("A", typeof(RectTransform));
        var bGo = new GameObject("B", typeof(RectTransform));
        var a = aGo.GetComponent<RectTransform>();
        var b = bGo.GetComponent<RectTransform>();

        a.anchoredPosition = new Vector2(0f, 0f);
        b.anchoredPosition = new Vector2(10f, 0f);

        GameObject linea = guiMapa.CreateLine(a, b, 10.0);

        Assert.AreEqual(parentRT, linea.transform.parent,
            "La línea creada debe ser hija de parentUI.");
    }

    // ------------------------------------------------------------------
    // 9. CreateLine: la posición es el punto medio entre los dos nodos
    // ------------------------------------------------------------------
    [Test]
    public void CreateLine_PosicionEsPuntoMedioEntreNodos()
    {
        var guiMapa = CrearGUIMAPAConLinePrefabYParent(out RectTransform parentRT);

        var aGo = new GameObject("A", typeof(RectTransform));
        var bGo = new GameObject("B", typeof(RectTransform));
        var a = aGo.GetComponent<RectTransform>();
        var b = bGo.GetComponent<RectTransform>();

        a.anchoredPosition = new Vector2(0f, 0f);
        b.anchoredPosition = new Vector2(10f, 0f);

        GameObject linea = guiMapa.CreateLine(a, b, 10.0f);
        var rtLinea = linea.GetComponent<RectTransform>();

        Vector2 expectedCenter = (a.anchoredPosition + b.anchoredPosition) * 0.5f;

        Assert.That(rtLinea.anchoredPosition.x, Is.EqualTo(expectedCenter.x).Within(0.001f));
        Assert.That(rtLinea.anchoredPosition.y, Is.EqualTo(expectedCenter.y).Within(0.001f));
    }

    // ------------------------------------------------------------------
    // 10. CreateLine: el largo de la línea coincide con la distancia entre nodos
    // ------------------------------------------------------------------
    [Test]
    public void CreateLine_LargoCoincideConDistanciaEntreNodos()
    {
        var guiMapa = CrearGUIMAPAConLinePrefabYParent(out RectTransform parentRT);

        var aGo = new GameObject("A", typeof(RectTransform));
        var bGo = new GameObject("B", typeof(RectTransform));
        var a = aGo.GetComponent<RectTransform>();
        var b = bGo.GetComponent<RectTransform>();

        a.anchoredPosition = new Vector2(3f, 4f);
        b.anchoredPosition = new Vector2(0f, 0f);

        float expectedLength = Vector2.Distance(a.anchoredPosition, b.anchoredPosition);

        GameObject linea = guiMapa.CreateLine(a, b, expectedLength);
        var rtLinea = linea.GetComponent<RectTransform>();

        Assert.That(rtLinea.sizeDelta.x, Is.EqualTo(expectedLength).Within(0.001f),
            "El ancho (sizeDelta.x) de la línea debe coincidir con la distancia entre los nodos.");
    }
}
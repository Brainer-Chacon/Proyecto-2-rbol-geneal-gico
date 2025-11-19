/*
    Representa un nodo dentro del árbol genealógico.

    Contiene la referencia a una persona y sus relaciones
        directas: padre, madre e hijos.

    Permite agregar y eliminar hijos de manera dinámica,
        manteniendo la estructura del árbol.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArbolGenealogico.Domain
{
    public class NodoGenealogico : MonoBehaviour    // Nodo que conecta a una persona con sus relaciones familiares.
    {
        public Persona Persona { get; set; } // Persona asociada al nodo.
        public NodoGenealogico Padre { get; set; } // Referencia al padre.
        public NodoGenealogico Madre { get; set; } // Referencia a la madre.
        public List<NodoGenealogico> Hijos { get; set; } = new List<NodoGenealogico>(); // Lista de hijos.


        /// Constructor para inicializar un nodo con una persona.
        public NodoGenealogico(Persona persona, NodoGenealogico padre, NodoGenealogico madre)
        {
            Persona = persona;
            Padre = padre;
            Madre = madre;
        }


        /// Agrega un hijo a la lista de hijos.
        public void AgregarHijo(NodoGenealogico hijo)
        {
            Hijos.Add(hijo); // Inserta el hijo en la lista.
        }

        /// Elimina un hijo de la lista de hijos.
        public void EliminarHijo(NodoGenealogico hijo)
        {
            Hijos.Remove(hijo); // Remueve el hijo de la lista.
        }
    }
}

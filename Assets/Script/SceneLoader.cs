using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour // Clase para cargar escenas en Unity.
{
    [SerializeField] private string sceneToLoad;

    public void LoadScene()
    {
        Debug.Log("LoadScene() called. Trying to load: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}


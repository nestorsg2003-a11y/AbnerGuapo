using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //para cambiar de escena


public class SceneChange : MonoBehaviour
{
    public void CargarEscena(string nombreEscena)
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(nombreEscena);
    }
    public void CerrarJuego()
    {
        Time.timeScale = 1f;

        Application.Quit();
    }
}

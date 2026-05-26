using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelControlUI : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
        MinibossControl.OnMinibossDead += ShowPanelControlUI;
    }
    void ShowPanelControlUI()
    {
        gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        MinibossControl.OnMinibossDead -= ShowPanelControlUI;
    
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

        Application.Quit();
    }
}

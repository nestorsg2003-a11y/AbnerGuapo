using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;

    private void Start()
    {
        victoryPanel.SetActive(false);

        MinibossControl.OnMinibossDead += ShowVictoryUI;
    }

    private void OnDestroy()
    {
        MinibossControl.OnMinibossDead -= ShowVictoryUI;
    }

    private void ShowVictoryUI()
    {
        Debug.Log("MOSTRAR UI");

        victoryPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}
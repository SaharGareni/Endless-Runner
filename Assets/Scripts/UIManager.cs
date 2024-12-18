using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI score;
    private bool isGameOver;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }

    void Update()
    {
        if (isGameOver)
        {
            if (CheckForInput())
            {
                SceneManager.LoadScene(0);
                Time.timeScale = 1.0f;
            }
        }
    }
    private void HandlePlayerDeath()
    {
        score.text = Mathf.RoundToInt(100 * Time.timeSinceLevelLoad * GameManager.GetDifficultyPercentage()).ToString();
        gameOverScreen.SetActive(true);
        isGameOver = true;

    }
    private bool CheckForInput()
    {
        return Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.S)
            || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.DownArrow);
    }
}

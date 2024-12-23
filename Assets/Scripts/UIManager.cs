using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;
    private bool isGameOver;
    private bool isTitleScreenActive;
    public static UIManager Instance { get; private set; } 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        isTitleScreenActive = true;
        titleScreen.SetActive(true);
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
        GameManager.OnInputDetected += HandleInputDetected;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
        GameManager.OnInputDetected -= HandleInputDetected;
    }

    void Update()
    {
        score.text = Mathf.RoundToInt(100 * Time.timeSinceLevelLoad * GameManager.GetDifficultyPercentage()).ToString();
    }
    private void HandlePlayerDeath()
    {
        
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        gameOverScreen.SetActive(true);
        isGameOver = true;

    }
    private void HandleInputDetected()
    {
        if (isGameOver)
        {
            isGameOver = false;
            gameOverScreen.SetActive(false);
        } 
        if (isTitleScreenActive)
        {
            isTitleScreenActive = false;
            if (titleScreen.TryGetComponent<UiMovement>(out var uiMovement))
            {
                uiMovement.enabled = true;
            }
        }
    }
}

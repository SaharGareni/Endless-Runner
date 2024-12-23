using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject titleScreen;
    //TODO: assign the new score screen and replace the current serilized field with a game object one to hold the correct panel
    [SerializeField] private GameObject scorePanel;
    //TODO:after implemting a Score property in game manager, remove the below refrence
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
        //TODO: change the math behind the score calc and access it from game manager, ui manager should not be concerned with this logic
        //FIX: the below logic now uses Time.timeSinceLevelLoad as a reference to the score which technically works fine except for the first iteration of the game (title screen iteration)
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
        //TODO: add logic here to display the score counter after the title screen is gone
       //* Dont forget to change the jumpy, shaky text scripts that you use in the text effect to use Time.unscaledDeltaTime
       // Should only be relevant 
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
            scorePanel.SetActive(true);
        }
    }
}

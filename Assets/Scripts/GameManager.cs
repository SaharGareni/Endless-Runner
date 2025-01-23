using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //TODO: optimize and clean up the useless code in here and the rest of the scripts
    private ObstacleSpawner obstacleSpawner;
    private bool gameStarted;
    private bool gameOver;
    private float scoreMultiplier = 20f;
    private float scoreFloat;
    private static float timeToMaxDifficulty = 90f;
    private static float timeSinceGameStart;
    private int highScore;
    //For debugging just to see the difficulty in the inspector this
    public float difficultyPercent;
    public static GameManager Instance;
    public static event System.Action OnInputDetected;
    public int Score {  get; private set; }
    //TODO: create a score property and calculate it from here with a private setter and public getter for ui mannger
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    void Update()
    {
        //Just for debugging there is no need for this
        difficultyPercent = Mathf.Clamp01(timeSinceGameStart / timeToMaxDifficulty);
        if (gameStarted)
        {
            timeSinceGameStart += Time.deltaTime;
            if (gameOver && CheckForInput())
            {
                OnInputDetected?.Invoke();
                Score = 0;
                scoreFloat = 0f;
                SceneManager.LoadScene("MainGame");
                Resume();
            }
            if (!gameOver)
            {
                scoreFloat += scoreMultiplier * Time.deltaTime;
                Score = Mathf.FloorToInt(scoreFloat);
            }
        }
        else
        {
            if (CheckForInput())
            {
                gameStarted = true;
                if (obstacleSpawner != null)
                {
                  obstacleSpawner.gameObject.SetActive(true);
                }
                OnInputDetected?.Invoke();

            }
        }
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void HandlePlayerDeath()
    {
        gameOver = true;
        //TODO: This is currently refered to as "highScore" but this should change to just "score"
        // then the logic should check if's truely a highscore or not
        //TODO: 
        highScore = Score;
        if (highScore > PlayerPrefs.GetInt("HighScore"))
        {
            //TODO:
            PlayerPrefs.SetInt("HighScore",highScore);
        }
        Pause();
    }
    public void Pause()
    {
        //changed this for 1 for the sake of debugging
        //set to 0 later
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        gameOver = false;
        Time.timeScale = 1f;
        timeSinceGameStart = 0f;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1 || scene.name == "MainGame")
        {
            obstacleSpawner = FindAnyObjectByType<ObstacleSpawner>();
          
            if (obstacleSpawner != null && !gameStarted)
            {
                obstacleSpawner.gameObject.SetActive(false);
                //obstacleSpawner.enabled = false;
            }
        }
    }
    public static float GetDifficultyPercentage()
    {
       return Mathf.Clamp01(timeSinceGameStart/ timeToMaxDifficulty);
    }
    private bool CheckForInput()
    {
        return Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.S)
            || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.LeftAlt);
    }
}

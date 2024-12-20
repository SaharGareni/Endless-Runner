using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private ObstacleSpawner obstacleSpawner;
    private bool gameStarted;
    private bool gameOver;
    private static float timeToMaxDifficulty = 130f;
    private static float timeSinceGameStart;
    //For debugging just to see the difficulty in the inspector this
    public float difficultyPercent;
    public static GameManager Instance;
    public static event System.Action OnInputDetected;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(1);
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
                SceneManager.LoadScene(1);
                Resume();
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
        if (scene.buildIndex == 1)
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

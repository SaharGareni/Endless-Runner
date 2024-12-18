using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isInitialRun = true;
    private static float timeToMaxDifficulty = 130f;
    //For debugging just to see the difficulty in the inspector this
    public float difficultyPercent;
    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        //Just for debugging there is no need for this
        difficultyPercent = Mathf.Clamp01(Time.timeSinceLevelLoad / timeToMaxDifficulty);
        //if (isInitialRun)
        //{
        //    if (CheckForInput())
        //    {
        //        isInitialRun = false;
        //        Resume();
        //    }
        //}
    }
    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }
    private void HandlePlayerDeath()
    {
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
        Time.timeScale = 1f;
    }
    public static float GetDifficultyPercentage()
    {
       return Mathf.Clamp01(Time.timeSinceLevelLoad/ timeToMaxDifficulty);
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

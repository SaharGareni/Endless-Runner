using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static float timeToMaxDifficulty = 130f;
    //For debugging just to see the difficulty in the inspector
    public float difficultyPercent;
    void Start()
    {
        
    }
    void Update()
    {
        difficultyPercent = Mathf.Clamp01(Time.timeSinceLevelLoad / timeToMaxDifficulty);
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
        Time.timeScale = 1f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
    }
    public static float GetDifficultyPercentage()
    {
       return Mathf.Clamp01(Time.timeSinceLevelLoad/ timeToMaxDifficulty);
    }
}

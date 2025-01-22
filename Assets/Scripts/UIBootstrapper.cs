using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBootstrapper : MonoBehaviour
{
    [SerializeField] private Canvas uiCanvas; 

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the loaded scene is MainGame (by name or buildIndex)
        if (scene.name == "MainGame" || scene.buildIndex == 1)
        {
            // Find the Main Camera in the MainGame scene
            // (Assumes it's tagged "MainCamera")
            GameObject camObj = GameObject.FindWithTag("MainCamera");
            if (camObj != null)
            {
                Camera gameCam = camObj.GetComponent<Camera>();

                // Switch Canvas to Screen Space - Camera
                uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;

                // Assign the newly loaded camera
                uiCanvas.worldCamera = gameCam;
            }
            else
            {
                Debug.LogWarning("MainCamera not found in MainGame scene!");
            }
        }
    }
}

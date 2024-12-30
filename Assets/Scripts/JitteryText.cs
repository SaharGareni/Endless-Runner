using TMPro;
using UnityEngine;

public class JitteryText : MonoBehaviour
{
    [Header("Shake Settings")]
    [Tooltip("How far each character can move from its original position.")]
    [SerializeField] private float shakeAmplitude = 5f;

    [Tooltip("How quickly characters jitter around.")]
    [SerializeField] private float shakeFrequency = 25f;

    // Reference to your TextMeshPro component
    private TMP_Text textMeshPro;

    // We’ll store random seeds for each character so that each has its own unique jitter.
    private Vector2[] perlinSeeds;

    // Cached info about the text geometry
    private TMP_TextInfo textInfo;

    private void Awake()
    {
        textMeshPro = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        //TODO: Add logic here that takes the number string and converts back to int
        //Then set the amplitude based on number

        // Force an initial update so we can get accurate character count and positions
        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;

        // Assign a random seed to each character to get distinct Perlin noise values
        perlinSeeds = new Vector2[textInfo.characterCount];
        for (int i = 0; i < perlinSeeds.Length; i++)
        {
            perlinSeeds[i] = new Vector2(Random.Range(0f, 1000f), Random.Range(0f, 1000f));
        }
    }

    private void Update()
    {
        // If your text changes every frame, you might need to re-fetch
        // textInfo = textMeshPro.textInfo; after ForceMeshUpdate().
        // Otherwise, if the text is static, you can skip re-calling ForceMeshUpdate().

        // ForceMeshUpdate() can be expensive if called every frame, so consider calling it
        // only when the text actually changes. For demonstration, we’ll do it here:
        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;

        // Iterate through each character
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            // Skip invisible (e.g. space) characters
            if (!textInfo.characterInfo[i].isVisible) continue;

            // Find where in the vertex buffer the current character's data is stored
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

            // Get the vertices array for this material
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // Use Time.unscaledTime so the effect isn’t scaled by Time.timeScale
            float time = Time.unscaledTime * shakeFrequency;

            // Generate small offsets via Perlin Noise
            float xOffset = (Mathf.PerlinNoise(perlinSeeds[i].x, time) - 0.5f) * 2f * shakeAmplitude;
            float yOffset = (Mathf.PerlinNoise(perlinSeeds[i].y, time) - 0.5f) * 2f * shakeAmplitude;

            // Apply the same offset to all 4 vertices of this character
            vertices[vertexIndex + 0] += new Vector3(xOffset, yOffset, 0f);
            vertices[vertexIndex + 1] += new Vector3(xOffset, yOffset, 0f);
            vertices[vertexIndex + 2] += new Vector3(xOffset, yOffset, 0f);
            vertices[vertexIndex + 3] += new Vector3(xOffset, yOffset, 0f);
        }

        // Push the updated vertex positions back to the mesh
        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {
            var meshInfo = textInfo.meshInfo[m];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textMeshPro.UpdateGeometry(meshInfo.mesh, m);
        }
    }
}


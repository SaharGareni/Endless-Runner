using TMPro;
using UnityEngine;

public class WavyText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float waveSpeed = 2f;      // Speed of the wave
    public float waveHeight = 5f;    // Height of the wave
    private TMP_Text tmpText;
    private Mesh mesh;
    private Vector3[] vertices;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        tmpText.ForceMeshUpdate();
        mesh = tmpText.mesh;
        vertices = mesh.vertices;

        for (int i = 0; i < tmpText.textInfo.characterCount; i++)
        {
            var charInfo = tmpText.textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            int vertexIndex = charInfo.vertexIndex;

            for (int j = 0; j < 4; j++)
            {
                Vector3 original = vertices[vertexIndex + j];
                vertices[vertexIndex + j] = original + new Vector3(0, Mathf.Sin(Time.time * waveSpeed + original.x * 0.1f) * waveHeight, 0);
            }
        }

        mesh.vertices = vertices;
        tmpText.canvasRenderer.SetMesh(mesh);
    }
}

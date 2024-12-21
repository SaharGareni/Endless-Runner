using TMPro;
using UnityEngine;

public class JumpingText : MonoBehaviour
{
    public float jumpHeight = 5f;     // Maximum height for the jump
    public float jumpSpeed = 2f;     // Speed of the jumping effect
    public float delayBetweenChars = 0.2f; // Delay between characters' jumps
    public bool isAnimating = true;  // Toggle for animation

    private TMP_Text textMeshPro;
    private TMP_TextInfo textInfo;
    private Vector3[] vertices;

    void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        if (textMeshPro == null)
        {
            Debug.LogError("No TextMeshPro component found!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (!isAnimating) return;

        textMeshPro.ForceMeshUpdate(); // Ensure text mesh data is updated
        textInfo = textMeshPro.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue; // Skip invisible characters

            int vertexIndex = charInfo.vertexIndex;
            vertices = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            // Calculate jumping offset
            float jumpOffset = Mathf.Sin(Time.time * jumpSpeed + i * delayBetweenChars) * jumpHeight;

            // Move the vertices of the character
            Vector3 offset = new Vector3(0, jumpOffset, 0);
            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j] += offset;
            }

            // Update the mesh
            textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        }
    }
}

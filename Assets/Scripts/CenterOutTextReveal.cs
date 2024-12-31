using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Attach this script to a TextMeshPro (UI or 3D) object.
/// It will animate the text once when the object is enabled.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class CenterOutTextReveal : MonoBehaviour
{
    [Header("Reveal Settings")]
    [Tooltip("Time (in seconds) it takes for each character to slide on-screen once its reveal has started.")]
    [SerializeField] private float perCharacterRevealDuration = 0.5f;

    [Tooltip("Delay (in seconds) between characters, based on how far they are from the center.")]
    [SerializeField] private float perStepDelay = 0.1f;

    [Tooltip("How far off-screen (to the left) letters start. Increase for a bigger slide distance.")]
    [SerializeField] private float offScreenOffsetX = -1000f;

    // Internal references
    private TMP_Text textMesh;
    private TMP_TextInfo textInfo;

    // We only want to run this once per Enable, so track if it’s completed
    private bool hasAnimated = false;

    private void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        // Only run once
        if (!hasAnimated)
        {
            hasAnimated = true;
            // Start the reveal
            StartCoroutine(RevealFromCenter());
        }
    }

    private IEnumerator RevealFromCenter()
    {
        // Force an update so the text info is up-to-date
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        int charCount = textInfo.characterCount;
        if (charCount == 0) yield break;

        // -------- 1) Identify center index  --------
        // For an odd character count, center is straightforward (e.g., 5 letters => index 2)
        // For even, you can pick either the left-center or right-center.
        int centerIndex = charCount / 2; // e.g., if charCount=5 => centerIndex=2; if 4 => centerIndex=2

        // -------- 2) Build reveal order: center -> right -> left -> right -> left... --------
        // Example: for a 5-letter word: [2, 3, 1, 4, 0]
        // Example: for a 4-letter word: [2, 3, 1, 0] (if centerIndex=2)
        List<int> revealOrder = new List<int>();
        revealOrder.Add(centerIndex);
        int step = 1;
        bool goRight = true;

        while (revealOrder.Count < charCount)
        {
            int nextIndex = goRight ? centerIndex + step : centerIndex - step;
            if (nextIndex >= 0 && nextIndex < charCount)
                revealOrder.Add(nextIndex);

            // Flip between right and left
            if (!goRight)
                step++;
            goRight = !goRight;
        }

        // -------- 3) Store final positions and set initial off-screen positions --------
        // We'll store each character’s final position, and also store a working array of positions
        // that we’ll manipulate each frame (the 'animatedPositions').
        Vector3[][] finalPositions = new Vector3[textInfo.meshInfo.Length][];
        Vector3[][] animatedPositions = new Vector3[textInfo.meshInfo.Length][];

        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {
            int vertCount = textInfo.meshInfo[m].vertices.Length;
            finalPositions[m] = new Vector3[vertCount];
            animatedPositions[m] = new Vector3[vertCount];

            // Copy the current (final) positions
            for (int v = 0; v < vertCount; v++)
            {
                finalPositions[m][v] = textInfo.meshInfo[m].vertices[v];
                // Initialize all animated positions to be far off-screen to the left
                animatedPositions[m][v] = textInfo.meshInfo[m].vertices[v] + new Vector3(offScreenOffsetX, 0f, 0f);
            }
        }

        // Push these off-screen positions into the mesh to hide everything initially
        UpdateMesh(animatedPositions);

        // -------- 4) Assign each character a "start time" to animate based on revealOrder --------
        // The center character will have startTime = 0, the next in the order is offset by 0.1s, etc.
        float[] startTimes = new float[charCount];
        for (int i = 0; i < revealOrder.Count; i++)
        {
            int charIndex = revealOrder[i];
            // The step for this character is basically i
            startTimes[charIndex] = i * perStepDelay;
        }

        // We can figure out how long the entire sequence lasts:
        float totalDuration = startTimes[revealOrder[revealOrder.Count - 1]] + perCharacterRevealDuration;

        // -------- 5) Animate over time --------
        float elapsed = 0f;
        while (elapsed < totalDuration)
        {
            elapsed += Time.deltaTime;

            // For each character, figure out how "far" into its reveal timeline we are
            for (int i = 0; i < charCount; i++)
            {
                // Skip invisible characters (spaces, etc.) 
                // to avoid index errors if isVisible == false
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                // Time at which we started to reveal this character
                float startTime = startTimes[i];
                // Normalized time from 0 -> 1 for this character’s movement
                float t = Mathf.InverseLerp(startTime, startTime + perCharacterRevealDuration, elapsed);

                // If t < 0 => not started yet (still off-screen)
                // If t > 1 => fully arrived
                t = Mathf.Clamp01(t);

                // Lerp each of the 4 vertices of this character from off-screen to final position
                for (int v = 0; v < 4; v++)
                {
                    Vector3 offScreenPos = finalPositions[materialIndex][vertexIndex + v]
                                           + new Vector3(offScreenOffsetX, 0f, 0f);
                    Vector3 finalPos = finalPositions[materialIndex][vertexIndex + v];
                    animatedPositions[materialIndex][vertexIndex + v] = Vector3.Lerp(offScreenPos, finalPos, t);
                }
            }

            // Update the mesh with new animated positions
            UpdateMesh(animatedPositions);

            yield return null;
        }

        // One last update to ensure everything is fully in place
        UpdateMesh(finalPositions);
    }

    /// <summary>
    /// Utility method to apply a nested-array of positions to the TMP mesh data.
    /// </summary>
    private void UpdateMesh(Vector3[][] allPositions)
    {
        // For each material sub-mesh
        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {
            Vector3[] src = allPositions[m];
            textInfo.meshInfo[m].mesh.vertices = src;
            textMesh.UpdateGeometry(textInfo.meshInfo[m].mesh, m);
        }
    }
}

using UnityEngine;
using System.Collections; // IEnumerator

[ExecuteInEditMode]
public class CameraAnchor : MonoBehaviour
{
    public enum AnchorType
    {
        BottomLeft,
        BottomCenter,
        BottomRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        TopLeft,
        TopCenter,
        TopRight,
    };

    public AnchorType anchorType;
    public Vector3 anchorOffset;

    IEnumerator updateAnchorRoutine;

    void Start()
    {
        // Run only once in Play Mode to anchor the initial position.
        updateAnchorRoutine = UpdateAnchorAsync();
        StartCoroutine(updateAnchorRoutine);
    }

    /// <summary>
    /// Called in the Editor whenever you modify a field in the Inspector.
    /// This re-applies the anchor so you can see the offset in real time.
    /// </summary>
    void OnValidate()
    {
        // Only try to update the anchor if ViewportHandler is present.
        if (ViewportHandler.Instance != null)
        {
            UpdateAnchor();
        }
    }

    IEnumerator UpdateAnchorAsync()
    {
        while (ViewportHandler.Instance == null)
        {
            yield return null;
        }

        UpdateAnchor();
        updateAnchorRoutine = null;
    }

    void UpdateAnchor()
    {
        switch (anchorType)
        {
            case AnchorType.BottomLeft:
                SetAnchor(ViewportHandler.Instance.BottomLeft);
                break;
            case AnchorType.BottomCenter:
                SetAnchor(ViewportHandler.Instance.BottomCenter);
                break;
            case AnchorType.BottomRight:
                SetAnchor(ViewportHandler.Instance.BottomRight);
                break;
            case AnchorType.MiddleLeft:
                SetAnchor(ViewportHandler.Instance.MiddleLeft);
                break;
            case AnchorType.MiddleCenter:
                SetAnchor(ViewportHandler.Instance.MiddleCenter);
                break;
            case AnchorType.MiddleRight:
                SetAnchor(ViewportHandler.Instance.MiddleRight);
                break;
            case AnchorType.TopLeft:
                SetAnchor(ViewportHandler.Instance.TopLeft);
                break;
            case AnchorType.TopCenter:
                SetAnchor(ViewportHandler.Instance.TopCenter);
                break;
            case AnchorType.TopRight:
                SetAnchor(ViewportHandler.Instance.TopRight);
                break;
        }
    }

    void SetAnchor(Vector3 anchor)
    {
        transform.position = anchor + anchorOffset;
    }

    // Remove or comment out any Update() calls that anchor every frame.
}

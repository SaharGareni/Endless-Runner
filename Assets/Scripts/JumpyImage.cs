using UnityEngine;

public class JumpyImage : MonoBehaviour
{
    public float jumpHeight = 30f; 
    public float jumpSpeed = 2f; 

    private RectTransform rectTransform; 

    void Start()
    {
        if (!TryGetComponent<RectTransform>(out rectTransform))
        {
            Debug.LogError("This script must be attached to a UI element with a RectTransform!");
        }
    }

    void Update()
    {
        if (rectTransform != null)
        {
            float offsetY = Mathf.Sin(Time.time * jumpSpeed) * jumpHeight;
            Vector3 originalPosition = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(originalPosition.x, originalPosition.y + offsetY, originalPosition.z);
        }
    }
}
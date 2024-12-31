using UnityEngine;

public class UiMovement : MonoBehaviour
{
    [SerializeField] private float speed = 100f; 
    private float canvasHeight;
    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
            canvasHeight = canvasRectTransform.rect.height;
        }
        else
        {
            Debug.LogError("UiMovement: No Canvas found in parent hierarchy.");
        }
    }

    void Update()
    {
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition += speed * Time.deltaTime * Vector2.up;

            if (rectTransform.anchoredPosition.y > canvasHeight)
            {
                gameObject.SetActive(false);
            }
        }
    }
}


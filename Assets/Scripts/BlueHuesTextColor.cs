using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class BlueHuesTextColor : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [Tooltip("Minimum hue (0..1 range), e.g. ~0.5 for blues, ~0.6 is more teal.")]
    [SerializeField] private float minHue = 0.5f;
    [Tooltip("Maximum hue (0..1 range), e.g. ~0.7 for teal-ish blues.")]
    [SerializeField] private float maxHue = 0.6f;
    [SerializeField] private float saturation = 1.0f;
    [SerializeField] private float value = 1.0f;

    private TMP_Text tmpText;
    private float hue;
    private bool ascending = true;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        // Start the hue in the middle of the range
        hue = (minHue + maxHue) / 2f;
    }

    private void Update()
    {
        float delta = Time.unscaledDeltaTime * speed;

        // Smoothly move hue between minHue and maxHue
        if (ascending)
        {
            hue += delta;
            if (hue > maxHue)
            {
                hue = maxHue;
                ascending = false;
            }
        }
        else
        {
            hue -= delta;
            if (hue < minHue)
            {
                hue = minHue;
                ascending = true;
            }
        }

        // Convert HSV -> RGB
        Color color = Color.HSVToRGB(hue, saturation, value);

        // Apply to the text
        tmpText.color = color;
    }
}

using UnityEngine;

public class BobUI : MonoBehaviour
{
    public float amplitude = 10f;   // How far it moves up and down
    public float frequency = 2f;    // Speed of the bob

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        rectTransform.anchoredPosition = startPos + new Vector2(0, y);
    }
}

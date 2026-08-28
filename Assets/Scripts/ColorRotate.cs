using TMPro;
using UnityEngine;

public class ColorRotate : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Color nextColor;
    private float r, g, b, timer, nextTime;

    void ColorChange()
    {
        r = Random.Range(0f,1.0f);
        g = Random.Range(0f,1.0f);
        b = Random.Range(0f,1.0f);
        nextColor = new Color(r, g, b);
        text.color = nextColor;
    }
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        nextTime = 1f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > nextTime)
        {
            ColorChange();
            timer = 0;
        }
    }
}

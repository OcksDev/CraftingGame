using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScaler : MonoBehaviour
{
    RectTransform balls;
    Vector2 anh;
    // Start is called before the first frame update
    void Start()
    {
        balls = GetComponent<RectTransform>();
        anh = balls.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float b = Mathf.Sin(Time.time) + 1;
        //balls.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 415*b);
        balls.sizeDelta = new Vector2(b * 415, balls.sizeDelta.y);
        balls.anchoredPosition = new Vector2(anh.x-((b*415)-415)/2, anh.y);
    }
}

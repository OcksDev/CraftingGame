using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSDis : MonoBehaviour
{
    public TextMeshProUGUI texty;
    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.1f;

    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.unscaledDeltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
            texty.text = "FPS: " + ((int)m_lastFramerate).ToString();
        }
    }
}

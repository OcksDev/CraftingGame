using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSex : MonoBehaviour
{
    public Transform orignalpos;
    public Transform mover;
    public Image Healthbar;
    public Image HealthbarTrailer;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(PlayerController.Instance != null)
        {
            float x = (float)(PlayerController.Instance.entit.Health / PlayerController.Instance.entit.Max_Health);
            Healthbar.fillAmount = x;
            HealthbarTrailer.fillAmount = Mathf.Lerp(HealthbarTrailer.fillAmount, x - 0.01f, 0.06f);
            Vector3 p = orignalpos.position;
            if (x <= 0.20f)
            {
                float ss = 0.03f;
                if (x <= 0.10f) ss = 0.05f;
                p.x += Random.Range(-ss, ss);
                p.y += Random.Range(-ss, ss);
            }
            mover.position = p;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSex : MonoBehaviour
{
    public Transform orignalpos;
    public Transform mover;
    public Image[] DashBarSex;
    public Image Healthbar;
    public Image HealthbarTrailer;
    public Image Healthbarsex;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(PlayerController.Instance != null)
        {
            var e = PlayerController.Instance;
            float x = (float)(e.entit.Health / e.entit.Max_Health);
            float[] y = new float[3];
            y[0] = (e.DashCoolDown) / (e.MaxDashCooldown);
            y[1] = (e.DashCoolDown-e.MaxDashCooldown) / (e.MaxDashCooldown);
            y[2] = (e.DashCoolDown-(e.MaxDashCooldown*2)) / (e.MaxDashCooldown);
            DashBarSex[0].fillAmount = y[0];
            DashBarSex[1].fillAmount = y[1];
            DashBarSex[2].fillAmount = y[2];
            if (DashBarSex[3].fillAmount > DashBarSex[0].fillAmount)
            {
                DashBarSex[3].fillAmount = DashBarSex[0].fillAmount;
            }
            else
            {
                DashBarSex[3].fillAmount = Mathf.Lerp(DashBarSex[3].fillAmount, y[0] + 0.01f, 0.075f);
            }
            if (DashBarSex[4].fillAmount > DashBarSex[1].fillAmount)
            {
                DashBarSex[4].fillAmount = DashBarSex[1].fillAmount;
            }
            else
            {
                DashBarSex[4].fillAmount = Mathf.Lerp(DashBarSex[4].fillAmount, y[1] + 0.01f, 0.075f);
            }
            if (DashBarSex[5].fillAmount > DashBarSex[2].fillAmount)
            {
                DashBarSex[5].fillAmount = DashBarSex[2].fillAmount;
            }
            else
            {
                DashBarSex[5].fillAmount = Mathf.Lerp(DashBarSex[5].fillAmount, y[2] + 0.01f, 0.075f);
            }
            Healthbar.fillAmount = x;
            Healthbarsex.fillAmount = (float)(PlayerController.Instance.entit.Shield / PlayerController.Instance.entit.Max_Shield);
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

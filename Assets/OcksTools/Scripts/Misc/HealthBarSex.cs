using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSex : MonoBehaviour
{
    public Transform orignalpos;
    public Transform mover;
    public Image DashBarSex1;
    public Image DashBarSex2;
    public Image DashBarSex3;
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
            DashBarSex1.fillAmount = (e.DashCoolDown) / (e.MaxDashCooldown);
            DashBarSex2.fillAmount = (e.DashCoolDown-e.MaxDashCooldown) / (e.MaxDashCooldown);
            DashBarSex3.fillAmount = (e.DashCoolDown-(e.MaxDashCooldown*2)) / (e.MaxDashCooldown);
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

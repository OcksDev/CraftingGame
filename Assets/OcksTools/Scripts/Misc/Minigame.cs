using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    public Transform Rotato;
    public Image Area;
    float mult = 1f;
    int rotm = 1;
    float rpos = -1;
    float fillperc = 0.15f;
    public int Score = 0;
    public void StartGame()
    {
        mult = 1;
        rotm = 1;
        Score = 0;
        fillperc = 0.15f;
        RandomPos();
    }
    void Update()
    {
        Rotato.Rotate(0, 0, (180*mult*rotm) * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ee = Rotato.transform.rotation * Quaternion.Euler(0, 0, -rpos);
            var ww = ee.eulerAngles;
            Debug.Log(ww);


            float leen = 0.01f; // provides around 1.8 (0.5% of circle) degrees of lenience before and after the actual area 

            if ((1-(ww.z/360))+(leen/2) <= fillperc+leen)
            {
                SuccHit();
            }
            else
            {
                Failhit();
            }
        }
    }
    public void SuccHit()
    {
        if(fillperc >= 0.1) fillperc *= 0.9f;
        if(mult < 2.9)mult += 0.20f;
        rotm *= -1;
        Score++;
        RandomPos();
    }
    public void Failhit()
    {
        if (Gamer.Instance.CompletedMinigame) return;
        Gamer.Instance.CompletedMinigame = true;
        Gamer.Instance.MinigameScore = Score;
        mult = 0f;
    }
    public void RandomPos()
    {
        rpos = rotm*Random.Range(90f, 240f);
        var xx = Area.transform.rotation.eulerAngles;
        rpos += xx.z;
        Area.transform.rotation = Quaternion.Euler(0, 0, rpos);
        Area.fillAmount = fillperc;
    }

}

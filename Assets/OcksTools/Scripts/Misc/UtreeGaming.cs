using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtreeGaming : MonoBehaviour
{
    public RectTransform TheGamer;
    public RectTransform TheGamerScaler;
    public Vector3 PosTarget;
    // Update is called once per frame

    public static UtreeGaming Instance;

    private void OnEnable()
    {
        Instance = this;
        PosTarget = Vector3.zero;
        TheGamer.transform.localPosition = PosTarget;
        TheGamerScaler.localScale = Vector3.one;
        scalem = 0.6823537f;
    }
    public float mult = 1;
    public float scrolmult = 1;
    public float scalem = 1;
    public float mousescale = 1;

    public Vector3 oldmouseshung = Vector3.zero;
    bool hasset = false;
    void Update()
    {
        Vector3 dir = Vector3.zero;
        if (InputManager.IsKey("move_forward")) dir -= Vector3.up;
        if (InputManager.IsKey("move_back")) dir -= Vector3.down;
        if (InputManager.IsKey("move_right")) dir -= Vector3.right;
        if (InputManager.IsKey("move_left")) dir -= Vector3.left;
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) // forward
        {
            scalem = Mathf.Clamp(scalem+(Input.GetAxis("Mouse ScrollWheel")*scrolmult * (scalem)), 0.15f, 3.5f);
        }

        if (InputManager.IsKey(KeyCode.Mouse1))
        {
            var c = Camera.main.ScreenToWorldPoint(Input.mousePosition) * mousescale;
            c.z = 0;
            if (hasset)
            {
                var diff = (c - oldmouseshung)*(1/scalem);
                PosTarget += diff;
                TheGamer.transform.localPosition += diff;   
            }
            oldmouseshung = c;
            hasset = true;
        }
        else
        {
            hasset = false;
        }

        PosTarget += dir * Time.deltaTime * mult * Mathf.Lerp(1, 1/scalem, 0.35f);
        TheGamer.transform.localPosition = Vector3.Lerp(TheGamer.transform.localPosition, PosTarget, 10 * Time.deltaTime);
        //TheGamerSM.transform.localPosition = c/scalem;
        TheGamerScaler.localScale = Vector3.one*scalem;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeybindInput : MonoBehaviour
{
    public KeyCode keyCode;
    public SettingInput me;
    [HideInInspector]
    public bool CurrentlySelecting = false;
    public GameObject rere;
    public void ChangeKeybind()
    {
        if (CurrentlySelecting) return;
        if (Gamer.Instance.checks[29]) return;
        reebak = StartCoroutine(WaitForKeybind());
    }
    Coroutine reebak;
    public IEnumerator WaitForKeybind()
    {
        if (CurrentlySelecting) yield break;
        Gamer.Instance.checks[29] = true;
        CurrentlySelecting = true;
        yield return new WaitUntil(() =>{ return InputManager.GetAllCurrentlyPressedKeys().Count > 0; });
        var aa = InputManager.GetAllCurrentlyPressedKeys();
        if (aa[0] != KeyCode.Escape)
        {
            keyCode = aa[0];
        }
        yield return new WaitUntil(() => { return !Input.GetKey(KeyCode.Mouse0); });
        me.WriteValue();
        CurrentlySelecting = false;
        Gamer.Instance.checks[29] = false;
    }

    public void ResetA()
    {
        if (reebak != null) StopCoroutine(reebak);
        keyCode = InputManager.defaultgamekeys[me.Type2][0];
        me.WriteValue();
        CurrentlySelecting = false;
        Gamer.Instance.checks[29] = false;
    }
    public void upup()
    {
        rere.GetComponent<Button>().interactable = keyCode != InputManager.defaultgamekeys[me.Type2][0];
    }


}

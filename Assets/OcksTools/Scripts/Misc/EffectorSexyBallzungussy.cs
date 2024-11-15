using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectorSexyBallzungussy : MonoBehaviour
{
    public Image rem;
    public TextMeshProUGUI Suckytoes;
    public EffectProfile sussyl;
    public void UpdateRender(EffectProfile nerd)
    {
        sussyl = nerd;
        rem.sprite = GISLol.Instance.EffectsDict[nerd.Type].Image;
        Suckytoes.text = nerd.Stack!=1?$"{nerd.Stack}":"";
    }
}

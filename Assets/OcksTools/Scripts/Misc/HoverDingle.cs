using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverDingle : MonoBehaviour
{
    public string type = "";
    public GISDisplay guny;
    public VaultitemDisplay vuny;
    public ItemHolder iuny;
    public SkillCum wunty;
    public EffectorSexyBallzungussy ounty;
    public UpgradeTreeSex zunty;
    private void Start()
    {
        switch (type)
        {
            case "Specifc":
                guny.Type = "Specific";
                break;
        }
    }

    void Update()
    {
        var g = GISLol.Instance;
        if (Gamer.Instance.checks[21]) return;
        var wan = g.IsHoveringReal(gameObject);
        if (wan)
        {
            var hv = new HoverType(type);
            hv.gamgam = gameObject;
            switch (type)
            {
                case "WeaponName":
                    hv.type = "TitleAndDesc";
                    hv.data = GISLol.Instance.ItemsDict[iuny.nerdl.ItemIndex].GetDisplayName();
                    hv.data2 = GISLol.Instance.ItemsDict[iuny.nerdl.ItemIndex].Description;
                    g.HoverDohicky(hv);
                    break;
                case "weenor":
                    g.HoverDohicky(new HoverType(vuny.item));
                    break;
                case "skilly":
                    var b = new GISItem(PlayerController.Instance.Skills[wunty.SkillIndex].Name);
                    g.HoverDohicky(new HoverType(b));
                    break;
                case "logbook":
                    if (Gamer.Instance.checks[13]) return;
                    if (g.LogbookDiscoveries.ContainsKey(guny.item.ItemIndex))
                    {
                        g.HoverDohicky(new HoverType(guny.item));
                    }
                    else
                    {
                        hv.type = "TitleAndDesc";
                        hv.data = "???";
                        hv.data2 = "Discover this in a run to unlock";
                        g.HoverDohicky(hv);
                    }
                    break;
                case "coollol":
                    if(guny.item != null && guny.item.ItemIndex != "Empty") g.HoverDohicky(new HoverType(guny.item));
                    break;
                case "Specifc":
                    if (guny.item == null || guny.item.ItemIndex == "Empty")
                    {
                        hv.type = "Title";
                        switch (guny.GetSwitch())
                        {
                            case 0:
                                hv.data = "Requires a Weapon";
                                break;
                            case 1:
                                hv.data = "Requires a Material";
                                break;
                            case 2:
                                hv.data = "Requires an Aspect";
                                break;
                            case 3:
                                hv.data = "Output";
                                break;
                        }
                        g.HoverDohicky(hv);
                    }
                    break;
                case "Effect":
                    hv.type = "Title";
                    hv.data = ounty.sussyl.Type;
                    g.HoverDohicky(hv);
                    break;
                default:
                    hv.type = "TitleAndDesc";
                    hv.data = "Craft";
                    hv.data2 = "A weapon requires<br>three materials and a name";
                    g.HoverDohicky(hv);
                    break;
                case "Graft":
                    hv.type = "TitleAndDesc";
                    hv.data = "Graft";
                    hv.data2 = "Requires three of the same material and a central weapon";
                    g.HoverDohicky(hv);
                    break;
                case "Aspect":
                    hv.type = "TitleAndDesc";
                    hv.data = "Aspect";
                    hv.data2 = "Requires an aspect and a central weapon";
                    g.HoverDohicky(hv);
                    break;
                case "Transmute":
                    hv.type = "TitleAndDesc";
                    hv.data = "Transmute";
                    hv.data2 = "Sacrifice any two materials to transform one into any other";
                    g.HoverDohicky(hv);
                    break;
                case "Repair":
                    hv.type = "TitleAndDesc";
                    hv.data = "Repair";
                    hv.data2 = "Sacrifice any two materials to repair a weapon";
                    g.HoverDohicky(hv);
                    break;
                case "Upgrte":
                    hv.type = "TitleAndDesc";
                    hv.data = zunty.Title;
                    hv.data2 = zunty.DescReal;
                    g.HoverDohicky(hv);
                    break;
            }
        }
    }
}

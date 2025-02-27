using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GISSlot : MonoBehaviour
{
    public string Name = "";
    public bool CanInteract = true;
    public GISItem Held_Item;
    public GISDisplay Displayer;
    public GISContainer Conte;
    public string InteractFilter ="";
    private float DoubleClickTimer = -69f;
    private RectTransform balls;

    public GISSlot(GISContainer cum)
    {
        Conte= cum;
    }

    public void Awake()
    {
        if(Held_Item == null)
        {
            Held_Item= new GISItem();
        }
        balls = GetComponent<RectTransform>();
        Displayer.memeparent = this;
        //GISLol.checkforhover += HoverCheckerData;
    }
    private void Start()
    {
        switch (InteractFilter)
        {
            case "RockGive":
                Held_Item = new GISItem("Rock");
                break;
            case "Trash":
                Held_Item = new GISItem("Trash");
                break;
            case "VaultInput":
                Held_Item = new GISItem("Vault Input");
                break;
        }
    }
    public bool FailToClick()
    {
        var pp = GISLol.Instance.Mouse_Held_Item;
        if (pp.ItemIndex == "Empty") return false;
        switch (InteractFilter)
        {
            case "Craftable":
                if (!GISLol.Instance.AllCraftables.Contains(pp.ItemIndex)) return true;
                break;
            case "Aspect":
                if (!GISLol.Instance.AllAspects.Contains(pp.ItemIndex)) return true;
                break;
            case "RockGive":
            case "Empty":
                if (pp.ItemIndex != "Empty") return true;
                break;
            case "GraftWeapon":
                if (pp.ItemIndex == "Empty") return false;
                if (!GISLol.Instance.AllWeaponNames.Contains(pp.ItemIndex)) return true;
                if(pp.GraftedMaterial.IsSet()) return true; 
                break;
            case "AspectWeapon":
                if (pp.ItemIndex == "Empty") return false;
                if (!GISLol.Instance.AllWeaponNames.Contains(pp.ItemIndex)) return true;
                if(pp.AspectMaterial.IsSet()) return true; 
                break;
            case "VaultInput":
            case "Trash":
                if (pp.ItemIndex == "Empty") return true;
                break;
            case "Weapon":
                if(!GISLol.Instance.AllWeaponNames.Contains(pp.ItemIndex)) return true;
                break;
        }
        return false;
    }

    public bool IsHoveringReal()
    {
        return GISLol.Instance.IsHoveringReal(gameObject);
    }
    public void OnInteract()
    {
        if(Conte.Name == "Equips")
        {
            if (PlayerController.Instance != null)
            PlayerController.Instance.SetData();
        }

        if(Conte.Name == "LeftNut" || Conte.Name == "RightNut")
        {
            List<int> mimics = new List<int>();
            List<GISItem> Valids = new List<GISItem>();
            for(int i = 0; i < Conte.slots.Count; i++)
            {
                if (Conte.slots[i].Held_Item.ItemIndex== "Rune Of Mimicry") mimics.Add(i);
            }
            if(mimics.Count > 0)
            {
                for (int i = 0; i < Conte.slots.Count; i++)
                {
                    if (!Conte.slots[i].Held_Item.CanMimic()) continue;
                    Valids.Add(Conte.slots[i].Held_Item);
                }
                if(Valids.Count > 0)
                {
                    foreach(var i in mimics)
                    {
                        var meme = Valids[Random.Range(0, Valids.Count)];
                        var picked = new GISItem(meme);
                        var me = Conte.slots[i].Held_Item;
                        Conte.slots[i].Held_Item = picked;
                        if(me.IAMSPECIL != null)
                        {
                            me.IAMSPECIL.sexyballer = picked;
                            me.IAMSPECIL.FixMe();
                        }
                    }
                }
            }

        }

        if (Gamer.Instance.checks[5])
        {
            Shungite();
            if(Conte.Name == "ItemPickup" && Gamer.Instance.checks[20])
            {
                if(Held_Item.ItemIndex != "Empty")
                {
                    Held_Item = new GISItem(Gamer.Instance.PrinterYoinks);
                }
            }
        }
    }
    public static void Shungite()
    {

        int me = GISLol.Instance.All_Containers["LeftNut"].TotalAmountOfItems();
        int you = GISLol.Instance.All_Containers["RightNut"].TotalAmountOfItems();

        var c = GISLol.Instance.All_Containers["Equips"];
        var c1 = c.slots[0].Held_Item;
        var c2 = c.slots[1].Held_Item;
        if(c1.GraftedMaterial.GetName() != "" && c1.GraftedMaterial.GetName() != "Empty")
        {
            me++;
        }
        if(c2.GraftedMaterial.GetName() != "" && c2.GraftedMaterial.GetName() != "Empty")
        {
            you++;
        }
        var x = GISItem.CalcBalance(me, you);
        var ww = Tags.refs["Balance1"].GetComponent<TextMeshProUGUI>();
        SetFunnyShingle(x, ww);
        x = GISItem.CalcBalance(you, me);
        ww = Tags.refs["Balance2"].GetComponent<TextMeshProUGUI>();
        SetFunnyShingle(x, ww);
    }
    private static void SetFunnyShingle(float x, TextMeshProUGUI ww)
    {
        if (x != 1f)
        {
            if (x > 1f)
            {
                ww.text = $"Unbalanced: Under<br>x{x} Total Damage";
                ww.color = new Color32(0, 212, 30, 255);
            }
            else
            {
                ww.text = $"Unbalanced: Over<br>x{x} Total Damage";
                ww.color = new Color32(231, 0, 0, 255);
            }
        }
        else
        {
            ww.text = "";
        }
    }


    float size = 1f;
    public bool IsHovering()
    {
        var m = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = balls.position;
        size = 1.15f;
        if (pos.x - size < m.x && pos.x + size > m.x && pos.y - size < m.y && pos.y + size > m.y)
        {
            return true;
        }
        return false;
    }

    public void HoverCheckerData()
    {
        if (!gameObject.activeInHierarchy) return;
        if (IsHoveringReal())
        {
            GISLol.Instance.hoverballer = Held_Item;
        }
    }

    private void Update()
    {
        if (!CanInteract) return;
        if (FailToClick()) return;
        if (balls == null) return;
        bool shift = InputManager.IsKey("item_alt");
        bool left = InputManager.IsKeyDown("item_select");
        bool right = InputManager.IsKeyDown("item_half");
        if (!(right || left)) return;
        if (!IsHovering()) return;
        switch (InteractFilter)
        {
            case "RockGive":
                GISLol.Instance.Mouse_Held_Item = new GISItem(Held_Item);
                return;
            case "VaultInput":
                if (GISLol.Instance.Mouse_Held_Item.ItemIndex == "Empty") return;
                var wee = GISLol.Instance.AddVaultItem(new GISItem(GISLol.Instance.Mouse_Held_Item));
                if (wee)
                {
                    foreach(var d in Gamer.Instance.spawnednerds)
                    {
                        d.UpdateDisplay();
                    }
                }
                else
                {
                    Gamer.Instance.LoadVaultPage(Gamer.Instance.currentvault);
                }
                var WEENIS2 = GISLol.Instance.Mouse_Held_Item;
                GISLol.Instance.Mouse_Held_Item = new GISItem();
                WEENIS2.Solidify();
                return;
            case "Trash":
                var WEENIS = GISLol.Instance.Mouse_Held_Item;
                GISLol.Instance.Mouse_Held_Item = new GISItem();
                WEENIS.Solidify();
                return;
        }
        //Debug.Log(balls.position.ToString() + ",    " + m.ToString());
        if (Conte.CanShiftClickItems && shift)
        {
            if (left||right)
            {
                Held_Item.AddConnection(Conte);
                SaveItemContainerData();
                var a = new GISItem(Held_Item);
                Held_Item = new GISItem();
                int i = left?0:Conte.slots.Count - 1;
                bool found = false;
                foreach (var item in Conte.slots)
                {
                    var x = Conte.slots[i].Held_Item;
                    if (x.Compare(a))
                    {
                        int max = GISLol.Instance.ItemsDict[a.ItemIndex].MaxAmount;
                        int t = x.Amount + a.Amount;
                        if(max <= 0)
                        {
                            x.Amount = t;
                            found = true;   
                            break;
                        }
                        else
                        {
                            int z = Mathf.Clamp(t, 0, max);
                            x.Amount = z;
                            a.Amount = t - z;
                            if(a.Amount == 0)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    else if (x.ItemIndex == "Empty")
                    {
                        break;
                    }
                    i += left?1:-1;
                }
                if (!found)
                {
                    Conte.slots[i].Held_Item = a;
                }
                SaveItemContainerData();
                OnInteract();
            }
        }
        else
        {
            if (left)
            {
                Held_Item.AddConnection(Conte);
                SaveItemContainerData();
                if (DoubleClickTimer < 0)
                {
                    DoubleClickTimer = 0.2f;
                    var a = GISLol.Instance.Mouse_Held_Item;

                    if (Held_Item.Compare(a))
                    {
                        var d = Held_Item.ItemIndex;
                        int b = a.Amount;
                        int c = Held_Item.Amount + b;
                        Held_Item.Amount = c;
                        int K = GISLol.Instance.ItemsDict[d].MaxAmount;
                        if (K != 0)
                        {
                            if (c > K)
                            {
                                Held_Item.Amount = K;
                                GISLol.Instance.Mouse_Held_Item.Amount = c - K;
                            }
                            else
                            {
                                GISLol.Instance.Mouse_Held_Item.Amount = 0;
                            }
                        }
                        else
                        {
                            GISLol.Instance.Mouse_Held_Item.Amount = 0;
                        }
                        Held_Item.AddConnection(GISLol.Instance.Mouse_Held_Item.Container);


                        if (GISLol.Instance.Mouse_Held_Item.Amount <= 0)
                        {
                            GISLol.Instance.Mouse_Held_Item = new GISItem();
                        }
                    }
                    else
                    {
                        GISLol.Instance.Mouse_Held_Item = Held_Item;
                        GISLol.Instance.Mouse_Held_Item.AddConnection(Conte);
                        if (GISLol.Instance.Mouse_Held_Item.ItemIndex == "Empty")
                        {
                            GISLol.Instance.Mouse_Held_Item.SetContainer(null);
                        }
                        Held_Item = a;
                        if (Held_Item.Container != Conte)
                        {
                            Held_Item.SetContainer(Conte);
                        }
                    }
                }
                else
                {
                    //double click code
                    DoubleClickTimer = -69f;

                    foreach (var slot in Conte.slots)
                    {
                        if (slot != this && slot.Held_Item.Compare(GISLol.Instance.Mouse_Held_Item))
                        {
                            int x = GISLol.Instance.ItemsDict[GISLol.Instance.Mouse_Held_Item.ItemIndex].MaxAmount;
                            if (x != 0 && GISLol.Instance.Mouse_Held_Item.Amount + slot.Held_Item.Amount > x)
                            {
                                slot.Held_Item.Amount = slot.Held_Item.Amount - (x - GISLol.Instance.Mouse_Held_Item.Amount);
                                GISLol.Instance.Mouse_Held_Item.Amount = x;
                            }
                            else
                            {
                                GISLol.Instance.Mouse_Held_Item.Amount += slot.Held_Item.Amount;
                                slot.Held_Item.Amount = 0;
                            }
                        }
                    }
                    for (int i = 0; i < Conte.slots.Count; i++)
                    {
                        if (Conte.slots[i].Held_Item.Amount <= 0)
                        {
                            Conte.slots[i].Held_Item = new GISItem();
                        }
                    }
                    GISLol.Instance.Mouse_Held_Item.AddConnection(Conte);

                }
                SaveItemContainerData();
                OnInteract();

            }
            if (right)
            {
                Held_Item.AddConnection(Conte);
                SaveItemContainerData();
                var a = GISLol.Instance.Mouse_Held_Item;

                if (Held_Item.ItemIndex == "Empty")
                {
                    if (a.Amount > 0)
                    {
                        Held_Item = new GISItem(a);
                        Held_Item.Amount = 1;
                        Held_Item.SetContainer(Conte);

                        GISLol.Instance.Mouse_Held_Item.Amount--;
                        GISLol.Instance.Mouse_Held_Item.AddConnection(Conte);
                        if (GISLol.Instance.Mouse_Held_Item.Amount <= 0)
                        {
                            GISLol.Instance.Mouse_Held_Item = new GISItem();
                        }
                    }
                }
                else
                {
                    if (a.ItemIndex == "Empty")
                    {
                        float b = (float)Held_Item.Amount / 2;
                        GISLol.Instance.Mouse_Held_Item = new GISItem(Held_Item);
                        GISLol.Instance.Mouse_Held_Item.Amount = Mathf.CeilToInt(b);
                        Held_Item.Amount = Mathf.FloorToInt(b);
                        GISLol.Instance.Mouse_Held_Item.AddConnection(Conte);
                        if (Held_Item.Amount <= 0)
                        {
                            Held_Item = new GISItem();
                        }
                    }
                    else if (Held_Item.Compare(a))
                    {
                        int max = GISLol.Instance.ItemsDict[Held_Item.ItemIndex].MaxAmount;
                        if(max == 0 || Held_Item.Amount < max)
                        {
                            Held_Item.Amount++;
                            Held_Item.AddConnection(GISLol.Instance.Mouse_Held_Item.Container);
                            GISLol.Instance.Mouse_Held_Item.AddConnection(Held_Item.Container);
                            GISLol.Instance.Mouse_Held_Item.Amount--;
                            if (GISLol.Instance.Mouse_Held_Item.Amount <= 0)
                            {
                                GISLol.Instance.Mouse_Held_Item = new GISItem();
                            }
                        }
                    }
                }

                SaveItemContainerData();
                OnInteract();



            }
        }
        
    }

    private void FixedUpdate()
    {
        DoubleClickTimer -= Time.deltaTime;
        Displayer.item = Held_Item;
    }

    private void SaveItemContainerData()
    {
        if (Conte.CanRetainItems)
        {
            bool sexg = Conte.SaveTempContents();
            if (sexg)
            {
                //Held_Item.AddConnection(Held_Item.Container);
                foreach (var c in Held_Item.Interacted_Containers)
                {
                    if (c != null) c.SaveTempContents();
                }
                Held_Item.Interacted_Containers.Clear();
            }

        }
        /*
        foreach(var item in placed_items)
        {
            item.Container.SaveTempContents();
        }
        */
    }
}

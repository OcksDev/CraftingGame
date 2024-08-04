using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static string locklevel = "";
    public static Dictionary<KeyCode, string> keynames = new Dictionary<KeyCode, string>();
    public static Dictionary<string, KeyCode> namekeys = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> gamekeys = new Dictionary<string, KeyCode>();
    public static Dictionary<string, KeyCode> gamedefkeys = new Dictionary<string, KeyCode>();
    public static Dictionary<string, string> gamekeynames = new Dictionary<string, string>();
    // Start is called before the first frame update
    void Start()
    {
        ResetLockLevel();
    }
    // Update is called once per frame
    void Awake()
    {
        AssembleTheCodes();
    }

    public static void AssembleTheCodes()
    {
        keynames.Clear();
        namekeys.Clear();
        gamekeys.Clear();
        gamedefkeys.Clear();
        gamekeynames.Clear();
        keynames.Add(KeyCode.A, "A");
        keynames.Add(KeyCode.B, "B");
        keynames.Add(KeyCode.C, "C");
        keynames.Add(KeyCode.D, "D");
        keynames.Add(KeyCode.E, "E");
        keynames.Add(KeyCode.F, "F");
        keynames.Add(KeyCode.G, "G");
        keynames.Add(KeyCode.H, "H");
        keynames.Add(KeyCode.I, "I");
        keynames.Add(KeyCode.J, "J");
        keynames.Add(KeyCode.K, "K");
        keynames.Add(KeyCode.L, "L");
        keynames.Add(KeyCode.M, "M");
        keynames.Add(KeyCode.N, "N");
        keynames.Add(KeyCode.O, "O");
        keynames.Add(KeyCode.P, "P");
        keynames.Add(KeyCode.Q, "Q");
        keynames.Add(KeyCode.R, "R");
        keynames.Add(KeyCode.S, "S");
        keynames.Add(KeyCode.T, "T");
        keynames.Add(KeyCode.U, "U");
        keynames.Add(KeyCode.V, "V");
        keynames.Add(KeyCode.W, "W");
        keynames.Add(KeyCode.X, "X");
        keynames.Add(KeyCode.Y, "Y");
        keynames.Add(KeyCode.Z, "Z");
        keynames.Add(KeyCode.Alpha0, "0");
        keynames.Add(KeyCode.Alpha1, "1");
        keynames.Add(KeyCode.Alpha2, "2");
        keynames.Add(KeyCode.Alpha3, "3");
        keynames.Add(KeyCode.Alpha4, "4");
        keynames.Add(KeyCode.Alpha5, "5");
        keynames.Add(KeyCode.Alpha6, "6");
        keynames.Add(KeyCode.Alpha7, "7");
        keynames.Add(KeyCode.Alpha8, "8");
        keynames.Add(KeyCode.Alpha9, "9");
        keynames.Add(KeyCode.Tab, "TAB");
        keynames.Add(KeyCode.LeftAlt, "LALT");
        keynames.Add(KeyCode.LeftControl, "LCTR");
        keynames.Add(KeyCode.LeftShift, "LSH");
        keynames.Add(KeyCode.LeftWindows, "LWIN");
        keynames.Add(KeyCode.CapsLock, "CAP");
        keynames.Add(KeyCode.RightAlt, "RALT");
        keynames.Add(KeyCode.RightControl, "RCTR");
        keynames.Add(KeyCode.RightShift, "RSH");
        keynames.Add(KeyCode.RightWindows, "RWIN");
        keynames.Add(KeyCode.Delete, "BACK");
        keynames.Add(KeyCode.Insert, "INS");
        keynames.Add(KeyCode.PageDown, "PGDW");
        keynames.Add(KeyCode.PageUp, "PGUP");
        keynames.Add(KeyCode.Print, "PRT");
        keynames.Add(KeyCode.ScrollLock, "SLCK");
        keynames.Add(KeyCode.End, "END");
        keynames.Add(KeyCode.Home, "HOME");
        keynames.Add(KeyCode.Mouse0, "m1");
        keynames.Add(KeyCode.Mouse1, "m2");
        keynames.Add(KeyCode.Mouse2, "m3");
        keynames.Add(KeyCode.Mouse3, "m4");
        keynames.Add(KeyCode.Mouse4, "m5");
        keynames.Add(KeyCode.Mouse5, "m6");
        keynames.Add(KeyCode.Mouse6, "m7");
        keynames.Add(KeyCode.Return, "ENT");
        keynames.Add(KeyCode.Backslash, "\\");
        keynames.Add(KeyCode.Slash, "/");
        keynames.Add(KeyCode.UpArrow, "UP");
        keynames.Add(KeyCode.DownArrow, "DOWN");
        keynames.Add(KeyCode.LeftArrow, "LEFT");
        keynames.Add(KeyCode.RightArrow, "RIGHT");
        keynames.Add(KeyCode.Space, "SPACE");
        keynames.Add(KeyCode.Escape, "ESC");
        keynames.Add(KeyCode.LeftBracket, "[");
        keynames.Add(KeyCode.RightBracket, "]");
        keynames.Add(KeyCode.Semicolon, ";");
        keynames.Add(KeyCode.Quote, "'");
        keynames.Add(KeyCode.Underscore, "_");
        keynames.Add(KeyCode.Equals, "=");
        keynames.Add(KeyCode.Numlock, "NML");
        keynames.Add(KeyCode.F1, "f1");
        keynames.Add(KeyCode.F2, "f2");
        keynames.Add(KeyCode.F3, "f3");
        keynames.Add(KeyCode.F4, "f4");
        keynames.Add(KeyCode.F5, "f5");
        keynames.Add(KeyCode.F6, "f6");
        keynames.Add(KeyCode.F7, "f7");
        keynames.Add(KeyCode.F8, "f8");
        keynames.Add(KeyCode.F9, "f9");
        keynames.Add(KeyCode.F10, "f10");
        keynames.Add(KeyCode.F11, "f11");
        keynames.Add(KeyCode.F12, "f12");
        keynames.Add(KeyCode.F13, "f13");
        keynames.Add(KeyCode.F14, "f14");
        keynames.Add(KeyCode.F15, "f15");
        keynames.Add(KeyCode.Keypad0, "n0");
        keynames.Add(KeyCode.Keypad1, "n1");
        keynames.Add(KeyCode.Keypad2, "n2");
        keynames.Add(KeyCode.Keypad3, "n3");
        keynames.Add(KeyCode.Keypad4, "n4");
        keynames.Add(KeyCode.Keypad5, "n5");
        keynames.Add(KeyCode.Keypad6, "n6");
        keynames.Add(KeyCode.Keypad7, "n7");
        keynames.Add(KeyCode.Keypad8, "n8");
        keynames.Add(KeyCode.Keypad9, "n9");
        keynames.Add(KeyCode.KeypadDivide, "n/");
        keynames.Add(KeyCode.KeypadEquals, "n=");
        keynames.Add(KeyCode.KeypadMinus, "n-");
        keynames.Add(KeyCode.KeypadMultiply, "n*");
        keynames.Add(KeyCode.KeypadPeriod, "n.");
        keynames.Add(KeyCode.KeypadPlus, "n+");
        keynames.Add(KeyCode.KeypadEnter, "nENT");


        //namekeys and keynames are both dictionaries
        foreach (var a in keynames)
        {
            namekeys.Add(a.Value, a.Key);
        }


        //create custom key allocations
        gamekeys.Add("shoot", KeyCode.Mouse0);
        gamekeynames.Add("shoot", "Use Weapon");
        gamekeys.Add("move_forward", KeyCode.W);
        gamekeynames.Add("move_forward", "Move Forward");
        gamekeys.Add("move_back", KeyCode.S);
        gamekeynames.Add("move_back", "Move Backward");
        gamekeys.Add("move_left", KeyCode.A);
        gamekeynames.Add("move_left", "Move Left");
        gamekeys.Add("move_right", KeyCode.D);
        gamekeynames.Add("move_right", "Move Right");
        gamekeys.Add("jump", KeyCode.Space);
        gamekeynames.Add("jump", "Jump");
        gamekeys.Add("reload", KeyCode.R);
        gamekeynames.Add("reload", "Reload");
        gamekeys.Add("close_menu", KeyCode.Escape);
        gamekeynames.Add("close_menu", "Close Menus");
        gamekeys.Add("tab_menu", KeyCode.Tab);
        gamekeynames.Add("tab_menu", "Leaderboard");
        gamekeys.Add("item_select", KeyCode.Mouse0);
        gamekeynames.Add("item_select", "Item Select");
        gamekeys.Add("item_half", KeyCode.Mouse1);
        gamekeynames.Add("item_half", "Item Half");
        gamekeys.Add("item_pick", KeyCode.Mouse2);
        gamekeynames.Add("item_pick", "Item Pick");
        gamekeys.Add("item_alt", KeyCode.LeftShift);
        gamekeynames.Add("item_alt", "Alt Item Mode");
        gamekeys.Add("console", KeyCode.Slash);
        gamekeynames.Add("console", "Open/Close Console");
        gamekeys.Add("console_up", KeyCode.UpArrow);
        gamekeynames.Add("console_up", "Console Up");
        gamekeys.Add("console_down", KeyCode.DownArrow);
        gamekeynames.Add("console_down", "Console Down");
        gamekeys.Add("console_autofill", KeyCode.Tab);
        gamekeynames.Add("console_autofill", "autofill console command");
        gamekeys.Add("dialog_skip", KeyCode.Space);
        gamekeynames.Add("dialog_skip", "Dialog Skip");
        gamekeys.Add("dialog_skip_mouse", KeyCode.Mouse0);
        gamekeynames.Add("dialog_skip_mouse", "Dialog Skip Mouse Edition");
        gamekeys.Add("inven", KeyCode.E);
        gamekeynames.Add("inven", "Fucks Yo Shit");
        gamekeys.Add("interact", KeyCode.F);
        gamekeynames.Add("interact", "Interacts Wit Yo Shit");
        gamekeys.Add("craft", KeyCode.R);
        gamekeynames.Add("craft", "Fucks Yo Shit again");
        gamekeys.Add("dash", KeyCode.LeftShift);
        gamekeynames.Add("dash", "Fucks Yo Shit again again");

        var lang = LanguageFileSystem.Instance;
        lang.UpdateGameFromFile();
        bool create = false;
        foreach(var k in gamekeys)
        {
            var s = "Input_" + k.Key;
            if (lang.IndexValuePairs.ContainsKey(s))
            {
                gamekeynames[k.Key] = lang.IndexValuePairs[s];
            }
            else
            {
                create = true;
                lang.IndexValuePairs.Add(s, gamekeynames[k.Key]);
            }
        }
        if (create)
        {
            lang.UpdateTextFile();
        }

        //assign default game keys
        foreach (var a in gamekeys)
        {
            gamedefkeys.Add(a.Key, a.Value);
        }
    }

    public void CheckNewBind(string keyname)
    {
        //run this every frame until a new keybind is selected

        if (Input.anyKeyDown)
        {
            bool goodboi = false;
            KeyCode boi = KeyCode.Mouse0;
            foreach(var kb in keynames)
            {
                if (Input.GetKeyDown(kb.Key))
                {
                    boi = kb.Key;
                    goodboi = true;
                    break;
                }
            }
            if (goodboi)
            {
                gamekeys[keyname] = boi;
                //code here will run when you have pressed a valid key
            }
        }
    }

    public void ReseBind(string keyname)
    {
        gamekeys[keyname] = gamedefkeys[keyname];
    }





    public static bool GetSelected(string ide)
    {
        return locklevel == ide || locklevel == "" || ide == "";
    }

    public static void ResetLockLevel()
    {
        locklevel = "";
    }

    public static void SetLockLevel(string e)
    {
        locklevel = e;
    }

    public static bool IsDie(string b2 = "")
    {
        //return true for making the input fail
        bool a = false;

        //add code to change boolean to true if the input is denied

        if (b2 != locklevel && locklevel != "")
        {
            if (b2 != "")
            {
                Debug.Log("Cockblocked!");
                a=true;
            }
            else
            {
                Debug.Log("FakeBlcoked!");
            }
        }

        return a;
    }


    public static bool CheckAvailability(string ide = "")
    {
        if (IsDie(ide)) return false;
        return true;
    }

    public static bool IsKeyDown(KeyCode baller, string ide = "")
    {
        if (IsDie(ide)) return false;
        return Input.GetKeyDown(baller) && GetSelected(ide);
    }
    public static bool IsKey(KeyCode baller, string ide = "")
    {
        if (IsDie(ide)) return false;
        return Input.GetKey(baller) && GetSelected(ide);
    }
    public static bool IsKeyUp(KeyCode baller, string ide = "")
    {
        if (IsDie(ide)) return false;
        return Input.GetKeyUp(baller) && GetSelected(ide);
    }
    public static bool IsMouseDown(int baller, string ide = "")
    {
        if (IsDie(ide))
            return false;
        return Input.GetMouseButtonDown(baller) && GetSelected(ide);
    }
    public static bool IsMouse(int baller, string ide = "")
    {
        if (IsDie(ide))
            return false;
        return Input.GetMouseButton(baller) && GetSelected(ide);
    }
    public static bool IsMouseUp(int baller, string ide = "")
    {
        if (IsDie(ide))
            return false;
        return Input.GetMouseButtonUp(baller) && GetSelected(ide);
    }
    public static bool IsButtonDown(string baller, string ide = "")
    {
        if (IsDie(ide))
            return false;
        return Input.GetButtonDown(baller) && GetSelected(ide);
    }
    public static bool IsButton(string baller, string ide = "")
    {
        if (IsDie(ide))
            return false;
        return Input.GetButton(baller) && GetSelected(ide);
    }
    public static bool IsButtonUp(string baller, string ide = "")
    {
        if (IsDie(ide))
            return false;
        return Input.GetButtonUp(baller) && GetSelected(ide);
    }
}

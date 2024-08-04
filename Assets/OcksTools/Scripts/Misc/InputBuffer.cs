using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    /*
     * how to use:
     *     BufferListen() to run every time you want to check for a new input of a given key
     *     GetBuffer() gets if the button exists in the buffer
     *     RemoveBuffer() removes the button from the buffer if it exists
     */
    public static InputBuffer Instance;

    public Dictionary<string, BufferedInput> buffer = new Dictionary<string, BufferedInput>();
    private void Awake()
    {
        Instance = this;
    }
    public void Update()
    {
        for(int i = 0; i < buffer.Count; i++)
        {
            var p = buffer.ElementAt(i);
            p.Value.Time -= Time.deltaTime;
            if(p.Value.Time <= 0)
            {
                buffer.Remove(p.Key);
                i--;
            }
        }
    }

    public bool GetBuffer(string name)
    {
        return buffer.ContainsKey(name);
    }
    public void RemoveBuffer(string name)
    {
        if (buffer.ContainsKey(name)) buffer.Remove(name);
    }

    public void BufferListen(KeyCode key, string name, string ide, float time, bool isdown = true)
    {
        if(isdown? InputManager.IsKeyDown(key, ide) : InputManager.IsKey(key, ide))
        {
            var b = new BufferedInput();
            b.Name = name;
            b.Key = key;
            b.Time = time;
            if (buffer.ContainsKey(name))
            {
                buffer[name] = b;
            }
            else
            {
                buffer.Add(name, b);
            }
        }
    }

}

public class BufferedInput
{
    //just a data holding class
    public float Time;
    public KeyCode Key;
    public string Name;
}

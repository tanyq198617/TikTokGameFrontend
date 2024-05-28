using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputControl : MonoBehaviour
{
    protected readonly Dictionary<KeyCode, Action> inputListener = new Dictionary<KeyCode, Action>();
    protected readonly List<KeyCode> keycodes = new List<KeyCode>();
    protected Action QuitAction = null;
    protected bool IsLocked = false;
    protected bool hasEscape = false;
    protected bool onEscape = false;

    private void Awake() => DontDestroyOnLoad(this);

    private void OnLocked(bool locked) { this.IsLocked = locked; }


    void Update()
    {
        if (IsLocked)
            return;
        OnListener();
    }

    private void OnListener()
    {
        hasEscape = keycodes.Contains(KeyCode.Escape);
        onEscape = false;
        for (int i = 0; i < keycodes.Count; i++)
        {
            if (Input.GetKeyDown(keycodes[i]))
            {
                onEscape = keycodes[i] == KeyCode.Escape;
                inputListener[keycodes[i]]?.Invoke();
            }
        }

        if (!onEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!hasEscape)
                QuitAction?.Invoke();
        }
    }

    public void AddLister(KeyCode keyCode, Action callback)
    {
        if (!inputListener.ContainsKey(keyCode))
        {
            inputListener.Add(keyCode, null);
            keycodes.Add(keyCode);
        }
        inputListener[keyCode] += callback;
    }
    public void RemoveLister(KeyCode keyCode, Action callback)
    {
        if (inputListener.ContainsKey(keyCode))
        {
            inputListener[keyCode] -= callback;
            if (inputListener[keyCode] == null)
            {
                keycodes.Remove(keyCode);
                inputListener.Remove(keyCode);
            }
        }
    }

    public void AddQuitEvent(Action callback)
    {
        QuitAction = callback;
    }

    public void RemoveQuitEvent()
    {
        QuitAction = null;
    }


    public void Clear()
    {
        inputListener.Clear();
        keycodes.Clear();
    }

    void OnDestroy()
    {
        Clear();
    }

    public static void AddInputLister(KeyCode keyCode, Action callback)
    {
        Game.Scene.GetComponent<InputControl>()?.AddLister(keyCode, callback);
    }

    public static void RemoveInputLister(KeyCode keyCode, Action callback)
    {
        Game.Scene.GetComponent<InputControl>()?.RemoveLister(keyCode, callback);
    }

    public static void AddQuitLister(Action callback)
    {
        Game.Scene.GetComponent<InputControl>()?.AddQuitEvent(callback);
    }

    public static void RemoveQuitLister()
    {
        Game.Scene.GetComponent<InputControl>()?.RemoveQuitEvent();
    }

    public static void Locked(bool locked)
    {
        Game.Scene.GetComponent<InputControl>()?.OnLocked(locked);
    }

    public static void RemoveAll()
    {
        Game.Scene.GetComponent<InputControl>()?.Clear();
        Game.Scene.GetComponent<InputControl>()?.OnLocked(false);
    }
}

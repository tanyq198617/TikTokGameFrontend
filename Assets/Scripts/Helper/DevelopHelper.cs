using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HotUpdateScripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class DevelopHelper : MonoBehaviour
{
    private GameObject _obj;

    private static DevelopHelper _instance;
    public static DevelopHelper Instance => _instance;

    private Dictionary<int, MethodInfo> calls = new Dictionary<int, MethodInfo>();
    private List<KeyCode> keycode = new List<KeyCode>();
    
    private Dictionary<int, MethodInfo> keydown = new Dictionary<int, MethodInfo>();
    private List<KeyCode> keydownCode = new List<KeyCode>();

    #region 注册按钮
    public void Awake()
    {
        _instance = this;
        for (KeyCode i = KeyCode.A; i <= KeyCode.Z; i++)
        {
            Register(i, this.GetType().GetMethod($"OnKeyCode_{i.ToString()}", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
        }
        
        RegisterKeyDown(KeyCode.F1, nameof(OnKeyCode_F1)); 
        RegisterKeyDown(KeyCode.F2, nameof(OnKeyCode_F2)); 
        
    
        RegisterKeyDown(KeyCode.Keypad1, nameof(OnKeyCode_Keypad1)); 
        RegisterKeyDown(KeyCode.Keypad2, nameof(OnKeyCode_Keypad2)); 
        RegisterKeyDown(KeyCode.Keypad3, nameof(OnKeyCode_Keypad3)); 
        RegisterKeyDown(KeyCode.Keypad4, nameof(OnKeyCode_Keypad4)); 
        RegisterKeyDown(KeyCode.Keypad5, nameof(OnKeyCode_Keypad5)); 
        RegisterKeyDown(KeyCode.Keypad6, nameof(OnKeyCode_Keypad6)); 
        RegisterKeyDown(KeyCode.Keypad7, nameof(OnKeyCode_Keypad7)); 
        RegisterKeyDown(KeyCode.End, nameof(OnKeyCode_End));
        RegisterKeyDown(KeyCode.Equals, nameof(OnKeyCode_Equals)); //+
        RegisterKeyDown(KeyCode.Minus, nameof(OnKeyCode_Minus)); //-
        
        Register(KeyCode.LeftArrow, nameof(OnKeyCode_LeftArrow));
        Register(KeyCode.RightArrow, nameof(OnKeyCode_RightArrow));
        Register(KeyCode.UpArrow, nameof(OnKeyCode_UpArrow));
        Register(KeyCode.DownArrow, nameof(OnKeyCode_DownArrow));
        RegisterKeyDown(KeyCode.Space, nameof(OnKeyCode_Space));
        
        DontDestroyOnLoad(this);
    }
    
    private void Register(KeyCode code, string name)
    {
        Register(code, this.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
    }
    private void RegisterKeyDown(KeyCode code, string name)
    {
        RegisterKeyDown(code, this.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
    }

    private void Register(KeyCode code, MethodInfo methodInfo)
    {
        if (methodInfo == null)
            return;
        if (calls.ContainsKey(code.ToInt()))
            calls[code.ToInt()] = methodInfo;
        else
            calls.Add(code.ToInt(), methodInfo);
        keycode.Add(code);
    }

    private void RegisterKeyDown(KeyCode code, MethodInfo methodInfo)
    {
        if (methodInfo == null)
            return;
        if (keydown.ContainsKey(code.ToInt()))
            keydown[code.ToInt()] = methodInfo;
        else
            keydown.Add(code.ToInt(), methodInfo);
        keydownCode.Add(code);
    }

    public void Update()
    {
        for (int i = 0; i < keycode.Count; i++)
        {
            if (Input.GetKey(keycode[i]))
            {
                calls.TryGetValue(keycode[i].ToInt(), out var call);
                call?.Invoke(this, new object[] { });
            }
        }
        for (int i = 0; i < keydownCode.Count; i++)
        {
            if (Input.GetKeyDown(keydownCode[i]))
            {
                keydown.TryGetValue(keydownCode[i].ToInt(), out var call);
                call?.Invoke(this, new object[] { });
            }
        }
    }
    #endregion


    private void OnKeyCode_B()
    {
        // GameStageMachine.ChangeState<LoginStage>(true);
        EventMgr.Dispatch(BattleEvent.Battle_Camera_Shake, 1);
    }
    
    private void OnKeyCode_C()
    {
        if(GmView.Instance.IsOpen)
            return;
        UIMgr.Instance.ShowUI(UIPanelName.GmView);
    }

    private void OnKeyCode_Space()
    {
        int camp = SimulateModel.Instance.CampType.ToInt();
        var player = PlayerModel.Instance.RandomPlayer(camp);
        // SimulateModel.Instance.Simulate_Sen_douyin_like(player);
        SimulateModel.Instance.Simulate_Send_douyin_gift(player, $"6", 20);
    }
    
    private void OnKeyCode_End()
    {
        EventMgr.Dispatch(BattleEvent.Battle_GameIsOver, CampType.红);
    }

    public void OnKeyCode_LeftArrow()
    {
    }
    
    public void OnKeyCode_RightArrow()
    {
    }
    
    public void OnKeyCode_UpArrow()
    {
    }
    
    public void OnKeyCode_DownArrow()
    {
    }

    private void OnKeyCode_F1()
    {
        SimulateModel.Instance.CampType = CampType.蓝;
    }

    private void OnKeyCode_F2()
    {
        SimulateModel.Instance.CampType = CampType.红; 
    }
    
    /// <summary>
    /// 按钮 =
    /// </summary>
    public void OnKeyCode_Equals()
    {
        SimulateModel.Instance.Simulate_send_douyin_choose_side(1); 
    }

    /// <summary>
    /// 按钮 -
    /// </summary>
    public void OnKeyCode_Minus()
    {
    }

    public void OnKeyCode_Keypad1()
    {
        Simulate_Send_douyin_gift("1");
    }
    public void OnKeyCode_Keypad2()
    {
        Simulate_Send_douyin_gift("2");
    }
    public void OnKeyCode_Keypad3()
    {
        Simulate_Send_douyin_gift("3");
    }
    public void OnKeyCode_Keypad4()
    {
        if (GmView.Instance.IsOpen)
            return;
        int camp = SimulateModel.Instance.CampType.ToInt();
        var player = PlayerModel.Instance.RandomPlayer(camp);
        SimulateModel.Instance.Simulate_Sen_douyin_like(player);
    }
    public void OnKeyCode_Keypad5()
    {
        Simulate_Send_douyin_gift("5");
    }
    public void OnKeyCode_Keypad6()
    {
        Simulate_Send_douyin_gift("6");
    }
    public void OnKeyCode_Keypad7()
    {
        Simulate_Send_douyin_gift("7");
    }

    private void Simulate_Send_douyin_gift(string gift_id, long count = 1)
    {
        if (GmView.Instance.IsOpen)
            return;
        int camp = SimulateModel.Instance.CampType.ToInt();
        var player = PlayerModel.Instance.RandomPlayer(camp);
        SimulateModel.Instance.Simulate_Send_douyin_gift(player, gift_id, count);
    }
}

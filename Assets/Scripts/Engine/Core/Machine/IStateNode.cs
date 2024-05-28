using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Math.EC.Custom.Sec;
using UnityEngine;

public interface IStateNode
{
    void OnCreate(StateMachine machine);
    void OnEnter();
    void SysUpdate();
    void FixedUpdate();
    void LateUpdate();
    void OnExit();
}


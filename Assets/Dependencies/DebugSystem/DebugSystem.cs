using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DebugSystem
{
    public class DebugComponent : MonoBehaviour
    {
        private DebugManager System;
        private void Awake()
        {
            System = GetComponent<DebugManager>();
            if (System == null)
                System = gameObject.AddComponent<DebugManager>();
            DontDestroyOnLoad(this);
        }

        private void OnEnable() => System.isDebug = true;
        private void OnDisable() => System.isDebug = false;
    }
}

using DebugSystem.Interface;
using UnityEngine;

namespace DebugSystem
{
    internal abstract class DebugComponent<T> : IDebugComponent where T : Component
    {
        protected T target;
        
        protected abstract void OnDebugScene();

        object IDebugComponent.Target
        {
            set => target = (T)value;
        }

        void IDebugComponent.OnDebugScene() => OnDebugScene();
    }
}
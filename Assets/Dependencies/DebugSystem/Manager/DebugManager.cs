using Sirenix.OdinInspector;
using UnityEngine;

namespace DebugSystem
{
    internal partial class DebugManager : MonoBehaviour
    {
        [ShowInInspector, LabelText("是否启动"),BoxGroup("调试器")]
        public bool isDebug;

        private int FPS;
        private bool IsExtend;
        private Rect minRect;
        private Color FPSColor = Color.white;
        private DebugType debugType;
        private float interval = 1;
        private float timer;


        private void Awake()
        {
            InitComponent();
            Application.logMessageReceived += LogMessageReceived;
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            timer = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            if (Time.realtimeSinceStartup - timer > interval)
            {
                timer = Time.realtimeSinceStartup;
                if (!isDebug) return;
                FPS = (int)(1.0f / Time.deltaTime);
            }
        }

        private void OnGUI()
        {
            if (!isDebug) return;
            Matrix4x4 cachedMatrix = GUI.matrix;
            GUI.matrix = Matrix4x4.Scale(new Vector3(DebugData.WindowScale, DebugData.WindowScale, 1f));

            if (IsExtend)
            {
                Rect screenRect = new Rect(0, 0, DebugData.WindowWidth, DebugData.WindowHeight);
                GUI.Window(0, screenRect, MaxWindow, DebugConst.Debugger,
                    DebugData.Window);
            }
            else
            {
                minRect.size = new Vector2(100, 60);
                minRect = GUI.Window(0, minRect, MinWindow, DebugConst.Debugger, DebugData.Window);
            }

            GUI.matrix = cachedMatrix;
        }

        private void MinWindow(int windowId)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            GUI.contentColor = FPSColor;

            if (GUILayout.Button($"{DebugConst.FPS}: {FPS}", DebugData.Button, DebugData.Width, DebugData.Height))
            {
                IsExtend = true;
            }

            GUI.contentColor = Color.white;
        }

        private void MaxWindow(int windowId)
        {
            DebugTitle();
            switch (debugType)
            {
                case DebugType.Console:
                    DebugConsole();
                    break;
                case DebugType.Scene:
                    DebugScene();
                    break;
                case DebugType.Memory:
                    DebugMemory();
                    break;
                case DebugType.DrawCall:
                    DebugDrawCall();
                    break;
                case DebugType.System:
                    DebugSystem();
                    break;
                case DebugType.Screen:
                    DebugScreen();
                    break;
                case DebugType.Environment:
                    DebugProject();
                    break;
                case DebugType.Time:
                    DebugTime();
                    break;
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= LogMessageReceived;
        }
    }
}
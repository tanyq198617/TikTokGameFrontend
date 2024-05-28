using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameNetwork
{
    public static class LogExtension
    {
        public static string AppendColor(this object str, string color)
        {
            if (!color.StartsWith("#"))
                color = $"#{color}";
            return $"<color={color}>{str.ToString()}</color>";
        }

        public static string Bold(this object str)
        {
            return $"<b>{str.ToString()}</b>";
        }
    }


    public class NetDebug
    {
        public enum LogLevel
        {
            Log,
            Warning,
            Error,
            Fatal,
        }

        protected static string TAG = "[网络消息]".AppendColor("0099FF");
        protected static string NORMAL = "[提示]".AppendColor("FFFACD");
        protected static string MESSAGE = "[收发]".AppendColor("00FF33");
        protected static string WARNING = "[警告]".AppendColor("FFFF00");
        protected static string ERROR = "[错误]".AppendColor("FF33CC");
        protected static string FATAL = "[严重错误]".Bold().AppendColor("FF0000");

        protected static string SEND = "[上行]".AppendColor("FF6600");
        protected static string RECEIVE = "[下行]".AppendColor("00CC33");

        protected static bool isActive = true;
        protected static LogLevel Level = LogLevel.Log;

        public static void Send(object msg)
        {
            if (!isActive) return;
            if (Level > LogLevel.Error) return;
            Debug.LogError($"{TAG} {SEND} {msg.AppendColor("FFD700")}");
        }

        public static void Receive(object msg)
        {
            if (!isActive) return;
            if (Level > LogLevel.Error) return;
            Debug.LogError($"{TAG} {RECEIVE} {msg.AppendColor("FFD700")}");
        }

        public static void Log(object msg)
        {
            if (!isActive) return;
            if (Level > LogLevel.Log) return;
            Debug.Log($"{TAG} {NORMAL} {msg.AppendColor("FFFACD")}");
        }

        public static void Log(object msg, params object[] param)
        {
            if (!isActive) return;
            if (Level > LogLevel.Log) return;
            string str = string.Format(msg?.ToString(), param).AppendColor("FFFACD");
            Debug.Log($"{TAG} {NORMAL} {str}");
        }

        public static void LogWarning(object msg)
        {
            if (!isActive) return;
            if (Level > LogLevel.Warning) return;
            Debug.LogWarning($"{TAG} {WARNING} {msg.AppendColor("FFFF00")}");
        }

        public static void LogWarning(object msg, params object[] param)
        {
            if (!isActive) return;
            if (Level > LogLevel.Warning) return;
            string str = string.Format(msg?.ToString(), param).AppendColor("FFFF00");
            Debug.LogWarning($"{TAG} {WARNING} {str}");
        }

        public static void LogError(object msg)
        {
            if (!isActive) return;
            if (Level > LogLevel.Error) return;
            Debug.LogError($"{TAG} {ERROR} {msg.AppendColor("FF99CC")}");
        }

        public static void LogError(object msg, params object[] param)
        {
            if (!isActive) return;
            if (Level > LogLevel.Error) return;
            string str = string.Format(msg?.ToString(), param).AppendColor("FF99CC");
            Debug.LogError($"{TAG} {ERROR} {str}");
        }

        public static void Fatal(object msg)
        {
            if (!isActive) return;
            if (Level > LogLevel.Fatal) return;
            Debug.LogError($"{TAG} {FATAL} {msg.Bold().AppendColor("FF0000")}");
        }

        public static void Fatal(object msg, params object[] param)
        {
            if (!isActive) return;
            if (Level > LogLevel.Fatal) return;
            string str = string.Format(msg?.ToString(), param).Bold().AppendColor("FF0000");
            Debug.LogError($"{TAG} {FATAL} {str}");
        }

        public static void OnShowLog(bool active)
        {
            isActive = active;
        }

        public static void SetLevel(LogLevel level)
        {
            Level = level;
        }
    }
}
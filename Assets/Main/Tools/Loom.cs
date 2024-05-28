using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Loom : MonoBehaviour
    {
        public static int maxThreads = 16;
        static int numThreads;

        private static Loom _current;

        private int _count;
        public static Loom Current
        {
            get
            {
                Initialize();
                return _current;
            }
        }

        void Awake()
        {
            _current = this;
            initialized = true;
            DontDestroyOnLoad(gameObject);
        }

        static bool initialized;

        public static void Initialize()
        {
            if (!initialized)
            {

                if (!Application.isPlaying)
                    return;
                initialized = true;
                var g = new GameObject("Loom");
                _current = g.AddComponent<Loom>();
#if !ARTIST_BUILD
                DontDestroyOnLoad(g);
#endif
            }

        }

        private List<Action> _actions = new List<Action>();
        public struct DelayedQueueItem
        {
            public float time;
            public Action action;
        }
        private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();

        public static void QueueOnMainThread(Action<object> taction, object tparam)
        {
            QueueOnMainThread(taction,tparam, 0f);
        }
        public static void QueueOnMainThread(Action<object> taction, object tparam, float time)
        {
            if (time > 0f)
            {
                lock (Current._delayed)
                {
                    Current._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = ()=>taction(tparam) });
                }
            }
            else
            {
                lock (Current._actions)
                {
                    Current._actions.Add(()=>taction(tparam));
                }
            }
        }

        public static Thread RunAsync(Action a)
        {
            while (numThreads >= maxThreads)
            {
                Thread.Sleep(1);
            }
            Interlocked.Increment(ref numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, a);
            return null;
        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                Interlocked.Decrement(ref numThreads);
            }

        }

        private List<Action> curActions = new List<Action>();
        private List<DelayedQueueItem> curDelayeds = new List<DelayedQueueItem>();
        // Update is called once per frame
        void Update()
        {
            lock (_actions)
            {
                if (_actions.Count > 0)
                {
                    curActions.AddRange(_actions);
                    _actions.Clear();
                }
            }

            if (curActions.Count > 0)
            {
                foreach (var a in curActions)
                {
                    try
                    {
                        a();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                curActions.Clear();
            }

            lock (_delayed)
            {
                var i = _delayed.Count - 1;
                while (i >= 0)
                {
                    var item = _delayed[i];
                    if (item.time <= Time.time)
                    {
                        curDelayeds.Add(item);
                        _delayed.RemoveAt(i);
                    }
                    i--;
                }
            }

            if (curDelayeds.Count > 0)
            {
                foreach (var item in curDelayeds)
                {
                    try
                    {
                        item.action();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                curDelayeds.Clear();
            }
        }
    }
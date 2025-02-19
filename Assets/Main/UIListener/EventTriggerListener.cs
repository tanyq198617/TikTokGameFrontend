using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void UIEventHandle<T>(GameObject go, T eventData) where T : BaseEventData;

public class EventTriggerListener :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerUpHandler,
    ISelectHandler,
    IUpdateSelectedHandler,
    IDeselectHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IDropHandler,
    IScrollHandler,
    IMoveHandler
{    
    public class EventHandle<T> where T : BaseEventData
    {
        private event UIEventHandle<T> m_Handle = null;

        public void AddListener(UIEventHandle<T> handle)
        {
            m_Handle += handle;
        }

        public void RemoveListener(UIEventHandle<T> handle)
        {
            m_Handle -= handle;
        }

        public void RemoveAllListener()
        {
            m_Handle -= m_Handle;
            m_Handle = null;
        }

        public void Invoke(GameObject go, T eventData)
        {
            m_Handle?.Invoke(go, eventData);
        }
    }

    public EventHandle<PointerEventData> onClick = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onDoubleClick = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onPress = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onUp = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onDown = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onEnter = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onExit = new EventHandle<PointerEventData>();
    public EventHandle<BaseEventData> onSelect = new EventHandle<BaseEventData>();
    public EventHandle<BaseEventData> onUpdateSelect = new EventHandle<BaseEventData>();
    public EventHandle<BaseEventData> onDeselect = new EventHandle<BaseEventData>();
    public EventHandle<PointerEventData> onBeginDrag = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onDrag = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onEndDrag = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onDrop = new EventHandle<PointerEventData>();
    public EventHandle<PointerEventData> onScroll = new EventHandle<PointerEventData>();
    public EventHandle<AxisEventData> onMove = new EventHandle<AxisEventData>();

    public void OnPointerClick(PointerEventData eventData) { }
    public void OnPointerEnter(PointerEventData eventData) { onEnter.Invoke(gameObject, eventData); }
    public void OnPointerExit(PointerEventData eventData) { onExit.Invoke(gameObject, eventData); }
    public void OnSelect(BaseEventData eventData) { onSelect.Invoke(gameObject, eventData); }
    public void OnUpdateSelected(BaseEventData eventData) { onUpdateSelect.Invoke(gameObject, eventData); }
    public void OnDeselect(BaseEventData eventData) { onDeselect.Invoke(gameObject, eventData); }
    public void OnBeginDrag(PointerEventData eventData) { m_IsDraging = true; m_Delta = eventData.delta; onBeginDrag.Invoke(gameObject, eventData); }
    public void OnDrag(PointerEventData eventData) { m_IsDraging = true; m_IsTryClick = false; onDrag.Invoke(gameObject, eventData); }
    public void OnEndDrag(PointerEventData eventData) { m_IsDraging = false; onEndDrag.Invoke(gameObject, eventData); }
    public void OnDrop(PointerEventData eventData) { onDrop.Invoke(gameObject, eventData); }
    public void OnScroll(PointerEventData eventData) { onScroll.Invoke(gameObject, eventData); }
    public void OnMove(AxisEventData eventData) { onMove.Invoke(gameObject, eventData); }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_IsPointDown = true;
        m_IsPress = false;
        m_IsTryClick = true;
        m_IsDraging = false;

        m_CurrDownTime = Time.unscaledTime;
        onDown?.Invoke(gameObject, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_IsPointDown = false;
        m_OnUpEventData = eventData;
        if (!m_IsPress)
        {
            m_ClickCount++;
        }
        else
        {
            m_IsPress = false;
            onUp?.Invoke(gameObject, m_OnUpEventData);
        }
    }

    public static EventTriggerListener Get(GameObject go)
    {
        if (go == null)
            return null;
        EventTriggerListener eventTrigger = go.GetComponent<EventTriggerListener>();
        if (eventTrigger == null) eventTrigger = go.AddComponent<EventTriggerListener>();
        return eventTrigger;
    }

    private const float DOUBLE_CLICK_TIME = 0.2f;
    private const float CLICK_TIME = 0.1f;
    private const float PRESS_TIME = 0.5F;
    
    private static float m_CurrClickTime = 0f;
    private static int m_ClickInstance;

    private Vector2 m_Delta;
    private float m_CurrDownTime = 0f;
    private bool m_IsPointDown = false;
    private bool m_IsPress = false;
    private bool m_IsDraging = false;
    private bool m_IsTryClick = false;
    private int m_ClickCount = 0;
    private PointerEventData m_OnUpEventData = null;
    public bool IsPress { get { return m_IsPress; } }
    public bool IsEnableDoubleClick { get; set; } = false;

    private void Update()
    {
        if (Input.touchCount > 1)
            return;

        if (m_IsPointDown)
        {
            if (Time.unscaledTime - m_CurrDownTime >= PRESS_TIME)
            {
                m_IsPress = true;
                m_IsPointDown = false;
                m_CurrDownTime = 0f;
                onPress?.Invoke(gameObject, null);
            }
        }

        if (m_ClickCount > 0)
        {
            if (!IsEnableDoubleClick || Time.unscaledTime - m_CurrDownTime >= DOUBLE_CLICK_TIME)
            {
                if (m_ClickCount < 2)
                {
                    //onUp?.Invoke(gameObject, m_OnUpEventData);
                    if (m_IsTryClick && !m_IsDraging)
                    {
                        if (m_ClickInstance != this.GetInstanceID())
                        {
                            if (Time.unscaledTime - m_CurrClickTime >= CLICK_TIME)
                            {
                                onClick?.Invoke(gameObject, m_OnUpEventData);
                                m_CurrClickTime = Time.unscaledTime;
                                m_ClickInstance = this.GetInstanceID();
                            }
                        }
                        else 
                        {
                            onClick?.Invoke(gameObject, m_OnUpEventData);
                            m_CurrClickTime = Time.unscaledTime;
                            m_ClickInstance = this.GetInstanceID();
                        }
                    }
                    m_OnUpEventData = null;
                }
                m_ClickCount = 0;
            }

            if (m_ClickCount > 1)
            {
                onDoubleClick?.Invoke(gameObject, m_OnUpEventData);
                m_OnUpEventData = null;
                m_ClickCount = 0;
            }
        }
    }

    private void OnDestroy()
    {
        RemoveUIListener();
    }

    private void RemoveUIListener()
    {
        onClick.RemoveAllListener();
        onDoubleClick.RemoveAllListener();
        onDown.RemoveAllListener();
        onEnter.RemoveAllListener();
        onExit.RemoveAllListener();
        onUp.RemoveAllListener();
        onSelect.RemoveAllListener();
        onUpdateSelect.RemoveAllListener();
        onDeselect.RemoveAllListener();
        onDrag.RemoveAllListener();
        onEndDrag.RemoveAllListener();
        onDrop.RemoveAllListener();
        onScroll.RemoveAllListener();
        onMove.RemoveAllListener();
    }
}
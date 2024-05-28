using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

internal class ButtonAnimationInMain : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerExitHandler
{
    public float _rawScale = 1.0f;
    public float _pressScale = 1.1f;
    private Tween tweener;

    void Start()
    {
        _rawScale = transform.localScale.x;
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.pivot = Vector2.one * 0.5f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TweenKill();
        tweener = transform.DOScale(_pressScale, 0.2f);
        tweener.SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TweenKill();
        tweener = transform.DOScale(_rawScale, 0.1f);
        tweener.SetUpdate(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        TweenKill();
        tweener = transform.DOScale(_rawScale, 0.1f);
        tweener.SetUpdate(true);
    }

    private void TweenKill()
    {
        if (tweener != null)
        {
            tweener.Kill(false);
            tweener = null;
        }
    }

    private void OnDestroy()
    {
        TweenKill();
    }
}

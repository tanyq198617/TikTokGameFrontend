using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IItemBase : IUpdate
{
    void setObj(GameObject obj);

    void Refresh();

    void Refresh<T>(T data);

    void OnClick(GameObject obj, PointerEventData eventData);

    void Destory();

    void sendPara(object args);
}


public class ALoopItemBase : AItemBase
{
    public new int index;

    public virtual void PlaySelfTween() { }
    public virtual void RefreshFlag() { }
}
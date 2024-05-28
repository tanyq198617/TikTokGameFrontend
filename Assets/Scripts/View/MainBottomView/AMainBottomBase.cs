using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AMainBottomBase : AItemButtonBase
{
    private GameObject inselect;
    private GameObject unselect;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        inselect = UIUtility.Control("inselect", m_gameobj);
        unselect = UIUtility.Control("unselect", m_gameobj);

        UIUtility.BindClickEvent(m_gameobj, OnClick);
    }

    public override void OnSelect()
    {
        base.OnSelect();
        inselect.SetActiveEX(true);
        unselect.SetActiveEX(false);
    }

    public override void NoSelect()
    {
        base.NoSelect();
        inselect.SetActiveEX(false);
        unselect.SetActiveEX(true);
    }

    public abstract int GetIndex();

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(m_gameobj))
        {
            OnItemClick?.Invoke(GetIndex());
        }
    }
}

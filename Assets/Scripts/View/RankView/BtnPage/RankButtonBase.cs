using UnityEngine;
using UnityEngine.EventSystems;

public abstract class RankButtonBase : AItemButtonBase
{
    private GameObject inselect;
    private GameObject unselect;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        inselect = UIUtility.Control("choose", m_gameobj);
        unselect = UIUtility.Control("normal", m_gameobj);

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

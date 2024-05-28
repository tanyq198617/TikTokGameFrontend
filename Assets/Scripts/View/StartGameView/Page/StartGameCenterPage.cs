using UnityEngine;
using UnityEngine.EventSystems;

public class StartGameCenterPage : AItemPageBase
{
    private GameObject btn_guidance;

    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        btn_guidance = UIUtility.BindClickEvent(RectTrans, "btn_guidance", OnClick);
    }

    public override void Refresh()
    {
        base.Refresh();
        btn_guidance.SetActiveEX(true);
    }

    public override void OnClick(GameObject obj, PointerEventData eventData)
    {
        base.OnClick(obj, eventData);

        if (obj.Equals(btn_guidance))
        {
            btn_guidance.SetActiveEX(false);
        }
    }
}
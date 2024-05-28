using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIPanelName.ShopView)]
public class ShopView : APanelBase
{
    public static ShopView Instance { get { return Singleton<ShopView>.Instance; } }

    public ShopView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }
}

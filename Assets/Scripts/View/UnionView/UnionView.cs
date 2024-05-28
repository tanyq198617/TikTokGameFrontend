using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIPanelName.UnionView)]
public class UnionView : APanelBase
{
    public static UnionView Instance { get { return Singleton<UnionView>.Instance; } }

    public UnionView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }
}

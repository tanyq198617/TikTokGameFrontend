using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIPanelName.RoleView)]
public class RoleView : APanelBase
{
    public static RoleView Instance { get { return Singleton<RoleView>.Instance; } }

    public RoleView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }
}

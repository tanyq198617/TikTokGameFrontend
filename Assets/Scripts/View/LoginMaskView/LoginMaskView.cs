using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIPanelName.LoginMaskView)]
public class LoginMaskView : APanelBase
{
    public static LoginMaskView Instance { get { return Singleton<LoginMaskView>.Instance; } }

    public LoginMaskView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.Mask;
        m_IsKeepOpen = true;
    }
}

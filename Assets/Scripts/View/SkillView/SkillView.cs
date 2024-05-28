using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UIBind(UIPanelName.SkillView)]
public class SkillView : APanelBase
{
    public static SkillView Instance { get { return Singleton<SkillView>.Instance; } }

    public SkillView() : base()
    {
        isFilm = true;
        m_Type = UIPanelType.One;
    }
}

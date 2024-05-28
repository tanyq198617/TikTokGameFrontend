using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFactory : AFactory
{
    public static TestFactory Instance => Singleton<TestFactory>.Instance;

    public TestFactory()
    {
        m_IsDefaultRoot = true;
        m_AutoLoad = true;
        m_prefabPath = PathConst.GetBattleItemPath("球球_简单大球_红");
    }
}


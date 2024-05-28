using UnityEngine;

/// <summary>
/// 大哥对象池获取模型后,往预制体上添加的脚本
/// </summary>
public class RedRichManControl : ARichManControlBase
{
   protected override CampType camp => CampType.红;
   public override void Recycle()
   {
      GameObjectFactory.Recycle(this);
   }
}
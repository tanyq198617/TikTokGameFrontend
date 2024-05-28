using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;
using System.Collections.Generic;

public class FindMissingScriptObject : EditorWindow
{

    [MenuItem("脚本处理/查找有missing脚本的物体")]
    public static void TryFindMissingScriptObject()
    {
        /*******************************************************************************************
         * 1、编辑器选中n个物体
         * 2、获取这n个物体及其它们的子物体，数据集记为A
         * 3、A中全部物体判断他们是否有null的脚本（MonoBehaviour）
         * 
         * 判断一个物体(object)是否有空脚本：obj.GetComponents<MonoBehaviour>().Any(mono => mono == null)
         *******************************************************************************************/

        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        string[] files = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories);
        List<GameObject> _prefabList = new List<GameObject>();
        GameObject _prefab;
        foreach (var _path in files)
        {
            _prefab = AssetDatabase.LoadAssetAtPath(_path, typeof(GameObject)) as GameObject;
            _prefabList.Add(_prefab);
        }

        Debug.Log($"选中的物体数量为：{_prefabList.Count}");

        foreach (var root in _prefabList)
        {
            var trans = root.GetComponentsInChildren<Transform>().ToList();
            trans.ForEach(obj =>
            {
                //1、该物体是否有null的脚本
                var hasNullScript = obj.GetComponents<MonoBehaviour>().Any(mono => mono == null); //注意:用【MonoBehaviour】而不是用【MonoScript】

                //2、Debug物体名字
                if (hasNullScript)
                {
                    Debug.Log($"物体 【{root.name}-->{obj.name}】 上有Missing的脚本");
                }
            });
        }

        //var allObjs = _prefabList.SelectMany(obj => obj.GetComponentsInChildren<Transform>().Select(x => x.gameObject)).ToList();
        //Debug.Log($"选中的物体及其子物体的数量为：{allObjs.Count()}");

        //allObjs.ForEach(obj =>
        //{
        //    //1、该物体是否有null的脚本
        //    var hasNullScript = obj.GetComponents<MonoBehaviour>().Any(mono => mono == null); //注意:用【MonoBehaviour】而不是用【MonoScript】
        //    //Debug.Log($"是否有空脚本：{hasNullScript}，物体名字：【{obj.name}】");

        //    //2、Debug物体名字
        //    if (hasNullScript)
        //    {
        //        Debug.Log($"物体 【{obj.name}】 上有Missing的脚本");
        //    }
        //});
    }
}


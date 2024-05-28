using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

/// <summary>
/// 自动写UI脚本
/// </summary>
public class CreateUIScriptsEditor : OdinEditorWindow
{
    [MenuItem("脚本处理/自动生成Prefab对应脚本", false, 501)]
    private static void OpenWindow() 
    {
        GetWindow<CreateUIScriptsEditor>().Show();
    }

    [MenuItem("脚本处理/关闭自动生成Prefab对应脚本", false, 502)]
    private static void CloseWindow()
    {
        GetWindow<CreateUIScriptsEditor>().Close();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        PanelTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/UIEditor/PanelTemplete.txt");
        PageTemplete = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/UIEditor/PageTemplete.txt");
        SavePathBase = SavePath;
        OnPagePrefixChanged();
        OnTextPrefixChanged();
        OnImagePrefixChanged();
        OnButtonPrefixChanged();
    }

    [TitleGroup("模板文件")]
    public TextAsset PanelTemplate;

    [TitleGroup("模板文件")]
    public TextAsset PageTemplete;

    [TitleGroup("搜索前缀, \"|\" 号分割")]
    [SuffixLabel("Panel搜索前缀")]
    public string viewPrefix = "uiview_";

    [SuffixLabel("Page搜索前缀")]
    [OnValueChanged("OnPagePrefixChanged")]
    public string pagePrefix = "uiitem_|page_";

    [SuffixLabel("TextMeshPro搜索前缀")]
    [OnValueChanged("OnTextPrefixChanged")]
    public string textPrefix = "lb_|tx_";

    [SuffixLabel("Image搜索前缀")]
    [OnValueChanged("OnImagePrefixChanged")]
    public string imagePrefix = "img_";

    [SuffixLabel("Button搜索前缀")]
    [OnValueChanged("OnButtonPrefixChanged")]
    public string btnPrefix = "btn_";

    [TitleGroup("预制体*")]
    [OnValueChanged("OnPrefabChanged")]
    public GameObject Prefab;

    [TitleGroup("脚本名字")]
    public string PanelViewName;

    [TitleGroup("保存路径")]
    public string SavePath = "Assets/Scripts/View/";
    private string SavePathBase;

    private HashSet<string> pagePrefixHash = new HashSet<string>();
    private HashSet<string> txPrefixHash = new HashSet<string>();
    private HashSet<string> imgPrefixHash = new HashSet<string>();
    private HashSet<string> btnPrefixHash = new HashSet<string>();


    [TitleGroup("生成")]
    [Button("一键生成对应脚本", ButtonSizes.Large), GUIColor(0, 1, 0)]
    private void Save()
    {
        if (Prefab == null) 
            return;

        if (EditorUtility.DisplayDialog("提示", $"生成预制体【{Prefab.name}】脚本到路径\n\n{SavePath}", "生成", "取消"))
        {
            EditorTools.ClearUnityConsole();
            EditorApplication.delayCall += ExecuteBuild;
        }
    }

    private void ExecuteBuild()
    {
        RectTransform root = Prefab.GetComponent<RectTransform>();
        if (root == null)
        {
            Debug.LogError($"只能生成UI相关预制体");
            return;
        }
        WritePanelScript(root);
        Debug.LogError($"生成【{PanelViewName}】完毕!");
    }

    private string panelName;

    public void OnPrefabChanged() 
    {
        if (Prefab == null)
        {
            PanelViewName = "";
            return;
        }
        string name = Prefab.name;
        var val = name.Replace(viewPrefix, "").Split('_');
        StringBuilder builder = new StringBuilder();
        if (val.Length > 0) 
        {
            foreach (var item in val)
                builder.Append(GetUpperWord(item));
            panelName = builder.ToString();
            builder.Append("View");
        }
        PanelViewName = builder.ToString();
        SavePath = $"{SavePathBase}{PanelViewName}/";
    }

    public void OnPagePrefixChanged() 
    {
        pagePrefixHash.Clear();
        var arr = pagePrefix.Split('|');
        pagePrefixHash.UnionWith(arr);
    }

    public void OnTextPrefixChanged()
    {
        txPrefixHash.Clear();
        var arr = textPrefix.Split('|');
        txPrefixHash.UnionWith(arr);
    }

    public void OnImagePrefixChanged()
    {
        imgPrefixHash.Clear();
        var arr = imagePrefix.Split('|');
        imgPrefixHash.UnionWith(arr);
    }

    public void OnButtonPrefixChanged()
    {
        btnPrefixHash.Clear();
        var arr = btnPrefix.Split('|');
        btnPrefixHash.UnionWith(arr);
    }

    private bool IsStartsWithButton(string name) 
    {
        if(string.IsNullOrEmpty(name))
            return false;
        foreach (var item in btnPrefixHash)
        {
            if (name.StartsWith(item))
                return true;
        }
        return false;
    }

    private bool IsStartsWithTextMeshPro(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        foreach (var item in txPrefixHash)
        {
            if (name.StartsWith(item))
                return true;
        }
        return false;
    }

    private bool IsStartsWithImage(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        foreach (var item in imgPrefixHash)
        {
            if (name.StartsWith(item))
                return true;
        }
        return false;
    }

    private bool IsStartsWithPage(string name)
    {
        if (string.IsNullOrEmpty(name))
            return false;
        foreach (var item in pagePrefixHash)
        {
            if (name.StartsWith(item))
                return true;
        }
        return false;
    }

    /// <summary>
    /// 写出Panel脚本
    /// </summary>
    /// <param name="root"></param>
    private void WritePanelScript(RectTransform root)
    {
        List<RectTransform> pages = new List<RectTransform>();
        List<RectTransform> btns = new List<RectTransform>();
        List<RectTransform> labels = new List<RectTransform>();
        List<RectTransform> images = new List<RectTransform>();

        StringBuilder fieldsBuilder = new StringBuilder();
        StringBuilder initBuilder = new StringBuilder();
        StringBuilder clickBuilder = new StringBuilder();

        foreach (RectTransform item in root)
        {
            if (item.name.Equals("Top") ||
                item.name.Equals("TopLeft") ||
                item.name.Equals("TopRight") ||
                item.name.Equals("Left") || 
                item.name.Equals("Center") ||
                item.name.Equals("Right") ||
                item.name.Equals("Bottom") ||
                item.name.Equals("BottomLeft") ||
                item.name.Equals("BottomRight") ||
                IsStartsWithPage(item.name)
                ) 
            {
                pages.Add(item);
                WritePageScript(item, panelName);
            }

            if (IsStartsWithButton(item.name)) 
            {
                btns.Add(item);
            }

            if (IsStartsWithTextMeshPro(item.name))
            {
                labels.Add(item);
            }

            if (IsStartsWithImage(item.name))
            {
                images.Add(item);
            }
        }

        string writeOut = PanelTemplate.text;

        if (pages.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendPage(pages, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, panelName)), (type, page) => initBuilder.AppendLine(CreateInitScript(type, page, panelName)));
        }

        if (labels.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendLabels(labels, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, panelName)), (type, page) => initBuilder.AppendLine(CreateInitScript(type, page, panelName)));
        }

        if (images.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendImages(images, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, panelName)), (type, page) => initBuilder.AppendLine(CreateInitScript(type, page, panelName)));
        }

        if (btns.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendButton(btns, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, panelName)), (type, page) => initBuilder.AppendLine(CreateInitScript(type, page, panelName)));

            for (int i = 0; i < btns.Count; i++)
            {
                if (i == 0)
                    clickBuilder.AppendLine($"        if (obj.Equals({btns[i].name})) \r\n        {{\r\n        }}");
                else
                    clickBuilder.AppendLine($"        else if (obj.Equals({btns[i].name})) \r\n        {{\r\n        }}");
            }
        }

        writeOut = writeOut.Replace("$PanelName", PanelViewName);
        writeOut = writeOut.Replace("$fieldsBuilder", fieldsBuilder.ToString());
        writeOut = writeOut.Replace("$initBuilder", initBuilder.ToString());
        writeOut = writeOut.Replace("$clickBuilder", clickBuilder.ToString());

        SavePath = !SavePath.EndsWith("/") ? $"{SavePath}/" : SavePath;
        if(!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
        string filePath = $"{SavePath}{PanelViewName}.cs";
        if(!File.Exists(filePath)) File.Delete(filePath);
        AssetDatabase.Refresh();
        File.WriteAllText(filePath, writeOut);
        AssetDatabase.Refresh();
    }

    /// <summary> 写出Page变量 </summary>
    private void WritePageScript(RectTransform root, string rootName)
    {
        string prefix = $"{rootName}{GetSplitWord(ReplacePrefix(root.name))}";
        string ClassName = $"{prefix}Page";
        string writeOut = PageTemplete.text;

        StringBuilder fieldsBuilder = new StringBuilder();
        StringBuilder initBuilder = new StringBuilder();
        StringBuilder clickBuilder = new StringBuilder();

        List<RectTransform> pages = new List<RectTransform>();
        List<RectTransform> btns = new List<RectTransform>();
        List<RectTransform> labels = new List<RectTransform>();
        List<RectTransform> images = new List<RectTransform>();
        FindAllNodes(root, prefix, pages, btns, labels, images);

        if (pages.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendPage(pages, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, prefix)), (type, page) => initBuilder.AppendLine(CreateSetobjScript(type, page, prefix)));
        }

        if (labels.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendLabels(labels, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, prefix)), (type, page) => initBuilder.AppendLine(CreateSetobjScript(type, page, prefix)));
        }

        if (images.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendImages(images, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, prefix)), (type, page) => initBuilder.AppendLine(CreateSetobjScript(type, page, prefix)));
        }

        if (btns.Count > 0)
        {
            fieldsBuilder.AppendLine();
            initBuilder.AppendLine();
            AppendButton(btns, (type, page) => fieldsBuilder.AppendLine(CreateDeclaration(type, page, prefix)), (type, page) => initBuilder.AppendLine(CreateSetobjScript(type, page, prefix)));
            
            for (int i = 0; i < btns.Count; i++)
            {
                if (i == 0)
                    clickBuilder.AppendLine($"        if (obj.Equals({btns[i].name})) \r\n        {{\r\n        }}");
                else
                    clickBuilder.AppendLine($"        else if (obj.Equals({btns[i].name})) \r\n        {{\r\n        }}");
            }
        }

        writeOut = writeOut.Replace("$PageName", ClassName);
        writeOut = writeOut.Replace("$fieldsBuilder", fieldsBuilder.ToString());
        writeOut = writeOut.Replace("$initBuilder", initBuilder.ToString());
        writeOut = writeOut.Replace("$clickBuilder", clickBuilder.ToString());

        string pagePath = !SavePath.EndsWith("/") ? $"{SavePath}/" : SavePath;
        pagePath = $"{pagePath}Page/";
        if (!Directory.Exists(pagePath)) Directory.CreateDirectory(pagePath);
        string filePath = $"{pagePath}{ClassName}.cs";
        if (!File.Exists(filePath)) File.Delete(filePath);
        AssetDatabase.Refresh();
        File.WriteAllText(filePath, writeOut);
        AssetDatabase.Refresh();
    }

    private void FindAllNodes(RectTransform root, string prefix, List<RectTransform> pages, List<RectTransform> btns, List<RectTransform> labels, List<RectTransform> images)
    {
        foreach (RectTransform item in root)
        {
            if (IsStartsWithPage(item.name))
            {
                pages.Add(item);
                WritePageScript(item, prefix);
                continue;
            }

            if (IsStartsWithButton(item.name))
            {
                btns.Add(item);
                continue;
            }

            if (IsStartsWithTextMeshPro(item.name))
            {
                labels.Add(item);
                continue;
            }

            if (IsStartsWithImage(item.name))
            {
                images.Add(item);
                continue;
            }

            FindAllNodes(item, prefix, pages, btns, labels, images);
        }
    }


    private void AppendPage(List<RectTransform> pages, Func<int, RectTransform, StringBuilder> fieldsFunc, Func<int, RectTransform, StringBuilder> initFunc) 
    {
        if (pages.Count <= 0)
            return;
        for (int i = 0; i < pages.Count; i++)
        {
            fieldsFunc.Invoke(0, pages[i]);
            initFunc.Invoke(0, pages[i]);
        }
    }

    private void AppendButton(List<RectTransform> btns, Func<int, RectTransform, StringBuilder> fieldsFunc, Func<int, RectTransform, StringBuilder> initFunc)
    {
        if (btns.Count <= 0)
            return;
        for (int i = 0; i < btns.Count; i++)
        {
            fieldsFunc.Invoke(1, btns[i]);
            initFunc.Invoke(1, btns[i]);
        }
    }

    private void AppendLabels(List<RectTransform> labels, Func<int, RectTransform, StringBuilder> fieldsFunc, Func<int, RectTransform, StringBuilder> initFunc)
    {
        if (labels.Count <= 0)
            return;
        for (int i = 0; i < labels.Count; i++)
        {
            fieldsFunc.Invoke(2, labels[i]);
            initFunc.Invoke(2, labels[i]);
        }
    }

    private void AppendImages(List<RectTransform> images, Func<int, RectTransform, StringBuilder> fieldsFunc, Func<int, RectTransform, StringBuilder> initFunc)
    {
        if (images.Count <= 0)
            return;
        for (int i = 0; i < images.Count; i++)
        {
            fieldsFunc.Invoke(3, images[i]);
            initFunc.Invoke(3, images[i]);
        }
    }

    /// <summary> 声明变量 </summary>
    private string CreateDeclaration(int type, RectTransform node, string prefix) 
    {
        switch (type) 
        {
            //Page 类型
            case 0:
                string ClassName = $"{prefix}{GetSplitWord(ReplacePrefix(node.name))}Page";
                string fieldName = $"{GetSplitWord(ReplacePrefix(node.name), false)}Page";
                return $"    private {ClassName} {fieldName};";

            //button 类型
            case 1: return $"    private GameObject {node.name};";

            //TextMeshProUGUI 类型
            case 2: return $"    private TextMeshProUGUI {node.name};";

            //Image 类型
            case 3: return $"    private Image {node.name};";
        }
        return "";
    }

    /// <summary> 实现变量 </summary>
    private string CreateInitScript(int type, RectTransform node, string prefix)
    {
        switch (type)
        {
            //Page 类型
            case 0:
                string ClassName = $"{prefix}{GetSplitWord(ReplacePrefix(node.name))}Page";
                string fieldName = $"{GetSplitWord(ReplacePrefix(node.name), false)}Page";
                return $"        {fieldName} = UIUtility.CreatePageNoClone<{ClassName}>(Trans, \"{node.name}\");";

            //button 类型
            case 1: return $"        {node.name} = UIUtility.BindClickEvent(Trans, \"{node.name}\", OnClick);";

            //TextMeshProUGUI 类型
            case 2: return $"        {node.name} = UIUtility.GetComponent<TextMeshProUGUI>(Trans, \"{node.name}\");";

            //Image 类型
            case 3: return $"        {node.name} = UIUtility.GetComponent<Image>(Trans, \"{node.name}\");";
        }
        return "";
    }


    /// <summary> 实现变量 </summary>
    private string CreateSetobjScript(int type, RectTransform node, string prefix)
    {
        switch (type)
        {
            //Page 类型
            case 0:
                string ClassName = $"{prefix}{GetSplitWord(ReplacePrefix(node.name))}Page";
                string fieldName = $"{GetSplitWord(ReplacePrefix(node.name), false)}Page";
                return $"        {fieldName} = UIUtility.CreatePageNoClone<{ClassName}>(RectTrans, \"{node.name}\");";

            //button 类型
            case 1: return $"        {node.name} = UIUtility.BindClickEvent(RectTrans, \"{node.name}\", OnClick);";

            //TextMeshProUGUI 类型
            case 2: return $"        {node.name} = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, \"{node.name}\");";

            //Image 类型
            case 3: return $"        {node.name} = UIUtility.GetComponent<Image>(RectTrans, \"{node.name}\");";
        }
        return "";
    }

    private string ReplacePrefix(string str)
    {
        foreach (var item in pagePrefixHash)
            str = str.Replace(item, "");
        return str;
    }

    private string GetUpperWord(string str) => !string.IsNullOrEmpty(str) ? char.ToUpper(str[0]) + str.Substring(1) : str;
    private string GetLowerWord(string str) => !string.IsNullOrEmpty(str) ? char.ToLower(str[0]) + str.Substring(1) : str;
    private string GetSplitWord(string str, bool firstUpper = true)
    {
        if (string.IsNullOrEmpty(str))
            return str;
        var val = str.Split('_');
        if (val.Length > 0)
        {
            StringBuilder builder = new StringBuilder(); 
            int i = 0;
            foreach (var item in val)
            {
                if (i == 0)
                {
                    if (firstUpper) builder.Append(GetUpperWord(item));
                    else builder.Append(GetLowerWord(item));
                }
                else
                    builder.Append(GetUpperWord(item));
            }
            return builder.ToString(); 
        } 
        return str; 
    }

}

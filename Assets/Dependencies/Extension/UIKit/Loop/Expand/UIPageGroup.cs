using System;
using System.Collections.Generic;
using UnityEngine;

public class APageButtonBase : ASelectItemBase
{
    public APageBase BindPage;
    public int BindIndex;
    public Delegate OnItemClick { get; set; }
    public Action onValueChanged { get; set; }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;

            if (BindPage != null)
                BindPage.IsActive = value;
        }
    }
}

public class APageBase : AItemPageBase
{
    public virtual void OnRebuild()
    {
    }
}

public class UIPageGroup<TButton, TButtonData> where TButton : APageButtonBase
{
    /// <summary> 所有页签 key=按钮, value=页签 </summary>
    //private Dictionary<TButton, APageBase> pageDict = new Dictionary<TButton, APageBase>();

    /// <summary> 所有按钮 </summary>
    private List<TButton> buttonList = new List<TButton>();

    /// <summary> 所有按钮 </summary>
    private List<APageBase> pageList = new List<APageBase>();

    private GameObject templete;

    private RectTransform root;

    private Action<TButton> onButtonClick;

    private TButton currentButton;

    private APageBase currentPage;

    public Func<TButton, AItemPageBase, bool> onTryChange;

    public Action<TButton, AItemPageBase> onPageChange;

    public APageBase CurrentPage => currentPage;

    public void setObj(GameObject templete, Action<TButton> bindItemEvent = null, bool localButton = false)
    {
        if (templete == null)
        {
            Debug.LogError($"初始化错误, 不存在的实体节点!!!");
            return;
        }

        this.buttonList?.Clear();
        this.templete = templete;
        this.templete.SetActiveEX(false);
        this.root = templete.transform.parent.GetComponent<RectTransform>();
        this.onButtonClick = bindItemEvent;

        if (localButton)
        {
            foreach (Transform trans in root.transform)
            {
                if (trans == null)
                    continue;
                if (templete == trans)
                    continue;
                TButton button = UIUtility.CreateItemNoClone<TButton>(trans.gameObject);
                if (button == null)
                {
                    trans?.SetActiveEX(false);
                    continue;
                }

                button.IsActive = false;
                buttonList.Add(button);
            }
        }
    }

    public void DefaultRefresh(Dictionary<TButtonData, APageBase> pages, int firstPage = 0)
    {
        if (pages == null)
        {
            Debug.LogError($"不存在的按钮列表");
            return;
        }

        List<TButtonData> btnDatas = new List<TButtonData>(pages.Keys);
        pageList = new List<APageBase>();
        int count = Mathf.Max(btnDatas.Count, buttonList.Count);
        int buttonsCount = buttonList.Count;

        for (int i = 0; i < count; i++)
        {
            if (i < buttonsCount)
            {
                buttonList[i].IsSelect = i < btnDatas.Count;
                if (i < btnDatas.Count)
                {
                    buttonList[i].SetActive(true);
                    buttonList[i].BindPage = pages[btnDatas[i]];
                    buttonList[i].BindIndex = i;
                    buttonList[i].OnItemClick = (Action<TButton>)OnButtonClick;
                    buttonList[i].Refresh(btnDatas[i]);
                    pageList.Add(pages[btnDatas[i]]);
                }
            }
            else
            {
                TButton button = UIUtility.CreateItem<TButton>(templete, root);
                button.SetActive(true);
                button.BindPage = pages[btnDatas[i]];
                button.BindIndex = i;
                button.OnItemClick = (Action<TButton>)OnButtonClick;
                button.Refresh(btnDatas[i]);
                button.NoSelect();
                buttonList.Add(button);
                pageList.Add(pages[btnDatas[i]]);
            }
        }

        OpenFirstPage(firstPage);
    }

    private void OpenFirstPage(int firstPage)
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (buttonList[i] != null)
                buttonList[i].IsSelect = i == firstPage;

            if (i == firstPage)
            {
                currentButton = buttonList[i];
            }
        }

        if (currentButton == null)
            currentButton = buttonList[0];

        if (currentButton == null)
            return;

        SetPageActive(currentButton.BindIndex);

        currentPage = currentButton.BindPage;
        onPageChange?.Invoke(currentButton, currentButton.BindPage);
    }

    private void OnButtonClick(TButton buttonItem)
    {
        if (onTryChange != null && !onTryChange.Invoke(buttonItem, buttonItem.BindPage))
        {
            return;
        }

        OnOpenPage(buttonItem);
    }

    public void OnOpenPage(TButton buttonItem)
    {
        SetPageActive(buttonItem.BindIndex);

        if (currentButton != null)
            currentButton.NoSelect();

        currentButton = buttonItem;
        currentButton?.InSelect();
        currentPage = buttonItem.BindPage;

        onPageChange?.Invoke(buttonItem, buttonItem.BindPage);
    }

    private void SetPageActive(int index)
    {
        for (int i = 0; i < pageList.Count; i++)
        {
            pageList[i].IsActive = false;
        }

        pageList[index].IsActive = true;
    }

    public void OnOpenIndex(int index)
    {
        if (buttonList == null)
        {
            Debug.LogError($"错误, 未初始化按钮数据...");
            return;
        }

        if (index < 0 || index >= buttonList.Count)
        {
            Debug.LogError($"错误, 不可以超出可用按钮范围");
            return;
        }

        TButton button = buttonList[index];
        OnOpenPage(button);
    }

    public void OnOpenPage(Predicate<TButton> match)
    {
        if (buttonList == null)
        {
            Debug.LogError($"错误, 未初始化按钮数据...");
            return;
        }

        TButton button = buttonList.Find(match);
        if (button == null)
        {
            Debug.LogError($"错误, 找不到条件按钮...");
            return;
        }

        OnOpenPage(button);
    }

    public TButton FindButton(Predicate<TButton> match)
    {
        return buttonList.Find(match);
    }

    public void ActionButton()
    {
        foreach (var item in buttonList)
        {
            item?.onValueChanged();
        }
    }

    public T FindPgae<T>(Predicate<TButton> match) where T : APageBase
    {
        var button = buttonList.Find(match);
        if (button == null) return null;
        T page = button.BindPage as T;
        return page;
    }

    public void Clear()
    {
        if (currentPage != null)
            currentPage.IsActive = false;
        currentPage = null;
    }
}
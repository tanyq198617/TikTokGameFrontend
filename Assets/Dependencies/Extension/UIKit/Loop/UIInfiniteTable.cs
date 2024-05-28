using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 无限循环 table 
/// 注意：自身锚点和子物体锚点 均为 Top - center
/// </summary>
public class UIInfiniteTable : MonoBehaviour
{
    //刷新对应item 数据
    public delegate void OnReposition(GameObject go, int dataIndex, int childIndex);

    public OnReposition onRefreshItem;

    //获取 item 挂载的脚本
    public Action<int, GameObject> onCreate;

    //显示物体
    public Action<int, GameObject> onShow;

    //回收物体
    public Action<int, GameObject> onHide;

    //所有item 创建结束
    public Action onLoadEnd;

    //布局 -- 横向 / 纵向
    public enum Layout { Horizontal, Vertical }

    //布局类型
    public Layout mLayout = Layout.Horizontal;

    //每行或每列的 Item 数量
    [Range(1, 20)]
    public int mColumns = 1;

    //Item 的宽高
    public Vector2 mItemSize = new Vector2();

    //Item 之间间距
    public Vector2 mSpcaing = new Vector2();

    //顶部空隙
    public float mTopSpcaing = 0;

    //补缺长度      计算不精确时用
    public float mAddLength = 0;

    //Item 预设名字
    //public string mItemPrefabName;
    public GameObject mItemPrefab;

    //是否展示加载动画 隔帧加载
    public bool mIsAnimation = true;

    private ScrollRect mScrollRect;
    private RectTransform mScrollTrans;
    private RectTransform mContentTrans;
    //当前 ScrollRect 下 最大显示的行或列数，比可显示行数 +1
    private int mMaxLineCount = 1;
    //当前刷新第几行或列 的 index
    private int mCurLineIndex = -1;
    //当前 - 数据 
    private int mTableDataCount = 0;

    //当前用到的 Item list
    private List<UIInfiniteTableItem> mItemList = new List<UIInfiniteTableItem>();
    //未用到的 Item 等用到的时候再取
    private Queue<UIInfiniteTableItem> mUnusedItemQueue = new Queue<UIInfiniteTableItem>();

    private bool mIsInitEnd = false;
    public int TableDataCount
    {
        get { return mTableDataCount; }
        set
        {
            // if (mTableDataCount == value) return;
            mTableDataCount = value;
            
            //先刷新滑动内容 size
            UpdateScrollContentSize();
            
            if (!mIsInitEnd)
            {
                if (mIsAnimation)
                    OnDelayCreateItemAsync().Forget();
                else
                    OnDelayCreateItem();
            }
            else
                RefreshData();
        }
    }

    public ScrollRect ScrollRect => mScrollRect;

    #region 初始化
    private void Awake()
    {
        //滑动控件
        mScrollTrans = transform.parent as RectTransform;
        mScrollRect = mScrollTrans.gameObject.GetComponent<ScrollRect>();
        mContentTrans = transform as RectTransform;
        mItemPrefab.SetActive(false);

        Init();
    }

    public void UpdateAnchor() 
    {
        //设置ScrollContent锚点
        InitScrollContentPivot();
        //初始化最大列或行 数
        InitMaxLineCount();
    }

    private void Init()
    {
        if (mScrollRect != null)
            mScrollRect.onValueChanged.AddListener(OnValueChanged);

        //设置ScrollContent锚点
        InitScrollContentPivot();

        //初始化最大列或行 数
        InitMaxLineCount();
    }

    /// <summary>
    /// 设置滑动区域锚点
    /// </summary>
    private void InitScrollContentPivot()
    {
        if (mLayout == Layout.Horizontal)
        {
            mContentTrans.pivot = new Vector2(0f, 0.5f);
            mContentTrans.anchoredPosition = Vector2.zero;
        }
        else if (mLayout == Layout.Vertical)
        {
            mContentTrans.pivot = new Vector2(0.5f, 1f);
            mContentTrans.anchoredPosition = Vector2.zero;
        }
    }

    /// <summary>
    /// 在 Scroll 下 最大显示的行或列数，比可显示行数 +1
    /// </summary>
    private void InitMaxLineCount()
    {
        if (mLayout == Layout.Horizontal)
        {
            mMaxLineCount = Mathf.CeilToInt(mScrollTrans.sizeDelta.x / mItemSize.x) + 1;
        }
        else if (mLayout == Layout.Vertical)
        {
            mMaxLineCount = Mathf.CeilToInt(mScrollTrans.sizeDelta.y / mItemSize.y) + 1;
        }
    }
    #endregion

    #region 创建 Item
    private async UniTask OnDelayCreateItemAsync()
    {
        if (gameObject == null)
            return;
        mCurLineIndex = 0;
        //显示满足条件的item
        for (int i = mCurLineIndex * mColumns; i < (mCurLineIndex + mMaxLineCount) * mColumns; i++)
        {
            if (i < 0) continue;
            //大于数据量 -- 不足以显示
            if (i > mTableDataCount - 1) continue;
            CreateItemByIndex(i);
            await UniTask.Yield();
        }
        mIsInitEnd = true;
        if (onLoadEnd != null)
            onLoadEnd(); 
    }

    private void OnDelayCreateItem()
    {
        if (gameObject == null)
            return;
        mIsInitEnd = true;
        Refresh();
        if (onLoadEnd != null)
            onLoadEnd();
    }

    /// <summary>
    /// 根据数据index 创建item
    /// </summary>
    private int childCount = 0;
    private void CreateItemByIndex(int dataIndex)
    {
        UIInfiniteTableItem item;
        if (mUnusedItemQueue.Count > 0)
        {
            item = mUnusedItemQueue.Dequeue();
        }
        else
        {
            item = new UIInfiniteTableItem();

            GameObject itemPrefab = mItemPrefab;

            if (itemPrefab != null)
            {
                GameObject obj = AddChild(transform, itemPrefab);
                item.mObj = obj;
            }

            item.mChildIndex = childCount;
            childCount++;
            //返回 item 挂的脚本回调
            if (onCreate != null)
                onCreate(item.mChildIndex, item.mObj);
        }
        item.mDataIndex = dataIndex;
        item.OnShow(GetItemPosByIndex(dataIndex));
        mItemList.Add(item);
        //刷洗 item 数据
        if (onRefreshItem != null)
            onRefreshItem(item.mObj, dataIndex, item.mChildIndex);
    }

    #endregion

    #region 刷新 item 
    //
    private void OnValueChanged(Vector2 pos)
    {
        if (mIsInitEnd)
            Refresh();
    }
    //刷新
    private void Refresh()
    {
        int index = GetIndexByPanelPos();
        //满足刷新条件
        if (mCurLineIndex != index && index > -1)
        {
            mCurLineIndex = index;
            //移除不再显示范围内的 item
            for (int i = mItemList.Count; i > 0; i--)
            {
                UIInfiniteTableItem item = mItemList[i - 1];
                if (item.mDataIndex < index * mColumns
                    || (item.mDataIndex >= (index + mMaxLineCount) * mColumns)
                    || item.mDataIndex > mTableDataCount - 1)
                {
                    //回收事件
                    if (onHide != null)
                        onHide(item.mChildIndex, item.mObj);

                    item.OnHide();
                    //移除
                    mItemList.Remove(item);
                    //添加到队列
                    mUnusedItemQueue.Enqueue(item);
                }
            }

            //显示满足条件的 item

            for (int i = mCurLineIndex * mColumns; i < (mCurLineIndex + mMaxLineCount) * mColumns; i++)
            {
                if (i < 0) continue;
                //大于数据量 -- 不足以显示
                if (i > mTableDataCount - 1) continue;
                bool isOk = false;
                foreach (UIInfiniteTableItem item in mItemList)
                    if (item.mDataIndex == i) isOk = true;
                if (isOk) continue;
                CreateItemByIndex(i);
            }
        }
    }
    /// <summary>
    /// 根据 mContent 位置 获取当前列或行的 index
    /// </summary>
    private int GetIndexByPanelPos()
    {
        if (mLayout == Layout.Horizontal)
        {
            float posX = mContentTrans.anchoredPosition.x;
            if (posX > 0f)
                posX = 0;

            posX = Mathf.Abs(posX) - mTopSpcaing;
            if (posX < 0) posX = 0;
            return Mathf.FloorToInt(posX / (mItemSize.x + mSpcaing.x));
        }
        else if (mLayout == Layout.Vertical)
        {
            float posY = mContentTrans.anchoredPosition.y;
            if (posY < 0f)
                posY = 0;
            return Mathf.FloorToInt(posY / (mItemSize.y + mSpcaing.y));
        }
        return 0;
    }

    /// <summary>
    /// 根据 item index 获取所在的位置
    /// </summary>
    Vector3 m_Pos = new Vector3();
    Vector3 GetItemPosByIndex(int dataIndex)
    {
        m_Pos.Set(0, 0, 0);
        if (mLayout == Layout.Horizontal)
        {
            m_Pos.x = (mItemSize.x + mSpcaing.x) * (dataIndex / mColumns) + mItemSize.x / 2 + mTopSpcaing;

            float tableSize_Y = (mItemSize.y + mSpcaing.y) * (mColumns - 1) * 0.5f;
            m_Pos.y = tableSize_Y - (mItemSize.y + mSpcaing.y) * (dataIndex % mColumns);
        }
        else if (mLayout == Layout.Vertical)
        {
            float tableSize_X = -(mItemSize.x + mSpcaing.x) * (mColumns - 1) * 0.5f;
            m_Pos.x = tableSize_X + (mItemSize.x + mSpcaing.x) * (dataIndex % mColumns);
            m_Pos.y -= (mItemSize.y + mSpcaing.y) * (dataIndex / mColumns) + mItemSize.y / 2 + mTopSpcaing;
        }
        return m_Pos;
    }
    /// <summary>
    /// 刷新可滑动区域的大小
    /// </summary>
    private void UpdateScrollContentSize()
    {
        int lineCount = Mathf.CeilToInt((float)mTableDataCount / mColumns);
        Vector2 size = new Vector2();
        if (mLayout == Layout.Horizontal)
        {
            size.x = Mathf.CeilToInt(mItemSize.x * lineCount + mSpcaing.x * (lineCount - 1)) + mTopSpcaing + mAddLength;
            size.y = mContentTrans.sizeDelta.y;
        }
        else if (mLayout == Layout.Vertical)
        {
            size.x = mContentTrans.sizeDelta.x;
            size.y = Mathf.CeilToInt(mItemSize.y * lineCount + mSpcaing.y * (lineCount - 1)) + mTopSpcaing + mAddLength;
        }
        mContentTrans.sizeDelta = size;
    }
    #endregion

    #region 外部调用
    /// <summary>
    /// 刷新数据
    /// </summary>
    public void RefreshData()
    {
        if (mIsInitEnd)
        {
            mCurLineIndex = -1;
            for (int i = 0, count = mItemList.Count; i < count; i++)
            {
                mItemList[i].mDataIndex = -1;
            }
            Refresh();
            onLoadEnd?.Invoke();
        }
    }
    /// <summary>
    /// 根据数据index 获取对应的item ChildIndex
    /// </summary>
    public int GetItemChildIndexByIndex(int index)
    {
        for (int i = 0; i < mItemList.Count; i++)
        {
            UIInfiniteTableItem item = mItemList[i];
            if (item.mDataIndex == index)
                return item.mChildIndex;
        }
        return -1;
    }

    public void OnResetPosition()
    {
        InitScrollContentPivot();
    }

    public void MoveTo(int index)
    {
        Vector3 pos = GetItemPosByIndex(index);

        if (mLayout == Layout.Horizontal)
        {
            mContentTrans.pivot = new Vector2(0f, 0.5f);
            float v = mScrollTrans.sizeDelta.x * 0.5f + (pos.x + mItemSize.x / 2f + mSpcaing.x);
            float mv = mScrollTrans.sizeDelta.x - mContentTrans.sizeDelta.x;
            if (v < mv) v = mv;
            if (v > 0) v = 0;
            mContentTrans.anchoredPosition = new Vector2(v, 0);
        }
        else if (mLayout == Layout.Vertical)
        {
            mContentTrans.pivot = new Vector2(0.5f, 1f);
            float p = mContentTrans.pivot.y - mScrollTrans.pivot.y; //默认0.5
            float v = mScrollTrans.sizeDelta.y * p - (pos.y + mItemSize.y / 2f + mSpcaing.y);
            float mv = (mContentTrans.sizeDelta.y - mScrollTrans.sizeDelta.y);
            if (v > mv) v = mv;
            if (v < 0) v = 0;
            mContentTrans.anchoredPosition = new Vector2(0, v);
        }
        RefreshData();
    }
    #endregion

    #region 编辑器
    [HideInInspector] public int mTempDataCount = 0;
    public void Reposition()
    {
        mScrollRect?.StopMovement(); 
        OnResetPosition();
        TableDataCount = mTempDataCount;
        // RefreshData();
    }
    #endregion

    public GameObject AddChild(Transform parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent);
            t.localScale = Vector3.one;
            go.layer = parent.gameObject.layer;
            go.SetActive(true);
        }
        return go;
    }
}
/// <summary>
/// 
/// </summary>
public class UIInfiniteTableItem
{
    public GameObject mObj;

    //孩子索引
    public int mChildIndex;

    //数据索引
    public int mDataIndex;

    public void OnShow(Vector3 pos)
    {
        mObj.transform.localPosition = pos;
    }
    public void OnHide()
    {
        mObj.transform.localPosition = Vector3.one * 10000;
    }
}
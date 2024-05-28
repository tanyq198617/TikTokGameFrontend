using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AWorldRankItem : ALayoutItem
{
    private TextMeshProUGUI tx_rank;
    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_rank2;
    private TextMeshProUGUI tx_killNum;
    private TextMeshProUGUI tx_winningStreak;

    private RawImage playerIcon;
    private Image image_rank;
    private RankInfo info;

    private PlayerHeadItem headItem;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);

        tx_rank = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_rank");
        tx_name = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_name");
        tx_rank2 = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_rank2");
        tx_killNum = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_killNum");
        tx_winningStreak = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winningStreak");
        playerIcon = UIUtility.GetComponent<RawImage>(RectTrans, "image_PlayerIcon");
        image_rank = UIUtility.GetComponent<Image>(RectTrans, "rankImage");
        headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(RectTrans,"uiitem_playerHeadObject");
    }

    public override void Refresh<T>(T data)
    {
        this.info = data as RankInfo;
        if (info == null)
            return;

        if (info.info == null)
            return;
        
        UIUtility.Safe_UGUI(ref tx_name, info.info.nickname);
        UIUtility.Safe_UGUI(ref tx_killNum, $"积分:{long.Parse(info.score).Number4Chinese()}");
        UIUtility.Safe_UGUI(ref tx_rank2, $"{info.rank}名");
        UIUtility.Safe_UGUI(ref tx_winningStreak, $"{info.info.win_combo.Number4Chinese()}连胜");
        
        headItem.LoadHeadTexture(info.info.avatar_url).Forget();
        if (info.rank > 3)
        {
            tx_rank.SetActiveEX(true);
            image_rank.SetActiveEX(false);
            UIUtility.Safe_UGUI(ref tx_rank, info.rank);
        }
        else
        {
            tx_rank.SetActiveEX(false);
            image_rank.SetActiveEX(true);
            string spriteName = $"rank_image_{info.rank}";
            UIUtility.Safe_UGUI(ref image_rank,SpriteMgr.Instance.LoadSpriteFromRankView(spriteName));
            image_rank.SetNativeSize();
        }
    }

    public override void Clear()
    {
        base.Clear();
        this.info = null;
    }

}
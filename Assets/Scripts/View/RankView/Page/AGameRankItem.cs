using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class AGameRankItem : ALayoutItem
{
    private TextMeshProUGUI tx_winningStreak;
    private TextMeshProUGUI tx_wonComboChange;
    private TextMeshProUGUI tx_name;
    private TextMeshProUGUI tx_rank1;
    private TextMeshProUGUI tx_rank2;
    private TextMeshProUGUI tx_killNum;
    private TextMeshProUGUI tx_getScore;
    private TextMeshProUGUI tx_rank;
    private Image rankImage;

    private GameObject object_up;
    private GameObject object_dn;
    private GameObject object_redbj;
    private GameObject object_blueBj;
    private GameObject object_add;
    private GameObject object_jian;

    private PlayerHeadItem headItem;
    public override void setObj(GameObject obj)
    {
      base.setObj(obj);
      tx_winningStreak = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_winningStreak");
   
      tx_wonComboChange = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_wonComboChange");
      tx_name = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_name");
      tx_rank1 = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_rank1");
      tx_rank2 = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_rank2");
      tx_killNum = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_killNum");
      tx_getScore = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_getScore");
      tx_rank = UIUtility.GetComponent<TextMeshProUGUI>(RectTrans, "tx_rank");
      
      rankImage = UIUtility.GetComponent<Image>(RectTrans, "rankImage");

      object_up = UIUtility.Control("object_up", m_gameobj);
      object_dn = UIUtility.Control("object_dn", m_gameobj);
      object_redbj = UIUtility.Control("object_redbj", m_gameobj);
      object_blueBj = UIUtility.Control("object_blueBj", m_gameobj);
      object_add = UIUtility.Control("object_add", m_gameobj);
      object_jian = UIUtility.Control("object_jian", m_gameobj);
      headItem = UIUtility.CreateItemNoClone<PlayerHeadItem>(RectTrans,"uiitem_playerHeadObject");
    }
    public override void Refresh<T>(T data)
    {
        RankInfo info = data as RankInfo;
        if (info == null)
            return;
        double aas = 9999.99;
        headItem.LoadHeadTexture(info.info.avatar_url).Forget();
        string rank1Str = info.info.rank_begin > 0 ? $"{info.info.rank_begin}名" : "";
        UIUtility.Safe_UGUI(ref tx_rank1,rank1Str);
        UIUtility.Safe_UGUI(ref tx_rank2, $"{info.info.rank_end}名");
        
        UIUtility.Safe_UGUI(ref tx_name,info.info.nickname);

        UIUtility.Safe_UGUI(ref tx_killNum, $"积分:{info.world_score.Number4Chinese()}<color=#00b6c6> + {long.Parse(info.score).Number4Chinese()}</color>");
        
        UIUtility.Safe_UGUI(ref tx_winningStreak,$"{info.info.win_combo.Number4Chinese()}连胜");
        UIUtility.Safe_UGUI(ref tx_getScore,$"【瓜分积分池:{info.player.poolScore.Number4Chinese()}】");
        
        bool isUpAndDn = info.info.rank_end < info.info.rank_begin;
        object_up.SetActiveEX(isUpAndDn);
        object_dn.SetActiveEX(!isUpAndDn);
    
        long wonComboChangeChange = info.info.win_combo - info.player.init_win_combo;
        bool isAddChange = wonComboChangeChange >= 0;
        string wonComBoChangeStr = isAddChange ? "+" : "";
        UIUtility.Safe_UGUI(ref tx_wonComboChange,$"{wonComBoChangeStr}{wonComboChangeChange.ToMoney()}连胜");
        object_add.SetActiveEX(isAddChange);
        object_jian.SetActiveEX(!isAddChange);
        
        bool isBlueCamp = info.player.campType == CampType.蓝;
        object_redbj.SetActiveEX(!isBlueCamp);
        object_blueBj.SetActiveEX(isBlueCamp);
        if (info.rank > 3)
        {
            tx_rank.gameObject.SetActiveEX(true);
            rankImage.gameObject.SetActiveEX(false);
            UIUtility.Safe_UGUI(ref tx_rank,info.rank);
        }
        else
        {
            tx_rank.gameObject.SetActiveEX(false);
            rankImage.gameObject.SetActiveEX(true);
            string spriteName = $"rank_image_{info.rank}";
            UIUtility.Safe_UGUI(ref rankImage,SpriteMgr.Instance.LoadSpriteFromRankView(spriteName));
            rankImage.SetNativeSize();
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

public class BattleCenterPage : AItemPageBase
{

    private BattleCenterRichManPage richManPage;
    private BattleYoungerBrotherPage youngerBrotherPage;
    private BattleGiftIconPage giftIconPage;
    public override void setObj(GameObject obj)
    {
        base.setObj(obj);
        richManPage = UIUtility.CreatePageNoClone<BattleCenterRichManPage>(Trans, "RichManPage"); 
        youngerBrotherPage = UIUtility.CreatePageNoClone<BattleYoungerBrotherPage>(Trans, "YoungerBrotherPage");
        giftIconPage = UIUtility.CreatePageNoClone<BattleGiftIconPage>(Trans, "giftIconPage");
    }

    public override void Refresh()
    {
        base.Refresh();
        richManPage.IsActive = true;
        youngerBrotherPage.IsActive = true;
        giftIconPage.IsActive = true;
    }

    public override void Close()
    {
        base.Close();
        richManPage.IsActive = false;
        youngerBrotherPage.IsActive = false;
        giftIconPage.IsActive = false;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.Wood;
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { (int)Utility.ItemId.Ono, (int)Utility.ItemId.Match }, new string[] { "斧を使用　切り倒す", "マッチを使用　燃やす" }, new string[] { "斧が必要", "マッチが必要" });
    }

    public override IEnumerator Use(int itemNum)
    {
        Item item = itemTask.items[itemNum];
        switch (itemNum)
        {
            case (int)Utility.ItemId.Ono:
                item.num--;
                item.UseUpdate();

                gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.WoodBlock, (int)Utility.ObjectId.Wood, GameTask.CreateData.upCreate);
                break;

            case (int)Utility.ItemId.Match:
                item.num--;
                item.UseUpdate();

                gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Fire, (int)Utility.ObjectId.Wood, GameTask.CreateData.upCreate);
                break;
        }

        yield break;
    }
}

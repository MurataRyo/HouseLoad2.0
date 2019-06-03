using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { (int)Utility.ItemId.Baketu }, new string[] { "水を使用　炎を消す" }, new string[] { "水入りバケツが必要" });
    }

    public override IEnumerator Use(int itemNum)
    {
        Item item = itemTask.items[(int)Utility.ItemId.Baketu];
        item.num--;
        item.TextUpdate();

        gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Ground);

        yield break;
    }

    public override bool UseIf(int itemNum)
    {
        if (itemTask.items[(int)Utility.ItemId.Baketu].num != 2)
            return false;

        return true;
    }
}

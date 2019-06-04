using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.Water;
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] {(int)Utility.ItemId.Baketu }, new string[] { "水を汲む" }, new string[] { "空バケツが必要" });
    }

    public override IEnumerator Use(int itemNum)
    {
        Item item = itemTask.items[itemNum];
        item.num++;
        item.UseUpdate();

        gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Ground, (int)Utility.ObjectId.Water, GameTask.CreateData.groundCreate);

        yield break;
    }

    public override bool UseIf(int itemNum)
    {
        Item item = itemTask.items[itemNum];

        //空のバケツのとき
        if (item.num == 1)
            return true;

        return false;
    }
}

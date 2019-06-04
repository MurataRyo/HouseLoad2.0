using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.Stone;
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] {(int)Utility.ItemId.Hammer }, new string[] { "ハンマーを使用　壊す" }, new string[] { "ハンマーが必要" });
    }

    public override IEnumerator Use(int itemNum)
    {
        Item item = itemTask.items[(int)Utility.ItemId.Hammer];
        item.num--;
        item.UseUpdate();

        gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Ground, (int)Utility.ObjectId.Stone, GameTask.CreateData.noCreate);

        yield break;
    }
}

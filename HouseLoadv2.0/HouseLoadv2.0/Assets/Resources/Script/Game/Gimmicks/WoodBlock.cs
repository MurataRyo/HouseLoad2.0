using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlock : Block
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.WoodBlock;
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { -1, (int)Utility.ItemId.Match }, new string[] { "前に押す", "マッチを使用　燃やす" }, new string[] { "押せなさそうだ", "マッチが必要" });
    }

    public override IEnumerator Use(int itemNum)
    {
        Vector3Int pos = Utility.PositionToData(transform.position);
        switch (itemNum)
        {
            case -1:
                MoveBlock(pos);
                break;

            case (int)Utility.ItemId.Match:
                Item item = itemTask.items[itemNum];
                item.num--;
                item.UseUpdate();

                gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Fire, (int)Utility.ObjectId.WoodBlock, GameTask.CreateData.upCreate);
                break;
        }

        yield break;
    }




}

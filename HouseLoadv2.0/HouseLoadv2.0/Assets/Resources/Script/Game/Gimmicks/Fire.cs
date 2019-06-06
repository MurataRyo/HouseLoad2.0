using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.Fire;
        IceMelt();
    }

    public void IceMelt()
    {
        Gimmick[] gimmicks = gameTask.GetGimmcks(new Utility.ObjectId[] { Utility.ObjectId.IceBlock });
        foreach(Gimmick gimmick in gimmicks)
        {
            IceBlock ice = gimmick.gameObject.GetComponent<IceBlock>();
            if(ice.MeltMoveIf(Utility.PositionToData(transform.position), Utility.PositionToData(ice.transform.position)))
            {
                gameTask.moveObjectTask.MoveObject(ice.MeltMove(ice.gameObject.transform.position),true);
            }
        }
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { (int)Utility.ItemId.Baketu }, new string[] { "水を使用　炎を消す" }, new string[] { "水入りバケツが必要" });
    }

    public override IEnumerator Use(int itemNum)
    {
        Item item = itemTask.items[(int)Utility.ItemId.Baketu];
        item.num--;
        item.UseUpdate();

        gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Ground, (int)Utility.ObjectId.Fire, GameTask.CreateData.noCreate);

        yield break;
    }

    public override bool UseIf(int itemNum)
    {
        //水入りバケツでなければならない
        if (itemTask.items[(int)Utility.ItemId.Baketu].num != 2)
            return false;

        return true;
    }
}

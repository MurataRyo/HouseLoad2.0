using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodBlock : Gimmick
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
                Vector3Int fowardPos = Utility.PositionToData(transform.position) - gameTask.playerTask.pos + pos;

                //地面が穴の場合
                if (gameTask.stageData[fowardPos.x][fowardPos.y][fowardPos.z] == (int)Utility.MapId.Hole)
                {
                    gameTask.MoveObject(pos, fowardPos, (int)Utility.MapId.WoodBlock,(int)Utility.MapId.Ground, (int)Utility.MapId.Hole);

                    gameTask.moveObjectTask.MoveObject(FallMove(), true);
                    gameTask.moveObjectTask.MoveObject(Move(gameTask.playerTask.gameObject), true);
                    break;
                }


                gameTask.MoveObject(pos, fowardPos, (int)Utility.MapId.WoodBlock);

                //移動させる
                gameTask.moveObjectTask.MoveObject(Move(gameObject), true);
                gameTask.moveObjectTask.MoveObject(Move(gameTask.playerTask.gameObject), true);

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

    public MoveObject[] FallMove()
    {
        MoveObject[] moveObjects = new MoveObject[2];
        moveObjects[0] = Move(gameObject);
        Vector3 foward = transform.position - Utility.DataToPosition(gameTask.playerTask.pos);
        Vector3[] movePoss = new Vector3[] { transform.position + foward, transform.position + foward + Vector3.down };
        moveObjects[1] = new MoveObject(gameObject, movePoss, 5f);

        return moveObjects;
    }

    //押す動作
    public MoveObject Move(GameObject go)
    {
        Vector3 foward = transform.position - Utility.DataToPosition(gameTask.playerTask.pos);
        Vector3[] movePoss = new Vector3[] { go.transform.position, go.transform.position + foward };
        return new MoveObject(go, movePoss, 3f);
    }

    public override bool UseIf(int itemNum)
    {
        switch (itemNum)
        {
            case -1:
                Vector3Int movePos = Utility.PositionToData(transform.position) * 2 - gameTask.playerTask.pos;

                if (!gameTask.InIfStageData(movePos))
                    return false;

                //先のマスが穴か地面なら動かせる
                return gameTask.stageData[movePos.x][movePos.y][movePos.z] == (int)Utility.MapId.Ground ||
                gameTask.stageData[movePos.x][movePos.y][movePos.z] == (int)Utility.MapId.Hole;
        }
        return base.UseIf(itemNum);
    }
}

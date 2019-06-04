using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hole : Gimmick
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.Hole;
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { -1 }, new string[] { "飛び降りる" }, new string[] { "飛び降りれなさそうだ" });
    }

    public override IEnumerator Use(int itemNum)
    {
        gameTask.moveObjectTask.MoveObject(FallMove(),true);
        yield break;
    }

    public MoveObject[] FallMove()
    {
        MoveObject[] moveObjects = new MoveObject[2];

        Vector3 start = gameTask.playerTask.transform.position;
        Vector3 end = transform.position;
        Vector3 center = (start + end) / 2 + Vector3.up * 2f;

        BesieData bData = new BesieData(start, end, center);
        Vector3[] poss = bData.Positions(20);

        GameObject player = gameTask.playerTask.gameObject;
        moveObjects[0] = new MoveObject(player, poss, 4f);

        Vector3 fallPoint = end - Vector3.up * StageCreateTask.Y_Scale;
        moveObjects[1] = new MoveObject(player, new Vector3[] { end, fallPoint }, 5f);

        return moveObjects;
    }

    public override bool UseIf(int itemNum)
    {
        ObjectChoice objectChoice = gameTask.playerTask.objectChoice;
        Vector3Int pos = objectChoice.choiceObjects[objectChoice.choiceNum].pos;

        if (pos.x == 0)
            return false;

        //飛び降りれない場所なら
        int mapId = Array.IndexOf(gameTask.Special.FallObject, gameTask.stageData[pos.x - 1][pos.y][pos.z]);
        return mapId != -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : Block
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        MapId = (int)Utility.MapId.IceBlock;
    }

    public override void UseSet()
    {
        useBase = new UseBase(new int[] { -1 }, new string[] { "前に押す" }, new string[] { "押せなさそうだ" });
    }

    public override IEnumerator Use(int itemNum)
    {
        Vector3Int pos = Utility.PositionToData(transform.position);
        switch (itemNum)
        {
            case -1:
                MoveBlock(pos);
                break;
        }
        yield break;
    }

    public MoveObject MeltMove(Vector3 pos)
    {
        MoveObject mobj = new MoveObject(gameObject, new Vector3[] { pos, pos + Vector3.down }, 1f);
        MoveObject mobj2 = new MoveObject(gameObject, new Vector3[] { pos, pos - Vector3.down }, 0.1f);
        mobj2.ie = Melt();
        mobj.endEvent = mobj2;

        return mobj;
    }

    private IEnumerator Melt()
    {
        Debug.Log(Utility.PositionToData(transform.position));
        gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Water, (int)Utility.ObjectId.IceBlock, GameTask.CreateData.groundCreate);
        gameTask.DeleteObject(Utility.PositionToData(transform.position), (int)Utility.MapId.Water, (int)Utility.ObjectId.Ground, GameTask.CreateData.noCreate);
        yield break;
    }

    public override MoveObject Move(GameObject go)
    {
        if (go != gameObject)
            return base.Move(go);

        Vector3 foward = transform.position - Utility.DataToPosition(gameTask.playerTask.pos);
        Vector3[] movePoss = new Vector3[] { go.transform.position, go.transform.position + foward };
        MoveObject moveObject = new MoveObject(go, movePoss, 3f);

        Gimmick[] fire = gameTask.GetGimmcks(new Utility.ObjectId[] { Utility.ObjectId.Fire });
        Vector3Int[] firePoss = new Vector3Int[fire.Length];
        for (int i = 0; i < firePoss.Length; i++)
        {
            firePoss[i] = Utility.PositionToData(fire[i].transform.position);
        }

        if (MeltMoveIf(firePoss,Utility.PositionToData(go.transform.position + foward)))
        {
            moveObject.endEvent = MeltMove(go.transform.position + foward);
        }

        return moveObject;
    }

    //氷が溶けるかどうかのフラグ
    public bool MeltMoveIf(Vector3Int FirePos, Vector3Int thisPos)
    {
        Vector3Int[] checkPoss = new Vector3Int[4];
        checkPoss[0] = thisPos + Vector3Int.up;
        checkPoss[1] = thisPos + Vector3Int.down;
        checkPoss[2] = thisPos + new Vector3Int(0, 0, 1);
        checkPoss[3] = thisPos + new Vector3Int(0, 0, -1);

        foreach (Vector3Int checkPos in checkPoss)
        {
            if (checkPos == FirePos)
            {
                return true;
            }
        }
        return false;
    }

    public bool MeltMoveIf(Vector3Int[] FirePoss, Vector3Int thisPos)
    {
        foreach (Vector3Int firePos in FirePoss)
        {
            if (MeltMoveIf(firePos, thisPos))
                return true;
        }
        return false;
    }
}

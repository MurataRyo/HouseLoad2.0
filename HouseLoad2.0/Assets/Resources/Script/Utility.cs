using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utility : MonoBehaviour
{
    #region　ID一覧

    public enum ItemId
    {
        Ono,
        Hammer,
        Match,
        Baketu
    }

    public enum ExItemId
    {
        WaterBaketu,
    }


    //オブジェクトID[配列番号とのリンク]
    public enum ObjectId
    {
        Player,
        Ground,
        Wood,
        WoodBlock,
        Stone,
        Wall,
        House,
        Water,
        Fire,
        Warp
    }

    public enum MapId
    {
        None,
        Hole,
        Player,
        Ground,
        Wood,
        WoodBlock,
        Stone,
        Wall,
        House,
        Water,
        Fire,
        Warp
    }
    #endregion

    public static MapId GetMapId(ObjectId objectId)
    {
        for (int i = 0; i < Enum.GetValues(typeof(MapId)).Length; i++)
        {
            if (objectId.ToString() == Enum.GetName(typeof(MapId), i))
            {
                return (MapId)i;
            }
        }

        Debug.Log("指定されたマップIDが存在しません");
        return 0;
    }

    public static ObjectId GetObjectId(MapId mapId)
    {
        for (int i = 0; i < Enum.GetValues(typeof(ObjectId)).Length; i++)
        {
            if (mapId.ToString() == Enum.GetName(typeof(ObjectId), i))
            {
                return (ObjectId)i;
            }
        }

        Debug.Log("指定されたオブジェクトが存在しません");
        return 0;
    }
}

public class GetPath
{
    public static string Prefab = "Prefab";
    public static string StageObject = Prefab + "/StageObject";

}

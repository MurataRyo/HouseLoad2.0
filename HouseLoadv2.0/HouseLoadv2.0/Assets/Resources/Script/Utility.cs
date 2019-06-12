using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utility : MonoBehaviour
{
    public const float GameSizeX = 1920f;
    public const float GameSizeY = 1080f;

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
        WaterBaketu = ItemId.Baketu,
    }


    //オブジェクトID[配列番号とのリンク]
    public enum ObjectId
    {
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
        Warp,
        IceBlock
    }

    public enum MapId
    {
        Hole = 0,
        Player = 1,
        Ground = 2,
        Wood = 3,
        WoodBlock = 4,
        Stone = 5,
        Wall = 6,
        House = 7,
        Water = 8,
        Fire = 9,
        Warp = 10,
        IceBlock = 11
    }
    #endregion

    public static GameObject GetCanvas()
    {
        return GameObject.FindGameObjectWithTag("Canvas");
    }

    public static GameObject GetTaskObject()
    {
        return GameObject.FindGameObjectWithTag("Task");
    }

    public static GameTask GetGameTask()
    {
        return GetTaskObject().GetComponent<GameTask>();
    }

    public static int BoolToInt(bool flag)
    {
        return flag ? 1 : -1;
    }

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

    //現在のデータ(int)を座標(float)へ変換する
    public static Vector3 DataToPosition(Vector3Int pos)
    {
        int y = pos.x * 4 + 1;

        int z = -pos.y;

        int x = pos.z;

        return new Vector3(x, y, z);
    }

    //現在の座標(float)をデータ(int)へ変換する
    public static Vector3Int PositionToData(Vector3 pos)
    {
        int y = Mathf.RoundToInt(pos.y);
        y /= StageCreateTask.Y_Scale;

        int z = -Mathf.RoundToInt(pos.z);

        int x = Mathf.RoundToInt(pos.x);

        return new Vector3Int(y, z, x);
    }

    //現在の座標(float)をデータ(int)へ変換する踏んでいるマス全て返す
    public static Vector3Int[] PositionToData(Vector3 pos, float radius)
    {
        int y = Mathf.RoundToInt(pos.y) - 1;
        y /= StageCreateTask.Y_Scale;

        int xMin = PositionToData(pos - new Vector3(radius, 0f, 0f)).z;
        int xMax = PositionToData(pos - new Vector3(-radius, 0f, 0f)).z;
        int zMin = PositionToData(pos - new Vector3(0f, 0f, -radius)).y;
        int zMax = PositionToData(pos - new Vector3(0f, 0f, radius)).y;

        List<Vector3Int> vec3s = new List<Vector3Int>();
        for (int i = xMin; i <= xMax; i++)
        {
            for (int j = zMin; j <= zMax; j++)
            {
                vec3s.Add(new Vector3Int(y, j, i));
            }
        }

        return vec3s.ToArray();
    }

    //ベシエ曲線
    public static Vector3 Besie(Vector3 start, Vector3 end, Vector3 center, float t)
    {
        return new Vector3(Besie(start.x, end.x, center.x, t),
                           Besie(start.y, end.y, center.y, t),
                           Besie(start.z, end.z, center.z, t));
    }

    public static float Besie(float start, float end, float center, float t)
    {
        return (1 - t) * (1 - t) * start + 2 * (1 - t) * t * center + t * t * end;
    }

    //選択の変更
    public static int ChoiceChange(int baseNum, int maxNum, bool flag)
    {
        baseNum += Utility.BoolToInt(flag);
        if (baseNum < 0 || baseNum >= maxNum)
        {
            if (baseNum < 0)
                baseNum = maxNum - 1;
            else
                baseNum = 0;
        }

        return baseNum;
    }
}

public class GetPath
{
    public const string Prefab = "Prefab";
    public const string StageObject = Prefab + "/StageObject";
    public const string Ui = Prefab + "/Ui";

    public const string Txt = "Txt";
    public const string MapData = Txt + "/MapData";
    public const string Tutorial = MapData + "/Tutorial";

    public const string Font = "Font";

    public const string Image = "Image";
    public const string Item = Image + "/Item";
    public const string ExItem = Item + "/ExItem";
}

public class MapObject
{
    public GameObject go;
    public int objectId;     //オブジェクト番号
    public Vector3Int pos;
    public MapObject(GameObject go, int objectId, Vector3Int pos)
    {
        this.go = go;
        this.objectId = objectId;
        this.pos = pos;
    }
}

public class WarpObject : MapObject
{
    public int warpNum;
    public WarpObject(GameObject gameObject, int objectId, Vector3Int pos, int wNum) : base(gameObject, objectId, pos)
    {
        warpNum = wNum;
    }
}

public struct BesieData
{
    public Vector3 start;
    public Vector3 end;
    public Vector3 center;

    public BesieData(Vector3 start, Vector3 end, Vector3 center)
    {
        this.start = start;
        this.end = end;
        this.center = center;
    }

    public Vector3[] Positions(int num)
    {
        Vector3[] pos = new Vector3[num + 1];
        for (int i = 0; i < num + 1; i++)
        {
            pos[i] = Position(i * (1f / num));
        }
        return pos;
    }

    public Vector3 Position(float pos)
    {
        return Utility.Besie(start, end, center, pos);
    }
}
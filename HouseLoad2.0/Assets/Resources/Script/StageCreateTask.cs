using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageCreateTask : MonoBehaviour
{
    public static GameObject[] objects;
    private const int Y_Scale = 5;
    private const int Z_Scale = 1;
    private const int X_Scale = 1;

    void Start()
    {
        MapDataToCreateStage(testStage);
    }

    public void MapDataToCreateStage(string str0)
    {
        //下にブロックを置くもの
        SpecialObject inUnderBlock = new SpecialObject();

        //str1→階層ごとのデータ
        string[] str1 = str0.Split(char.Parse("/"));
        for (int y = 0; y < str1.Length; y++)
        {
            //str2→横1列ごとのデータ
            string[] str2 = str1[y].Split(char.Parse(";"));

            for (int z = 0; z < str2.Length; z++)
            {
                //str3→1マスのデータ
                string[] str3 = str2[z].Split(char.Parse(":"));

                for (int x = 0; x < str3.Length; x++)
                {
                    string[] str4 = str3[x].Split(char.Parse("."));

                    int mapId = Convert.ToInt32(str4[0]);
                    int objectId = (int)Utility.GetObjectId((Utility.MapId)mapId);
                    int customId = Convert.ToInt32(str4[1]);

                    //地上に置くオブジェクトならtrue
                    if (Array.IndexOf(inUnderBlock.ThisUnder, mapId) == -1)
                    {
                        CreateObject((y * Y_Scale) + 1, -z * Z_Scale, x * X_Scale, objectId, customId);
                        //オブジェクトの下にブロックを置くものならtrue
                        if (Array.IndexOf(inUnderBlock.InUnder, mapId) != -1)
                            CreateObject(y * Y_Scale, -z * Z_Scale, x * X_Scale, (int)Utility.ObjectId.Ground, 0);
                    }
                    //地面に置くオブジェクト
                    else
                    {
                        CreateObject(y * Y_Scale, -z * Z_Scale, x * X_Scale, objectId, 0);
                    }
                }
            }
        }
    }

    public void CreateObject(int yPos, int zPos, int xPos, int objectId, int customId)
    {
        if (objects == null)
            objects = ObjectsLoad();

        GameObject obj = Instantiate(objects[objectId]);
        obj.transform.position = new Vector3Int(xPos, yPos, zPos);

        if (customId != 0)
            CustomObject(obj, objectId, customId);
    }

    #region　カスタムIDありのやつ
    private void CustomObject(GameObject obj, int objectId, int customId)
    {
        switch (objectId)
        {
            case (int)Utility.ObjectId.House:
                AngleCustom(obj, customId);
                break;

            case (int)Utility.ObjectId.Warp:
                WarpCustom(obj, customId);
                break;
        }
    }

    //0で前から時計回りに回転
    private void AngleCustom(GameObject obj,int customId)
    {
        obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y + customId * 90f, obj.transform.eulerAngles.z);
    }

    private void WarpCustom(GameObject obj, int customId)
    {

    }
    #endregion

    //オブジェクトの保存
    private GameObject[] ObjectsLoad()
    {
        int objectNum = Enum.GetValues(typeof(Utility.ObjectId)).Length;
        GameObject[] loadObjects = new GameObject[objectNum];
        for (int i = 0; i < objectNum; i++)
        {
            string objectName = Enum.GetName(typeof(Utility.ObjectId), i);
            loadObjects[i] = Resources.Load<GameObject>(GetPath.StageObject + "/" + objectName);
        }

        return loadObjects;
    }

    public const string testStage =
        "3.0:2.0:4.0:3.0:8.2:3.0:4.0:8.0:4.0:3.0:4.0:3.0;" +
        "3.0:3.0:3.0:3.0:3.0:3.0:4.0:4.0:4.0:3.0:3.0:3.0;" +
        "3.0:4.0:3.0:4.0:3.0:3.0:3.0:3.0:4.0:3.0:3.0:3.0;" +
        "3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0/" +
        "3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0;" +
        "3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0;" +
        "3.0:4.0:3.0:4.0:3.0:3.0:3.0:3.0:4.0:3.0:3.0:3.0;" +
        "3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0:3.0";
}


#region 
public class SpecialObject
{
    public int[] InUnder;
    public int[] ThisUnder;
    public int[] CustomObject;

    //下にブロックを置くもの
    public SpecialObject()
    {
        InUnder = new int[] {
        (int)Utility.MapId.Fire,
        (int)Utility.MapId.House,
        (int)Utility.MapId.Player,
        (int)Utility.MapId.Stone,
        (int)Utility.MapId.Wall,
        (int)Utility.MapId.Warp,
        (int)Utility.MapId.Wood,
        (int)Utility.MapId.WoodBlock
        };

        ThisUnder = new int[] {
        (int)Utility.MapId.Ground,
        (int)Utility.MapId.Water,
        };

        CustomObject = new int[] {
        (int)Utility.ObjectId.House,
        (int)Utility.ObjectId.Warp,
        };
    }
}
#endregion
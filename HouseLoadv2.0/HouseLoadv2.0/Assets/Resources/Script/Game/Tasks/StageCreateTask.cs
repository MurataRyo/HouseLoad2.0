using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageCreateTask : MonoBehaviour
{
    public static GameObject[] objects;
    public const int Y_Scale = 4;       //Y軸のブロック同士の間隔
    public const int ZX_Scale = 1;       //Z軸のブロック同士の間隔

    void Start()
    {

    }

    //パスからの生成
    public void MapDataCreate(string path, List<MapObject> mapObjects, ref int[][][] mapData)
    {
        TextAsset text = Resources.Load<TextAsset>(path);
        MapDataToCreateStage(text.text, mapObjects, ref mapData);
        gameObject.AddComponent<DrawingFloorTask>();
    }

    public void MapDataToCreateStage(string str, List<MapObject> mapObjects, ref int[][][] mapData)
    {
        //下にブロックを置くもの
        SpecialObject Special = new SpecialObject();

        //ステージの親オブジェクト
        GameObject stageParent = new GameObject();
        stageParent.name = "StageParent";
        stageParent.tag = "StageParent";

        //アイテムとマップで分ける
        string[] str0 = str.Split(char.Parse("|")); ;

        ItemAdd(str0[0]);

        //str1→階層ごとのデータ
        string[] str1 = str0[1].Split(char.Parse("/"));
        mapData = new int[str1.Length][][];
        for (int y = 0; y < str1.Length; y++)
        {
            //階層ごとの親オブジェクト
            GameObject floor = new GameObject();
            floor.transform.parent = stageParent.transform;
            floor.name = "Floor" + y;
            floor.tag = "StageFloor";

            //str2→横1列ごとのデータ
            string[] str2 = str1[y].Split(char.Parse(";"));
            mapData[y] = new int[str2.Length][];

            for (int z = 0; z < str2.Length; z++)
            {
                //str3→1マスのデータ
                string[] str3 = str2[z].Split(char.Parse(":"));
                mapData[y][z] = new int[str3.Length];

                for (int x = 0; x < str3.Length; x++)
                {
                    string[] str4 = str3[x].Split(char.Parse("."));

                    int mapId = Convert.ToInt32(str4[0]);
                    int objectId = (int)Utility.GetObjectId((Utility.MapId)mapId);
                    int customId = Convert.ToInt32(str4[1]);
                    mapData[y][z][x] = mapId;

                    //プレイヤーマスは地面なのでデータを変更する
                    if (objectId == (int)Utility.ObjectId.Player)
                        mapData[y][z][x] = (int)Utility.MapId.Ground;

                    //地上に置くオブジェクトならtrue
                    if (Array.IndexOf(Special.ThisUnder, mapId) == -1)
                    {
                        Vector3 pos = Utility.DataToPosition(new Vector3Int(y, z, x));
                        CreateObject(pos, objectId, customId, Special, mapObjects, floor, false);
                        //オブジェクトの下にブロックを置くものならtrue
                        if (Array.IndexOf(Special.InUnder, mapId) != -1)
                            CreateObject(pos, (int)Utility.ObjectId.Ground, 0, Special, mapObjects, floor, true);
                    }
                    //地面に置くオブジェクト
                    else
                    {
                        Vector3 pos = Utility.DataToPosition(new Vector3Int(y, z, x));
                        CreateObject(pos, objectId, 0, Special, mapObjects, floor, true);
                    }
                }
            }
        }
    }

    //アイテムのデータを渡す
    private void ItemAdd(string str)
    {
        string[] itemCode = str.Split(char.Parse("/"));
        int[] num = new int[itemCode.Length];
        for (int i = 0; i < num.Length; i++)
        {
            num[i] = Convert.ToInt32(itemCode[i]);
        }
        GetComponent<ItemTask>().ItemCreate(num);
    }

    //オブジェクトの生成   　　　　　　　　　　　　　　　　　　　　　　　　　新規のSpecoalでOK                                親                   地面かどうか
    public void CreateObject(Vector3 position, int objectId, int customId, SpecialObject Special, List<MapObject> mapObjects, GameObject parentObj, bool isGround)
    {
        if (objects == null)
            objects = ObjectsLoad();

        GameObject obj = Instantiate(objects[objectId]);
        if (isGround)
        {
            position -= Vector3.up;
        }
        obj.transform.position = position;

        if (objectId != (int)Utility.ObjectId.Player)
        {
            obj.transform.parent = parentObj.transform;
        }

        Vector3Int pos = Utility.PositionToData(obj.transform.position);
        if (customId != 0)
            CustomObject(obj, objectId, customId, pos, mapObjects);
        else
        {
            MapObject mapObject = new MapObject(obj, objectId, pos);
            mapObjects.Add(mapObject);
        }
    }

    #region　カスタムIDありのやつ
    private void CustomObject(GameObject obj, int objectId, int customId, Vector3Int pos, List<MapObject> mapObjects)
    {
        switch (objectId)
        {
            case (int)Utility.ObjectId.House:
                MapObject mapObject = new MapObject(obj, objectId, pos);
                mapObjects.Add(mapObject);
                AngleCustom(obj, customId);
                break;

            case (int)Utility.ObjectId.Warp:
                WarpObject warpObject = new WarpObject(obj, objectId, pos, customId);
                mapObjects.Add(warpObject);

                obj.GetComponent<Warp>().number = customId;
                break;
        }
    }

    //0で前から時計回りに回転
    private void AngleCustom(GameObject obj, int customId)
    {
        obj.transform.eulerAngles = new Vector3(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y + customId * 90f, obj.transform.eulerAngles.z);
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
}


//条件式などに使用
public class SpecialObject
{
    public int[] InUnder;       //そのオブジェクトの下にブロックを置くかどうか
    public int[] ThisUnder;     //そのオブジェクトの位置がしたかどうか
    public int[] CustomObject;  //特殊な設定があるかどうか
    public int[] Kinematic;     //MapObjectsにいれないもの
    public int[] Block;         //プレイヤーが通れないところ
    public int[] ChoiceObject;  //選択可能なオブジェクト
    public int[] FallObject;    //落下可能なオブジェクト

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

        Kinematic = new int[]
        {
            (int)Utility.ObjectId.Ground,
            (int)Utility.ObjectId.Player
        };

        Block = new int[]
            {
                (int)Utility.ObjectId.Fire,
                (int)Utility.ObjectId.Stone,
                (int)Utility.ObjectId.Wall,
                (int)Utility.ObjectId.Water,
                (int)Utility.ObjectId.Wood,
                (int)Utility.ObjectId.WoodBlock,
                (int)Utility.ObjectId.Hole
            };

        ChoiceObject = new int[]
            {
                (int)Utility.MapId.Fire,
                (int)Utility.MapId.Stone,
                (int)Utility.MapId.Water,
                (int)Utility.MapId.Wood,
                (int)Utility.MapId.WoodBlock,
                (int)Utility.MapId.Hole,
                (int)Utility.MapId.Warp
            };

        FallObject = new int[]
        {
                (int)Utility.MapId.Ground,
        };
    }
}
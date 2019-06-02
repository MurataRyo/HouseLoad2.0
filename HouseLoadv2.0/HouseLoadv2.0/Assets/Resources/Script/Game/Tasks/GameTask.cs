﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTask : MonoBehaviour
{
    public List<MapObject> mapObjects;  //マップのオブジェクト(動く可能性のある物のみ)
    public int[][][] stageData;         //ステージのデータを更新する左から y z x
    public SpecialObject Special;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<StageCreateTask>();
        mapObjects = new List<MapObject>();
        GetComponent<StageCreateTask>().MapDataCreate(GetPath.Tutorial + "/Stage1", mapObjects, ref stageData);
        Special = new SpecialObject();
    }

    public bool InIfStageData(Vector3Int pos)
    {
        //プレイヤーが範囲外にいる場合
        if (pos.y < 0 || pos.z < 0 ||
            pos.y >= stageData[pos.x].Length ||
            pos.x >= stageData[pos.x][pos.y].Length)
            return false;

        return true;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ChoiceClass : IComparable
{
    public Vector3Int pos;  //場所
    public int mapId;       //ID
    public int number;      //ソート用
    private GameObject go;

    public ChoiceClass(Vector3Int pos, int mapId)
    {
        this.pos = pos;
        this.mapId = mapId;
        number = 0;
    }

    //このオブジェクトを探す
    public GameObject ThisObj()
    {
        if (go != null)
            return go;

        GameTask gameTask = Utility.GetGameTask();
        foreach (MapObject mapObject in gameTask.mapObjects)
        {
            if (mapObject.pos == pos)
            {
                go = mapObject.go;
                return go;
            }
        }

        Debug.Log("えらー");
        return null;
    }

    //並び替え用numberが大きいほうが配列番号が小さくなる
    public int CompareTo(object obj)
    {
        ChoiceClass choice = (ChoiceClass)obj;

        if (number > choice.number)
        {
            return -1;
        }
        else if (number < choice.number)
        {
            return 1;
        }

        return 0;
    }
}

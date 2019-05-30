﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StageCreateTask : MonoBehaviour
{
    GameObject[] objects;

    public void MapDataToCreateStage(string[] str1)
    {
        if(objects == null)


        //str1→階層ごとのデータ
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
                    string[] str4 = str3[x].Split(char.Parse(","));

                    int objectId = Convert.ToInt32(str4[0]);
                    int customId = Convert.ToInt32(str4[1]);

                    CreateObject(y, z, x, objectId, customId);
                }
            }
        }
    }

    private GameObject[] ObjectsLoad()
    {
        for(int i = 0;i < Enum.GetValues((Type)Utility.ObjectId))
    }

    public void CreateObject(int y, int z, int x, int objectId, int customId)
    {

    }
}

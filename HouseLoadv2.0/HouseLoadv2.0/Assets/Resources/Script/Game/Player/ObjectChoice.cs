using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectChoice : MonoBehaviour
{
    private PlayerTask playerTask;
    private GameObject choiceUi;
    private List<ChoiceClass> choiceObjects; //選択可能なオブジェクト
    private int choiceNum;                    //選択しているオブジェクト※choiceObjectsの配列番号
    private void Awake()
    {
        playerTask = GetComponent<PlayerTask>();
        choiceUi = Instantiate(Resources.Load<GameObject>(GetPath.Ui + "/ChoiceUi"));
        choiceUi.GetComponent<ChoiceUi>().mainCamera = playerTask.mainCamera;
        choiceUi.SetActive(false);
    }

    private void Update()
    {
        ChoiceChange();
    }

    private void ChoiceChange()
    {
        if (choiceObjects.Count == 0)
            return;

        if (Input.GetKeyDown(KeyCode.O))
        {
            ChoiceChange(true);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            ChoiceChange(false);
        }
    }

    private void ChoiceChange(bool flag)
    {
        choiceNum += Utility.BoolToInt(flag);
        if (choiceNum < 0 || choiceNum >= choiceObjects.Count)
        {
            if (choiceNum < 0)
                choiceNum = choiceObjects.Count - 1;
            else
                choiceNum = 0;
        }
        ChoicePosUpdate();
    }

    //マスが変更されたとき
    public void UpdatePos(Vector3Int oldPos, Vector3Int newPos)
    {
        Debug.Log("場所更新");
        NewChoiceObject(oldPos, newPos);
        choiceNum = 0;

        ChoiceUiSet();
    }

    //choiceObjectの更新
    void NewChoiceObject(Vector3Int oldPos, Vector3Int newPos)
    {
        choiceObjects = new List<ChoiceClass>();
        AddChoicePos(newPos);
        AddChoicePos(newPos + Vector3Int.up);
        AddChoicePos(newPos + Vector3Int.down);
        AddChoicePos(newPos + new Vector3Int(0, 0, 1));
        AddChoicePos(newPos + new Vector3Int(0, 0, -1));
        ChoicePosSort(oldPos, newPos);
    }

    //選択可能マスを追加
    void AddChoicePos(Vector3Int pos)
    {
        //存在しない場所なら
        if (!playerTask.gameTask.InIfStageData(pos))
            return;

        //選択不可な場所なら
        int mapId = Array.IndexOf(playerTask.gameTask.Special.ChoiceObject, playerTask.gameTask.stageData[pos.x][pos.y][pos.z]);
        if (mapId == -1)
            return;

        ChoiceClass choiceObject = new ChoiceClass(pos, mapId);
        choiceObjects.Add(choiceObject);
    }

    //並び替え
    void ChoicePosSort(Vector3Int oldPos, Vector3Int newPos)
    {
        int count = 1;

        //forrachとforで総あたりになる
        foreach (ChoiceClass choiceObject in choiceObjects)
        {
            for (int i = count; i < choiceObjects.Count; i++)
            {
                //centerに近いほど先に選択される※キャラの向いているマス
                Vector3 centerPos = Utility.DataToPosition(newPos * 2 - oldPos) + transform.forward;
                if ((Utility.DataToPosition(choiceObject.pos) - centerPos).magnitude < (Utility.DataToPosition(choiceObjects[i].pos) - centerPos).magnitude)
                {
                    choiceObject.number++;
                }
                else
                {
                    choiceObjects[i].number++;
                }
            }
            count++;
        }

        choiceObjects.Sort();
    }

    //UIの更新
    private void ChoiceUiSet()
    {
        if (choiceObjects.Count == 0)
        {
            if (choiceUi.activeInHierarchy)
                choiceUi.SetActive(false);

            return;
        }

        if (!choiceUi.activeInHierarchy)
            choiceUi.SetActive(true);

        ChoicePosUpdate();
    }

    private void ChoicePosUpdate()
    {
        //選択中の座標に場所を変更                                                          微調整
        choiceUi.transform.position = Utility.DataToPosition(choiceObjects[choiceNum].pos) + Vector3.up * 1.25f;
    }
}
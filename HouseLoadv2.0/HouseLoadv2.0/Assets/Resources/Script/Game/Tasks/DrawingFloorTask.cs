using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingFloorTask : MonoBehaviour
{
    GameObject stageParent;
    public GameObject[] floorObjects;
    WarpLine[] warpLines;

    private void Awake()
    {
        stageParent = GameObject.FindGameObjectWithTag("StageParent");
        int floorNum = stageParent.transform.childCount;
        floorObjects = new GameObject[floorNum];

        int count = 0;
        foreach (Transform child in stageParent.transform)
        {
            floorObjects[count] = child.gameObject;
            count++;
        }
    }

    public void StopDraw(int nextFloor)
    {
        for (int i = nextFloor + 1; i < floorObjects.Length; i++)
        {
            if (floorObjects[i].activeInHierarchy)
                floorObjects[i].SetActive(false);
        }
        if (warpLines == null)
            WarpObjLoad();

        WarpSet(nextFloor, false);
    }

    public void ResumeDraw(int nextFloor)
    {
        for (int i = nextFloor; i > 0; i--)
        {
            if (!floorObjects[i].activeInHierarchy)
                floorObjects[i].SetActive(true);
        }
        if (warpLines == null)
            WarpObjLoad();

        WarpSet(nextFloor, true);
    }

    public void Update()
    {

    }

    private void WarpObjLoad()
    {
        GameObject[] warpObject = GameObject.FindGameObjectsWithTag("WarpLine");
        warpLines = new WarpLine[warpObject.Length];
        for (int i = 0; i < warpObject.Length; i++)
        {
            warpLines[i] = warpObject[i].GetComponent<WarpLine>();
        }
    }

    //ワープの描画変更       　　　　 表示か消去か
    private void WarpSet(int nextFloor,bool set)
    {
        foreach (WarpLine warpLine in warpLines)
        {
            //アクティブしているかどうか
            bool actFlag = warpLine.gameObject.activeInHierarchy;

            if (actFlag == set ||
                actFlag == WarpLineIf(nextFloor, warpLine,set))
                continue;
            
                warpLine.gameObject.SetActive(set);
        }
    }

    //Lineを描画するかどうか
    private bool WarpLineIf(int nextFloor,WarpLine warpLine,bool set)
    {
        foreach(int linePos in warpLine.objectFloor)
        {
            if(nextFloor >= linePos)
            {
                return true;
            }
        }
        return false;
    }
}

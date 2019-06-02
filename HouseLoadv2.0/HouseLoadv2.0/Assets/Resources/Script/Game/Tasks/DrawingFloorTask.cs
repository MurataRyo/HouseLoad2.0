using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingFloorTask : MonoBehaviour
{
    GameObject stageParent;
    GameObject[] floorObjects;
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

        GameObject[] warpObject = GameObject.FindGameObjectsWithTag("WarpLine");
        for (int i = 0; i < warpObject.Length; i++)
        {
            warpLines[i] = warpObject[i].GetComponent<WarpLine>();
        }
    }

    public void StopDraw(int nextFloor)
    {
        for (int i = nextFloor + 1; i < floorObjects.Length; i++)
        {
            if (floorObjects[i].activeInHierarchy)
                floorObjects[i].SetActive(false);
        }
    }

    public void ResumeDraw(int nextFloor)
    {
        for (int i = nextFloor; i > 0; i--)
        {
            if (!floorObjects[i].activeInHierarchy)
                floorObjects[i].SetActive(true);
        }
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            StopDraw(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResumeDraw(1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpLine : MonoBehaviour
{
    public int[] objectFloor;
    void Awake()
    {
        objectFloor = new int[2];
    }
}

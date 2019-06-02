using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceMark : MonoBehaviour
{
    [HideInInspector] public GameObject mainCamera;
    // Update is called once per frame
    void Update()
    {
        //カメラにたいして向ける
        transform.LookAt(new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z));
    }
}

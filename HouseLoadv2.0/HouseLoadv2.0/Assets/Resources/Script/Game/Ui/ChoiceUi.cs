using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceUi : MonoBehaviour
{
    [HideInInspector] public GameObject mainCamera;
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(mainCamera.transform.position.x, transform.position.y, mainCamera.transform.position.z));
    }
}

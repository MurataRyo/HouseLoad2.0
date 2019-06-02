using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    public CameraTask cameraTask;
    public virtual void Awake()
    {
        cameraTask = Camera.main.GetComponent<CameraTask>();
    }

    public virtual void ChangeCamera()
    {

    }

    public virtual void MoveCamera()
    {

    }
}

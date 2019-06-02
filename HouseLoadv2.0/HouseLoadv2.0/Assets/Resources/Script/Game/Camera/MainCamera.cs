using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : BaseCamera
{
    public override void MoveCamera()
    {
        NextAngle();
        NextPos();
    }

    private void NextPos()
    {
        Vector3 pos = Vector3.zero;
        pos = cameraTask.player.transform.position + (-transform.forward * CameraTask.Range);

        transform.position = pos;
    }

    private void NextAngle()
    {
        transform.eulerAngles = new Vector3(cameraTask.angle.x, cameraTask.angle.y, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCamera : BaseCamera
{
    const float speed = 8f;
    public override void MoveCamera()
    {
        AngleUpdate();
        NextPos(cameraTask.player.transform.position, speed);
    }

    public override Vector3 NextAngle()
    {
        return new Vector3(cameraTask.angle.x, cameraTask.angle.y, 0f);
    }
}
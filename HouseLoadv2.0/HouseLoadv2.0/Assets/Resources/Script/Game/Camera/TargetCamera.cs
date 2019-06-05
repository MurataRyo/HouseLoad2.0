using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCamera : BaseCamera
{
    const float speed = 8f;
    public override void MoveCamera()
    {
        NextAngle();
        NextPos(cameraTask.player.transform.position, speed);
    }
}
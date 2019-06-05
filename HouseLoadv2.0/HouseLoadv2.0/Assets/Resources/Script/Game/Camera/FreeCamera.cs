using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : BaseCamera
{
    const float speed = 6f;
    Vector2 nextPos;
    int floor;
    public override void LoadCamera(BaseCamera baseCamera)
    {
        base.LoadCamera(baseCamera);
        nextPos = new Vector2(cameraTask.player.transform.position.x, cameraTask.player.transform.position.z);
        floor = Utility.PositionToData(cameraTask.player.transform.position).x;
    }

    public void NextPosUpdate()
    {
        Vector2 vec2 = gameTask.controllerTask.joyKey;

        //移動方向の取得
        Vector3 velocity =
            (new Vector3(transform.forward.x, 0f, transform.forward.z).normalized * vec2.y +
           transform.right * vec2.x).normalized;

        vec2 = new Vector2(velocity.x, velocity.z);
        nextPos += vec2 * Time.deltaTime * speed;

        if (gameTask.controllerTask.SerectKey(true))
        {
            floor++;
        }
        else if (gameTask.controllerTask.SerectKey(false))
        {
            floor--;
        }
        floor = Mathf.Clamp(floor, 0, gameTask.stageData.Length - 1);
    }

    private Vector3 NextPos()
    {
        return new Vector3(nextPos.x, floor * 4 + 1, nextPos.y);
    }

    public override void MoveCamera()
    {
        NextPosUpdate();
        NextAngle();
        NextPos(NextPos(), 10f);
    }

    public override int NowFloor(Vector3 nextPos)
    {
        return ((int)nextPos.y) / 4;
    }
}

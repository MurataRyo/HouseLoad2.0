using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamera : MonoBehaviour
{
    public CameraTask cameraTask;
    public GameTask gameTask;
    public DrawingFloorTask floorTask;
    public Vector3 targetPos;           //このカメラが目指している場所
    public int nowFloor;

    public virtual void Awake()
    {
        cameraTask = Camera.main.GetComponent<CameraTask>();
    }

    public virtual void Start()
    {
        nowFloor = 0;
        GameObject taskObject = Utility.GetTaskObject();
        floorTask = taskObject.GetComponent<DrawingFloorTask>();
        gameTask = taskObject.GetComponent<GameTask>();
    }

    public virtual void LoadCamera(BaseCamera baseCamera)
    {
        targetPos = baseCamera.targetPos;
        nowFloor = baseCamera.nowFloor;
    }

    public virtual void NextPos(Vector3 nextPos, float cameraSpeed)
    {
        targetPos = nextPos + (-transform.forward * cameraTask.CameraRange());
        transform.position = Vector3.MoveTowards(transform.position, targetPos, cameraSpeed * Time.deltaTime);
        nowFloor = NowFloor(nextPos);

        if (nowFloor >= 0 && nowFloor < gameTask.stageData.Length && transform.position == targetPos)
        {
            floorTask.StopDraw(nowFloor);
            floorTask.ResumeDraw(nowFloor);
        }
    }

    public virtual int NowFloor(Vector3 nextPos)
    {
        return ((int)(nextPos.y + 2.0f)) / 4;
    }

    public virtual void ChangeCamera()
    {

    }

    public virtual void MoveCamera()
    {

    }

    public void AngleUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(NextAngle()), (360f / 2) * Time.deltaTime);
    }

    public virtual Vector3 NextAngle()
    {
        return Vector3.zero;
    }
}

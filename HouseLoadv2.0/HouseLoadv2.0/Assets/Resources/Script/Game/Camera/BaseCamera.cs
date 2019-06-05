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

    public const float MinAngleX = 20f;
    public const float MaxAngleX = 60f;
    public Vector2 angle;
    private float RotateSpeed = 90f;

    public virtual void Awake()
    {
        angle = new Vector2(40f, 0f);
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
        angle = baseCamera.angle;
    }

    public virtual void NextPos(Vector3 nextPos, float cameraSpeed)
    {
        targetPos = nextPos + (-transform.forward * CameraTask.Range);
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

    public virtual void NextAngle()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            angle.y += Time.deltaTime * RotateSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            angle.y -= Time.deltaTime * RotateSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            angle.x -= Time.deltaTime * RotateSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            angle.x += Time.deltaTime * RotateSpeed;
        }

        angle.x = Mathf.Clamp(angle.x, MinAngleX, MaxAngleX);

        transform.eulerAngles = new Vector3(angle.x, angle.y, 0f);
    }

    public virtual void ChangeCamera()
    {

    }

    public virtual void MoveCamera()
    {

    }
}

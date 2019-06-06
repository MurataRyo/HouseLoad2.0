using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTask : MonoBehaviour
{
    public GameObject player;

    public BaseCamera baseCamera;
    private GameTask gameTask;
    public Vector2 angle;

    public const float MinAngleX = 20f;
    public const float MaxAngleX = 60f;
    private float RotateSpeed = 90f;
    enum CameraMode
    {
        Target,
        Free,
    }
    CameraMode cameraMode;
    // Start is called before the first frame update
    void Awake()
    {
        cameraMode = CameraMode.Target;
        baseCamera = gameObject.AddComponent<TargetCamera>();
    }

    void Start()
    {
        angle = new Vector2(40f, 0f);
        gameTask = Utility.GetGameTask();
    }

    // Update is called once per frame
    void Update()
    {
        ModeChange();
    }

    void ModeChange()
    {
        if (gameTask.controllerTask.CameraModeChangeButton())
        {
            if (cameraMode == CameraMode.Free)
            {
                gameTask.eventCount--;
                cameraMode = CameraMode.Target;
                BaseCamera newBase = gameObject.AddComponent<TargetCamera>();
                newBase.LoadCamera(baseCamera);
                Destroy(baseCamera);
                baseCamera = newBase;
            }
            else if (cameraMode == CameraMode.Target &&
                gameTask.eventCount == 0)
            {
                gameTask.eventCount++;
                cameraMode = CameraMode.Free;
                BaseCamera newBase = gameObject.AddComponent<FreeCamera>();
                newBase.LoadCamera(baseCamera);
                Destroy(baseCamera);
                baseCamera = newBase;
            }
        }
    }

    void LateUpdate()
    {
        if (cameraMode == CameraMode.Target)
            NextAngle();

        baseCamera.MoveCamera();
    }

    public float CameraRange()
    {
        if (cameraMode == CameraMode.Free)
            return 5f;

        return 4f;
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
    }
}

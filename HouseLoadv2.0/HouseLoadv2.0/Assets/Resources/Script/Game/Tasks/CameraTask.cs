using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTask : MonoBehaviour
{
    public GameObject player;
    public const float Range = 4f;

    public BaseCamera baseCamera;
    private GameTask gameTask;
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
        gameTask = Utility.GetGameTask();
    }

    // Update is called once per frame
    void Update()
    {
        ModeChange();
    }

    void ModeChange()
    {
        if(gameTask.controllerTask.CameraModeChangeButton())
        {
            if(cameraMode == CameraMode.Free)
            {
                gameTask.eventCount--;
                cameraMode = CameraMode.Target;
                BaseCamera newBase = gameObject.AddComponent<TargetCamera>();
                newBase.LoadCamera(baseCamera);
                Destroy(baseCamera);
                baseCamera = newBase;
            }
            else if(cameraMode == CameraMode.Target &&
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
        baseCamera.MoveCamera();
    }
}

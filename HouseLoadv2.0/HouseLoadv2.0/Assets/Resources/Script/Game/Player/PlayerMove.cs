using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    ControllerTask controllerTask;
    PlayerTask playerTask;
    // Start is called before the first frame update
    void Start()
    {
        playerTask = GetComponent<PlayerTask>();
        controllerTask = Utility.GetTaskObject().GetComponent<ControllerTask>();
    }

    // Update is called once per frame
    void NextPos()
    {
        Vector3 nextPos = transform.position + transform.forward * controllerTask.joyKey.y + transform.right * controllerTask.joyKey.y;
    }
}
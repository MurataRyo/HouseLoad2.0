using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask : MonoBehaviour
{
    PlayerMove playerMove;
    public ControllerTask controllerTask;
    public GameObject mainCamera;
    public GameTask gameTask;
    // Start is called before the first frame update
    void Awake()
    {
        controllerTask = Utility.GetTaskObject().GetComponent<ControllerTask>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.GetComponent<CameraTask>().player = gameObject;
        playerMove = gameObject.AddComponent<PlayerMove>();
        gameTask = Utility.GetGameTask();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            Vector3Int[] vec3 = Utility.PositionToData(transform.position, 0.3f);
            foreach (Vector3Int i in vec3)
            {
                Debug.Log(i);
            }
        }
    }

    void FixedUpdate()
    {
        playerMove.Move();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTask : MonoBehaviour
{
    public PlayerMove playerMove;
    public ObjectChoice objectChoice;

    public ControllerTask controllerTask;
    public GameObject mainCamera;
    public GameTask gameTask;

    public Vector3Int pos;
    // Start is called before the first frame update
    void Awake()
    {
        controllerTask = Utility.GetTaskObject().GetComponent<ControllerTask>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.GetComponent<CameraTask>().player = gameObject;
        playerMove = gameObject.AddComponent<PlayerMove>();
        objectChoice = gameObject.AddComponent<ObjectChoice>();
        gameTask = Utility.GetGameTask();

        pos = Utility.PositionToData(transform.position);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        objectChoice.ChoiceChange();
        playerMove.Move();
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3Int[] vec3 = Utility.PositionToData(transform.position, 0.3f);
            foreach (Vector3Int i in vec3)
            {
                Debug.Log((Utility.MapId)gameTask.stageData[i.x][i.y][i.z]);
            }
        }
    }

    void FixedUpdate()
    {

    }
}
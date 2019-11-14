using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTask : MonoBehaviour
{
    public PlayerMove playerMove;
    public ObjectChoice objectChoice;

    public ControllerTask controllerTask;
    public GameObject mainCamera;
    public GameTask gameTask;

    public Vector3Int pos;
    public Vector3Int posLog;
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
        UpdatePos();
    }

    void UpdatePos()
    {
<<<<<<< HEAD
        /*if(posLog != pos)
        {
            if(gameTask.stageData[pos.x][pos.y][pos.z] == (int)Utility.MapId.House)
            {
                //ゴール判定 
                playerMove.StartCoroutine(playerMove.NowPosMoveAndRotation(pos, pos));
            }
        }*/
        Vector3Int[] vec3 = Utility.PositionToData(transform.position, 0.25f);
        if(gameTask.eventCount == 0)
        {
            foreach (Vector3Int i in vec3)
            {
                if (gameTask.stageData[i.x][i.y][i.z] == (int)Utility.MapId.House)
                {
                    if(transform.position == Utility.DataToPosition(i))
                    {
                        SceneManager.LoadScene("Clear");
                        break;
                    }

                    //ゴール判定
                    playerMove.StartCoroutine(playerMove.NowPosMoveAndRotation(i, i));
                }
=======
        posLog = pos;
        if(posLog != pos)
        {
            if(gameTask.stageData[pos.x][pos.y][pos.z] == (int)Utility.MapId.House)
            {
                
>>>>>>> parent of e9b2820... タイトル画面　クリア画面　ロード画面の追加
            }
        }
    }

    void FixedUpdate()
    {

    }
}
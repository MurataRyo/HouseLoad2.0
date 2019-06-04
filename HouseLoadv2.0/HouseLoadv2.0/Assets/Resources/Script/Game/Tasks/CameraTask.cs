using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTask : MonoBehaviour
{
    public GameObject player;
    public const float MinAngleX = 20f;
    public const float MaxAngleX = 60f;
    public Vector2 angle;
    public const float Range = 4f;
    public Vector2 position = Vector2.zero;
    private float RotateSpeed = 90f;

    private BaseCamera baseCamera;
    // Start is called before the first frame update
    void Awake()
    {
        angle = new Vector2(40f, 0f);
        baseCamera = gameObject.AddComponent<MainCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        baseCamera.MoveCamera();

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

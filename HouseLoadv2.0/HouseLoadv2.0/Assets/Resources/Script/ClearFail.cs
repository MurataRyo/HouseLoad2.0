using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ClearFail : MonoBehaviour
{
    ControllerTask controllerTask;
    private void Start()
    {
        controllerTask = GetComponent<ControllerTask>();
    }
    // Start is called before the first frame update
    void Update()
    {
        if(controllerTask.EnterButton())
        {
            SceneManager.LoadScene("Title");
        }
    }
}

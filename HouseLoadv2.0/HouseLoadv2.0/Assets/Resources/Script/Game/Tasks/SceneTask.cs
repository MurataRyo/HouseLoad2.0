using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTask : MonoBehaviour
{
    public enum SceneName
    {
        Main,
        Load,
        Title
    }

    //ロード関係---------------------------------------
    public static SceneName nextScene;
    AsyncOperation loadScene;      //ロード先
    const float LoadTimeMin = 0.5f;  //最低のロード時間
    //------------------------------------------------

    private void Start()
    {
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name != SceneName.Load.ToString())
        {
            Debug.Log("Load以外のScene");
            return;
        }

        loadScene = SceneManager.LoadSceneAsync(nextScene.ToString());
        loadScene.allowSceneActivation = false;
        StartCoroutine(load());
    }

    public void LoadScene(SceneName sceneName, bool LoadIf)
    {
        nextScene = sceneName;
        if (LoadIf)
            SceneManager.LoadScene(SceneName.Load.ToString());
        else
            SceneManager.LoadScene(nextScene.ToString());
    }

    IEnumerator load()
    {
        Debug.Log(LoadTimeMin);
        yield return new WaitForSeconds(LoadTimeMin);
        loadScene.allowSceneActivation = true;
        yield break;
    }
}

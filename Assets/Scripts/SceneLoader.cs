using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string SceneToBeLoaded;
    public void LoadScene(string sName)
    {
        SceneToBeLoaded = sName;

        StartCoroutine(InitialiseScaneLoading());
    }

    IEnumerator InitialiseScaneLoading()
    {
        yield return SceneManager.LoadSceneAsync("Scene_Loading");
        StartCoroutine(LoadActualScnene());
    }

    IEnumerator LoadActualScnene()
    {
       var asyncSceneLoad = SceneManager.LoadSceneAsync(SceneToBeLoaded);

        asyncSceneLoad.allowSceneActivation = false;

        while(!asyncSceneLoad.isDone)
        {
            Debug.Log(asyncSceneLoad.progress);
            if(asyncSceneLoad.progress>=0.9f)
            {
                asyncSceneLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}

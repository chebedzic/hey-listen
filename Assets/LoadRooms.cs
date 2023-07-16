using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRooms : MonoBehaviour
{

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadingRooms());

        for (int i = SceneManager.GetActiveScene().buildIndex + 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            scenesToLoad.Add(SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive));
        }

        //for (int i = SceneManager.GetActiveScene().buildIndex + 1; i < SceneManager.sceneCountInBuildSettings; i++)
        //{
        //    SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
        //}
    }

    IEnumerator LoadingRooms()
    {
        float totalProgress = 0;

        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                yield return null;
            }
        }

        LightProbes.Tetrahedralize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

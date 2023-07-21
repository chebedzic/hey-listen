using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadRooms : MonoBehaviour
{

#if UNITY_EDITOR

    public UnityEditor.SceneAsset[] targetSceneAsset;
    private void OnValidate()
    {
        targetScenes = new string[targetSceneAsset.Length];
        for (int i = 0; i < targetSceneAsset.Length; i++)
            if (targetSceneAsset[i] != null)
                targetScenes[i] = targetSceneAsset[i].name;
            else
                Debug.LogError("Target Scene Asset is NULL >:(");
    }

#endif

    [HideInInspector] public string[] targetScenes;
    public UnityEvent onLoadComplete;

    public void Start()
    {
        StartCoroutine(LoadingRooms());


        IEnumerator LoadingRooms()
        {
            foreach (var scene in targetScenes)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
            }
            LightProbes.Tetrahedralize();
            onLoadComplete.Invoke();
        }
    }
}

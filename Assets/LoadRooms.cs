using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadRooms : MonoBehaviour
{

    [SerializeField] private int[] allRooms;

    // Start is called before the first frame update
    void Start()
    {
        foreach (int room in allRooms)
            SceneManager.LoadSceneAsync(room, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

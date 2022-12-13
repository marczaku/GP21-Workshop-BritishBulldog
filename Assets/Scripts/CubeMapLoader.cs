using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMapLoader : MonoBehaviour
{
    void Start(){
        for(int i = 0; i < 10; i++)
        {
            SpawnPowerUp((success) =>
            {
                if (!success)
                {
                    Debug.Log($"Power Up Spawn #{i} Failed.");
                }
            });
        }
    }

    public void SpawnPowerUp(Action<bool> callback){
        StartCoroutine(Co_SpawnPowerUp(callback));
    } 

    IEnumerator Co_SpawnPowerUp(Action<bool> callback){
        yield return new WaitForSeconds(0.5f);
        callback(false);
    }
}
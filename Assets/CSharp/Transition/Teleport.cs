using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SceneName]
    public string secneToGo;
    public Vector3 positionToGo;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EventHander.CallTransitionEvent(SceneManager.GetActiveScene().name, secneToGo, positionToGo);
        }
    }
}

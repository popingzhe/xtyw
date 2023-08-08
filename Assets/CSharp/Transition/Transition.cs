using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Transition : MonoBehaviour
{
    [SceneName]
    public string startName = string.Empty;

    private CanvasGroup fadeCanvasDroup;

    //���������Ƿ����
    bool isFade;

    private void OnEnable()
    {
        EventHander.TransitionEvent += OnTransitionEvent;
    }

    private void OnDisable()
    {
        EventHander.TransitionEvent -= OnTransitionEvent;
    }

/*    private void Update()
    {
        Scene currentActiveScene = SceneManager.GetActiveScene();
        Debug.Log(currentActiveScene.name);
    }*/
    private void OnTransitionEvent(String s,string sceneToGo, Vector3 posToGo)
    {
        if (!isFade)
        StartCoroutine(MTransition(s,sceneToGo,posToGo));
    }

    private void Start()
    {
        fadeCanvasDroup = FindObjectOfType<CanvasGroup>();
        StartCoroutine(LoadScenceSetActive(startName));

    }


    private IEnumerator MTransition(String s,string sceneName,Vector3 targetPosition)
    {
        yield return Fade(1);
        EventHander.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        yield return LoadScenceSetActive(sceneName);

        //������ƶ���ѡ��λ��
        EventHander.CallMoveToPosition(targetPosition);
  
        EventHander.CallAfterSceneloadEvent();
        yield return Fade(0);

    }

    private IEnumerator LoadScenceSetActive(string SecneName)
    {
        //�첽���س���
        yield return SceneManager.LoadSceneAsync(SecneName,LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount-1);

        SceneManager.SetActiveScene(newScene);
    }

    //1��0͸��
    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasDroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasDroup.alpha - targetAlpha)/Setting.fadeDuration;
       
        while(!Mathf.Approximately(fadeCanvasDroup.alpha, targetAlpha))
        {
            fadeCanvasDroup.alpha = Mathf.MoveTowards(fadeCanvasDroup.alpha, targetAlpha,  speed*Time.deltaTime);
            yield return null;
        }
        fadeCanvasDroup.blocksRaycasts = false;
        isFade = false;
    }


}

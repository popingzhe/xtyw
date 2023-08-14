using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage;

    public RectTransform clockParent;

    public Image seasonIamge;

    public TextMeshProUGUI dateText;

    public TextMeshProUGUI timeText;

    public Sprite[] seasonSprites;

    private List<GameObject> clockBlocks = new List<GameObject>();

    private void Awake()
    {
        //初始化
        for(int i = 0; i < clockParent.childCount; i++)
        {
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventHander.GameMinuteEvenet += OnGameMinuteEvenet;
        EventHander.GameDateEvenet += OnGameDateEvenet;
    }

    private void OnDisable()
    {
        EventHander.GameMinuteEvenet -= OnGameMinuteEvenet;
        EventHander.GameDateEvenet -= OnGameDateEvenet;
    }

    private void OnGameDateEvenet(int hour, int day, int month, int year, Season season)
    {
        dateText.text = year + "年" + month.ToString("00") + "月" + day.ToString("00") + "日";
        seasonIamge.sprite = seasonSprites[(int)season];
        BayNightImageRotate(hour);
        SwitchHourImage(hour);
    }

    private void SwitchHourImage(int hour)
    {
        int index = hour / 4;
        if(index == 0)
        {
            foreach (var item in clockBlocks)
            {
                item.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < clockBlocks.Count; i++)
            {
                if (i < index+1)
                {
                    clockBlocks[i].SetActive(true);
                }
                else
                {
                    clockBlocks[i].SetActive(false);
                }
            }
        }
    }

    private IEnumerator RotateToTargetAngle(Transform targetTransform, float targetAngle, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);

        while (Quaternion.Angle(targetTransform.rotation, targetRotation) > 0.1f)
        {
            targetTransform.rotation = Quaternion.Slerp(targetTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void BayNightImageRotate(int hour)
    {
        float targetAngle = hour * 15f;
        float rotationSpeed = 5f;

        StartCoroutine(RotateToTargetAngle(dayNightImage.transform, targetAngle, rotationSpeed));
    }
    private void OnGameMinuteEvenet(int minute, int hour)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }
}



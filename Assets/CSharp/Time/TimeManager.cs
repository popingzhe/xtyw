using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond, gameMinute,gameHour,gameDay,gameMonth,gameYear;

    private Season gameSeason = Season.spring;

    private int mouthInSeason = 3;

    public bool gameClockPause;

    private float tikTime;

    private void Awake()
    {
        NewGameTime();
    }

    private void Start()
    {
        EventHander.CallGameDateEvenet(gameHour, gameDay, gameMonth, gameYear,gameSeason);
        EventHander.CallGameMinuteEvenet(gameMinute, gameHour);
    }
    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;
            if(tikTime >=Setting.secondThreshold)
            {
                tikTime -= Setting.secondThreshold;
                UpdateGameTime();
            }
        }
        if(Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }

        if(Input.GetKey(KeyCode.G))
        {
            gameDay++; 
            EventHander.CallGameDateEvenet(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            EventHander.CallGameDayEvenet(gameDay, gameSeason);
        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2023;
        gameSeason = Season.spring;
    }

    private void UpdateGameTime()
    {
        gameSecond++;
        if(gameSecond > Setting.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if(gameMinute > Setting.minuteHold)
            {
                gameHour++;
                gameMinute = 0;
                if(gameHour > Setting.hourHold)
                {
                    gameDay++;
                    gameHour = 0;
                    if( gameDay > Setting.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;
                        if(gameMonth > 12)
                        {
                            gameMonth = 1;
                        }

                        mouthInSeason--;
                        if(mouthInSeason == 0)
                        {
                            mouthInSeason = 3;
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if(seasonNumber > Setting.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear ++;   
                            }

                            gameSeason = (Season) seasonNumber;

                            if(gameYear > 9999)
                            {
                                gameYear = 2023;
                            }
                        }
                        
                    }
                    EventHander.CallGameDayEvenet(gameDay, gameSeason);
                }
                EventHander.CallGameDateEvenet(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHander.CallGameMinuteEvenet(gameMinute, gameHour);
        }
     //   Debug.Log("S:"+gameSecond+" M"+gameMinute);
    }
}



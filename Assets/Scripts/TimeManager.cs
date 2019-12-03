using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private float curruntTime;
    private float curruntFill;
    private Image timeGauge;
    public GameObject resultSet;
    public GameObject uiHandler;
    public GameManager gm;

    private bool isTimeOver;
    private float maxTime;
    private AudioManager audio;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        timeGauge = GetComponent<Image>();
    }

    void Start()
    {
        maxTime = GameManager.maxTime;
        isTimeOver = false;
        curruntTime = maxTime;
    }

    void Update()
    {
        if (curruntTime > 0)
        {
            TimeDecrease();
        }
        timeGauge.fillAmount = curruntFill;

        if (curruntFill <= 0.0 && isTimeOver == false)
        {
            TimeOver();
        }
    }

    private void TimeOver()
    {
        isTimeOver = true;

        Debug.Log("Result");
        gm.SetStar();
        audio.SetBGMPitch(1.0f);
        uiHandler.GetComponent<UIHandler>().Pause();
        uiHandler.GetComponent<UIHandler>().PopupHandler(resultSet);
    }

    private void TimeDecrease()
    {
        curruntTime -= Time.deltaTime;
        if (curruntTime < 0)
        {
            curruntTime = 0;
        }
        curruntFill = curruntTime / maxTime;
    }
}

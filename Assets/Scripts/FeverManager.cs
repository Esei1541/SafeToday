using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverManager : MonoBehaviour
{
    private const float MaxFever = 100;

    private static float curruntFever;
    private static float feverBonusRate;

    private float curruntFill;
    private Image feverGauge;
    [HideInInspector] public static bool isFever;
    private AudioManager audio;

    public float feverTimeLength;
    public Camera camera;
    
    void Awake()
    {
        feverGauge = GetComponent<Image>();
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        isFever = false;
        feverBonusRate = 1;
        curruntFever = 0;
    }

    void Update()
    {
        curruntFill = curruntFever / MaxFever;
        feverGauge.fillAmount = curruntFill;

        if (!isFever && curruntFever >= MaxFever)
        {
            StartFeverMode();
        }

        if (isFever)
        {
            FeverMode();
        }
    }

    public static void FeverIncrease(float value)
    {
        if (!isFever)
        {
            curruntFever += (value * feverBonusRate);
            if (curruntFever <= 0)
            {
                curruntFever = 0;
            }
            else if (curruntFever > MaxFever)
            {
                curruntFever = MaxFever;
            }
        }
    }

    public static void FeverIncreaseRaw(float value)
    {
        if (!isFever)
        {
            curruntFever += value;
            if (curruntFever <= 0)
            {
                curruntFever = 0;
            }
            else if (curruntFever > MaxFever)
            {
                curruntFever = MaxFever;
            }
        }
    }

    public static void SetBonusRate(float rate)
    {
        feverBonusRate = rate;
    }

    private void StartFeverMode()
    {
        camera.GetComponent<Camera>().backgroundColor = new Color32(255, 135, 0, 255);
        isFever = true;
        audio.SetBGMPitch(1.5f);
    }

    private void FeverMode()
    {
        curruntFever -= (MaxFever / feverTimeLength) * Time.deltaTime;
        GameManager.SetFeverScoreBonus(3);
        if (curruntFever <= 0)
        {
            GameManager.SetFeverScoreBonus(1);
            curruntFever = 0;
            if (ObstacleManager.isOwnerCome)
            {
                camera.GetComponent<Camera>().backgroundColor = new Color32(255, 0, 0, 255);
            } else
            {
                camera.GetComponent<Camera>().backgroundColor = new Color32(0, 0, 0, 255);
            }
            audio.SetBGMPitch(1.0f);
            isFever = false;
        }
    }

    /* 디버그용 메서드 */
    public static float GetIncreaseValue() {
        return 3 * feverBonusRate;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    private int level;                                  // StageSelect Scene에서 받아올 level 값

    /* 스테이지 난이도 설정용 변수 */
    // SetLevel() 메서드에서 설정
    public static float maxTime;                        // 제한시간 -> TimeManager
    public static int roachFreinds;                     // 등장하는 바퀴벌레의 수
    public static int[] obstacleType = new int[2];      // 방해요소 난수 범위를 지정 : Random.Range(obstacleType[0], obstacleType[1]) -> ObstacleManager
    public static float obstacleTiming;                 // 방해요소 난수 생성 주기를 지정 -> ObstacleManager
    /* --------------------------- */

    private static int feverScoreBonus;
    private static int score;
    private static int scoreBonusRate;
    private static int combo;
    private static int maxCombo;
    private static int star;
    private GameObject windowSunset;
    public GameObject[] stars;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxScoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI maxComboText;
    public GameObject roach;
    public Sprite starImage;
    public int testLevel;

    private void Awake()
    {
        windowSunset = GameObject.Find("WindowSunset");
        if (testLevel == 0)    // 디버그용 변수 testLevel이 1 이상일 경우 해당 스테이지로 설정
        {
            level = DataManager.instance.selectLevel;
        } else
        {
            level = testLevel;
        }
        
        SetLevel(level);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (level < 4)
        {
            windowSunset.SetActive(false);
        }
        maxCombo = 0;
        combo = 0;
        score = 0;
        star = 0;
        scoreBonusRate = 0;
        feverScoreBonus = 1;
        for (int i = 1; i <= roachFreinds; i++)         // SetLevel 메서드에서 정해진 roachFreinds의 값만큼 바퀴벌레를 생성하여 Grid의 자식요소로 붙여준다.
        {
            GameObject roachInstance = Instantiate(roach, new Vector2(Random.Range(-4.0f, 5.0f), Random.Range(-3.5f, 1.5f)), Quaternion.identity);
            roachInstance.transform.parent = GameObject.Find("Grid").GetComponent<Transform>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = string.Format("{0:00000000}", score);
        comboText.text = string.Format("{0:000}", combo);
        maxScoreText.text = string.Format("{0:00000000}", score);

        if (combo > maxCombo)
        {
            maxCombo = combo;
            maxComboText.text = string.Format("{0:000}", maxCombo);
        }

        SetComboBonusRate();
    }

    public void ScoreIncrease(int value)
    {
        score += (value + ((value / 100) * scoreBonusRate)) * feverScoreBonus;
        ComboIncrease();
        if (score <= 0)
        {
            score = 0;
        }
        Debug.Log("Score Get: " + (value + ((value / 100) * scoreBonusRate)) * feverScoreBonus + " / Fever Get: " + FeverManager.GetIncreaseValue());
    }

    public void ScoreIncreaseRaw(int value)
    {
        score += value;
        ComboIncrease();
        if (score <= 0)
        {
            score = 0;
        }
        Debug.Log("Score Get: " + value);
    }

    public void ComboIncrease()
    {
        combo++;
        CancelInvoke("ComboReset");
        Invoke("ComboReset", 8.0f);
    }
    
    public void SetComboBonusRate()
    {
        if (combo < 5)
        {
            scoreBonusRate = 0;
            FeverManager.SetBonusRate(1);
        } else if (combo < 10)
        {
            scoreBonusRate = 15;
            FeverManager.SetBonusRate(2);
        } else if (combo < 20)
        {
            scoreBonusRate = 30;
            FeverManager.SetBonusRate(3);
        } else
        {
            scoreBonusRate = 50;
            FeverManager.SetBonusRate(4);
        }

    }

    public void ComboReset()
    {
        if (!ObstacleManager.isOwnerCome)
        {
            combo = 0;
        }
    }

    public static void SetFeverScoreBonus(int value)
    {
        feverScoreBonus = value;
    }

    public void SetStar()
    {
        switch (level)
        {
            case 1:
                if (score >= 11000)
                {
                    star = 3;
                } else if (score >= 8000)
                {
                    star = 2;
                } else if (score >= 5000)
                {
                    star = 1;
                } else
                {
                    star = 0;
                }
                break;
            case 2:
                if (score >= 30000)
                {
                    star = 3;
                }
                else if (score >= 25000)
                {
                    star = 2;
                }
                else if (score >= 15000)
                {
                    star = 1;
                }
                else
                {
                    star = 0;
                }
                break;
            case 3:
                if (score >= 33000)
                {
                    star = 3;
                }
                else if (score >= 26000)
                {
                    star = 2;
                }
                else if (score >= 18000)
                {
                    star = 1;
                }
                else
                {
                    star = 0;
                }
                break;
            case 4:
                if (score >= 37000)
                {
                    star = 3;
                }
                else if (score >= 28000)
                {
                    star = 2;
                }
                else if (score >= 22000)
                {
                    star = 1;
                }
                else
                {
                    star = 0;
                }
                break;
            case 5:
                if (score >= 60000)
                {
                    star = 3;
                }
                else if (score >= 45000)
                {
                    star = 2;
                }
                else if (score >= 35000)
                {
                    star = 1;
                }
                else
                {
                    star = 0;
                }
                break;
        }

        for (int i = 0; i < 3; i++)
        {
            if (i < star)
            {
                stars[i].GetComponent<SpriteRenderer>().sprite = starImage;
            }
        }

        if (PlayerPrefs.GetInt("ClearProgress") < level && star >= 1) // 스테이지를 클리어하지 않은 상태에서 별 1개 이상을 취득할 경우 클리어 처리
        {
            PlayerPrefs.SetInt("ClearProgress", level);
        }
        
        if (star > PlayerPrefs.GetInt("MaxStar" + level))              // 이번 판에서 취득한 별 수가 기록보다 많다면 저장
        {
            PlayerPrefs.SetInt("MaxStar" + level, star);
        }
        
    }

    public void SetLevel(int level)
    {
        // 스테이지의 세팅값을 불러와 게임 시작 시 적용
        switch (level)
        {
            case 1:
                maxTime = 30.0f;
                roachFreinds = 0;
                obstacleType[0] = 3;
                obstacleType[1] = 4;
                obstacleTiming = 999.9f;
                break;
            case 2:
                maxTime = 50.0f;
                roachFreinds = 1;
                obstacleType[0] = 3;
                obstacleType[1] = 4;
                obstacleTiming = 999.0f;
                break;
            case 3:
                maxTime = 60.0f;
                roachFreinds = 2;
                obstacleType[0] = 0;
                obstacleType[1] = 1;
                obstacleTiming = 15.0f;
                break;
            case 4:
                maxTime = 75.0f;
                roachFreinds = 2;
                obstacleType[0] = 1;
                obstacleType[1] = 2;
                obstacleTiming = 15.0f;
                break;
            case 5:
                maxTime = 90.0f;
                roachFreinds = 3;
                obstacleType[0] = 0;
                obstacleType[1] = 2;
                obstacleTiming = 10.0f;
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class SelectMenuManager : MonoBehaviour
{
    private const int STAGECOUNT = 5;
    public Image background;
        
    /* 스테이지 정보를 저장하는 구조체 */
    [Serializable]
    public struct stage
    {
        public int stageNumber;             // 스테이지 No.
        public string stageTitle;           // 스테이지명
        public Sprite previewImage;         // 버튼을 선택했을 때 배경에 깔릴 이미지
    }

    public stage[] stageInfo;
    public GameObject[] stageButton;
    public Sprite[] starImage;
    public TextMeshProUGUI TitleText;

    public GameObject[] stars = new GameObject[3];

    private void Awake()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        DataManager.instance.selectLevel = 1;
    }

    private void Start()
    {
        Debug.Log(PlayerPrefs.GetInt("StoryProgress"));
        DisableButton();
    }

    private void Update()
    {
        SetMenuTitle(DataManager.instance.selectLevel - 1);
    }

    public void SetMenuTitle(int index)
    {
        TitleText.text = "제 " + stageInfo[index].stageNumber + " 화\n" + "<" + stageInfo[index].stageTitle + ">";
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < PlayerPrefs.GetInt("MaxStar" + stageInfo[index].stageNumber))
            {
                stars[i].GetComponent<SpriteRenderer>().sprite = starImage[1];
            } else
            {
                stars[i].GetComponent<SpriteRenderer>().sprite = starImage[0];
            }
        }
    }

    public void DisableButton()
    {
        for (int i = STAGECOUNT - 1; i > PlayerPrefs.GetInt("ClearProgress"); i--)
        {
            stageButton[i].GetComponent<Button>().interactable = false;
            stageButton[i].GetComponent<Image>().color = new Color32(80, 80, 80, 255);
        }
    }

    public void ChangePreview()
    {
        /* 버튼을 눌렀을 때 호출되어 배경 이미지를 바꾸는 메서드 */
        int stageNumber = int.Parse(UIHandler.GetClickedButtonName().Replace("StageButton","")) - 1;    // 선택한 스테이지 버튼에서 배열의 index no를 얻는다.
        Debug.Log(stageNumber);
        background.gameObject.GetComponent<Image>().sprite = stageInfo[stageNumber].previewImage;
    }
}

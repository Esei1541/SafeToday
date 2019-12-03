using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /* PlayerPrefs 패러미터 목록
    int StoryProgress   : 스토리 진행도(어디까지 봤는가)의 값
    int MaxStar1~5      : 각 스테이지마다 최대로 획득한 별의 값
    int ClearProgress   : 현재까지 클리어한 스테이지의 값 
    ------------------------- */

    public static DataManager instance = null;
    [HideInInspector] public int selectLevel;

    private void Awake()
    {
        /* ---------- 싱글톤 구현 ---------- */
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        /* --------------------------------- */
    }

}

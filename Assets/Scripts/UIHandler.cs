using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class UIHandler : MonoBehaviour
{
    private bool isPaused;

    [SerializeField]
    private GameObject[] defaultHiddenObject;                    // 게임 시작 시 숨길 UI

    [SerializeField]
    private GameObject[] onPauseHiddenObjects;                   // Pause() 메서드가 실행될 때 숨길 UI

    private AudioManager aud;


    private void Awake()
    {
        aud = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    protected virtual void Start()
    {
        isPaused = false;
        for (int i = 0; i < defaultHiddenObject.Length; i++)
        {
            defaultHiddenObject[i].SetActive(false);
        }
    }

    /* 버튼 터치 게임오브젝트를 매개변수로 받아 active 상태를 전환해준다. */
    public void PopupHandler(GameObject popupWindow)
    {
        popupWindow.SetActive(!popupWindow.activeInHierarchy);
    }

    /* 문자열을 매개변수로 받아 해당 이름의 씬으로 넘겨주는 메서드 */
    public void SceneChanger(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("LoadScene(" + sceneName + ")");
        Debug.Log("DataManager.instance.selectLevel: " + DataManager.instance.selectLevel);
    }

    /* 현재 스토리 진행도와 일치하면 Dialog 씬으로, 아닐 경우 elseSceneName 씬으로 넘겨주는 메서드 */
    public void SceneChangerStory(string progressAndSceneName)
    {
        int progress = int.Parse(progressAndSceneName.Substring(0,1));
        string elseSceneName = progressAndSceneName.Substring(1, progressAndSceneName.Length - 1);

        if (PlayerPrefs.GetInt("StoryProgress") == progress)
        {
            SceneManager.LoadScene("Dialog");
        } else
        {
            SceneManager.LoadScene(elseSceneName);
        }
    }

    public void SceneChangerLast(string SceneName)
    {
        if (PlayerPrefs.GetInt("StoryProgress") == 6 && PlayerPrefs.GetInt("MaxStar5") >= 1)
        {
            SceneManager.LoadScene("Dialog");
        }
        else
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    public void PlayButton()
    {
        if (PlayerPrefs.GetInt("StoryProgress") == DataManager.instance.selectLevel)
        {
            SceneManager.LoadScene("Dialog");
        }
        else
        {
            SceneManager.LoadScene("GamePlay");
        }
    }

    /* DataManager 싱글턴의 selectLevel 변수를 설정해준다 */
    public void SetSelectLevel(int level)
    {
        DataManager.instance.selectLevel = level;
        Debug.Log("DataManager.selectLevel: " + DataManager.instance.selectLevel);
    }

    /* 메서드를 호출하는 오브젝트를 클릭했을 때 해당 오브젝트의 이름을 가져온다*/
    public static string GetClickedButtonName()
    {
        string clickedObjectName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(clickedObjectName);

        return clickedObjectName;
    }
    
    /*
    public static IEnumerator popSlideText(TextMeshProUGUI text)
    {
        // 안 됨
        text.faceColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        text.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
        text.gameObject.SetActive(true);

        while (text.faceColor.a != 0)
        {
            text.transform.Translate(new Vector2(text.transform.position.x, text.transform.position.y + 0.1f));
            text.faceColor = new Color(1.0f, 1.0f, 1.0f, text.faceColor.a - 0.01f);
            yield return new WaitForSeconds(0.1f);
        }

        text.gameObject.SetActive(false);
    }
    */

    /* 게임의 일시정지 상태를 전환하는 메서드 */
    public void Pause()
    {
        aud.Pause(0);

        // isPaused의 상태를 체크하여 timeScale의 값을 전환해준다
        if (isPaused)
        {
            Time.timeScale = 1;
        } else
        {
            Time.timeScale = 0;
        }


        for (int i = 0; i < onPauseHiddenObjects.Length; i++)
        {
            onPauseHiddenObjects[i].gameObject.SetActive(!onPauseHiddenObjects[i].activeInHierarchy);
        }

        isPaused = !isPaused;                           // isPaused의 상태를 반전
    }
}

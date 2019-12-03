using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    // 이 소스는 미개한 원시인같다 우가우가
    private int scenenumber;
    private DialogManager dm;
    private string nextScene;
    private string currentName;
    private string currentScript;
    private int currentFace;
    private int scriptProgress;
    private AudioManager audio;
    private GameObject player;
    private GameObject[] junks = new GameObject[3];
    private GameObject boyFreind;
    private GameObject windowSunset;
    public GameObject[] roaches;


    ArrayList name = new ArrayList();
    ArrayList script = new ArrayList();
    ArrayList face = new ArrayList();

    private void Awake()
    {
        dm = GameObject.Find("DiaglogPanel").GetComponent<DialogManager>();
        windowSunset = GameObject.Find("WindowSunset");
        scenenumber = PlayerPrefs.GetInt("StoryProgress");
        player = GameObject.Find("Player");
        junks[0] = GameObject.Find("Junks 1");
        junks[1] = GameObject.Find("Junks 2");
        junks[2] = GameObject.Find("Junks 3");
        boyFreind = GameObject.Find("BoyFreind");
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        ScriptDefine(scenenumber);
    }

    void Start()
    {
        windowSunset.SetActive(false);
        boyFreind.SetActive(false);
        roaches[0].SetActive(false);
        roaches[1].SetActive(false);
        roaches[2].SetActive(false);
        scriptProgress = 0;
        Next();
    }

    public void Next()
    {   
        if (scriptProgress < name.Count)
        {
            currentFace = (int)face[scriptProgress];
            currentName = name[scriptProgress].ToString();
            currentScript = script[scriptProgress].ToString();
            dm.SetCharImage(currentFace);
            dm.SetDialogText(currentName, currentScript);
            ScriptEvent(scenenumber, scriptProgress);
        } else
        {
            PlayerPrefs.SetInt("StoryProgress", ++scenenumber);
            GameObject.Find("Canvas").GetComponent<UIHandler>().SceneChanger(nextScene);
        }

        scriptProgress++;
    }

    public void ScriptEvent(int scene, int progress)
    {
        Debug.Log(progress);
        // 지정된 scene, progress 번호와 일치하는 대화문에서 이벤트 실행 
        switch (scene)
        {
            case 0:
                if (progress == 4)
                {
                    
                    player.transform.rotation = Quaternion.identity;
                }
                break;
            case 1:
                if (progress == 0)
                {
                    player.transform.rotation = Quaternion.identity;
                    player.transform.position = new Vector2(1.2f, 0.23f);
                }
                break;
            case 2:
                if (progress == 0)
                {
                    player.transform.rotation = Quaternion.identity;
                    player.transform.position = new Vector2(1.2f, 0.23f);
                }
                if (progress == 5)
                {
                    roaches[0].SetActive(true);
                    roaches[0].GetComponent<Animator>().SetFloat("dirX", 1.0f);
                }
                if (progress == 6)
                {
                    audio.PlaySE(5, 2);
                }
                break;
            case 3:
                if (progress == 0)
                {
                    junks[2].SetActive(false);
                    player.transform.rotation = Quaternion.identity;
                    player.transform.position = new Vector2(1.2f, 0.23f);
                }
                if (progress == 1)
                {
                    audio.PlaySE(6, 2);
                }
                break;
            case 4:
                if (progress == 0)
                {
                    windowSunset.SetActive(true);
                    junks[1].SetActive(false);
                    junks[2].SetActive(false);
                    player.transform.rotation = Quaternion.identity;
                    player.transform.position = new Vector2(1.2f, 0.23f);
                }
                if (progress == 4)
                {
                    audio.PlaySE(7, 2);
                }
                if (progress == 6)
                {
                    audio.PlaySE(7, 2);
                }
                break;
            case 5:
                if (progress == 0)
                {
                    windowSunset.SetActive(true);
                    junks[1].SetActive(false);
                    junks[2].SetActive(false);
                    player.transform.rotation = Quaternion.identity;
                    player.transform.position = new Vector2(1.2f, 0.23f);
                }
                if (progress == 1)
                {
                    roaches[0].SetActive(true);
                    roaches[0].GetComponent<Animator>().SetFloat("dirX", 1.0f);
                    roaches[1].SetActive(true);
                    roaches[1].GetComponent<Animator>().SetFloat("dirX", 1.0f);
                    roaches[2].SetActive(true);
                    roaches[2].GetComponent<Animator>().SetFloat("dirX", 1.0f);
                }
                if (progress == 2)
                {
                    audio.PlaySE(5, 2);
                }
                if (progress == 3)
                {
                    audio.PlaySE(7, 2);
                }
                if (progress == 4)
                {
                    audio.PlaySE(6, 2);
                }
                break;
            case 6:
                if (progress == 0)
                {
                    windowSunset.SetActive(true);
                    junks[0].SetActive(false);
                    junks[1].SetActive(false);
                    junks[2].SetActive(false);
                    player.transform.rotation = Quaternion.identity;
                    player.transform.position = new Vector2(1.2f, 0.23f);
                }
                if (progress == 5)
                {
                    boyFreind.SetActive(true);
                }
                break;
            default:
                return;
        }
    }

    public void ScriptDefine(int scene)
    {
        // 답이 없다
        switch (scene)
        {
            case 0:
                nextScene = "StageSelect";

                // 0
                face.Add(3);
                name.Add("알람시계");
                script.Add("따르릉~");

                // 1
                face.Add(0);
                name.Add("주인공");
                script.Add("하아암... 으음, 5분만 더...");

                // 2
                face.Add(3);
                name.Add("알람시계");
                script.Add("따르르르릉!!");

                // 3
                face.Add(0);
                name.Add("주인공");
                script.Add("으으... 대체 몇 시지?");

                // 4
                face.Add(0);
                name.Add("주인공");
                script.Add("헉... 벌써 오후잖아! 오늘 저녁에 남자친구가 오기로 했는데!");

                // 5
                face.Add(0);
                name.Add("주인공");
                script.Add("남자친구가 오기 전까지 빨리 방을 치워야 해!");
                break;
            case 1:
                nextScene = "GamePlay";

                // 0
                face.Add(0);
                name.Add("주인공");
                script.Add("으악! 방이 완전 쓰레기장이잖아!");

                // 1
                face.Add(0);
                name.Add("주인공");
                script.Add("요즘 너무 피곤해서 게을러졌더니 이런 사태가.......");

                // 2
                face.Add(0);
                name.Add("주인공");
                script.Add("안되겠어, 남자친구가 오기 전에 방을 잘 치워야 해!");

                // 3
                face.Add(0);
                name.Add("주인공");
                script.Add("하지만 물건들이 전부 섞여있으니 잘 분류해야겠지?");

                // 4
                face.Add(0);
                name.Add("주인공");
                script.Add("[쓰레기]는 [쓰레기통]에, [음식]은 [냉장고]에, [의류]는 [옷장]에 잘 정리해야겠다...!");
                break;
            case 2:
                nextScene = "GamePlay";

                // 0
                face.Add(0);
                name.Add("주인공");
                script.Add("으으, 치워도 치워도 끝이 안 나......");

                // 1
                face.Add(3);
                name.Add("???");
                script.Add("(사각사각)");

                // 2
                face.Add(0);
                name.Add("주인공");
                script.Add("응? 무슨 소리지?");

                // 3
                face.Add(3);
                name.Add("???");
                script.Add("(사각사각)");

                // 4
                face.Add(3);
                name.Add("???");
                script.Add("(사각사각사각)");

                // 5
                face.Add(3);
                name.Add("바선생");
                script.Add("(뿅!)");

                // 6
                face.Add(0);
                name.Add("주인공");
                script.Add("으아아아아아아아아아아아아아아아아악!!!!!\n바퀴벌레다!!!!!!!!!!!!!!!!!!!!!");

                // 7
                face.Add(0);
                name.Add("주인공");
                script.Add("안 돼, 이리로 오지 마!!\n저걸 밟으면 [기절]해버릴지도 몰라!!");

                // 8
                face.Add(3);
                name.Add("도움말");
                script.Add("바퀴벌레와 접촉할 경우 일정 시간동안 움직일 수 없게 됩니다.\n잘 피하며 방을 정리합시다!");
                break;

            case 3:
                nextScene = "GamePlay";

                // 0
                face.Add(0);
                name.Add("주인공");
                script.Add("헉, 헉... 바퀴벌레가... 어디로 간 거지...?");

                // 1
                face.Add(3);
                name.Add("???");
                script.Add("(따르르릉~)");

                // 2
                face.Add(0);
                name.Add("주인공");
                script.Add("앗, 이건 휴대폰 진동소리? 문자가 왔나보네. 확인해보자.");

                // 3
                face.Add(0);
                name.Add("주인공");
                script.Add("뭐야, 스팸이잖아... 앗! 그러고 보니 오늘 면접 결과가 나오기로 했었지!");

                // 4
                face.Add(0);
                name.Add("주인공");
                script.Add("언제 연락이 올 지 모르니 주의하고 있어야겠다...!");

                // 5
                face.Add(3);
                name.Add("도움말");
                script.Add("랜덤하게 침대 위 휴대전화가 흔들립니다.\n전화를 받을 경우 피버 게이지가 증가하고, 받지 못할 경우 감소합니다.");
                break;
            case 4:
                nextScene = "GamePlay";

                // 0
                face.Add(0);
                name.Add("주인공");
                script.Add("휴... 대충 끝이 보이기 시작하네. 앞으로 조금만 더 하면...");

                // 1
                face.Add(3);
                name.Add("???");
                script.Add("(띵동)");

                // 2
                face.Add(3);
                name.Add("???");
                script.Add("(띵동) (띵동) (띵동)");

                // 3
                face.Add(0);
                name.Add("주인공");
                script.Add("응? 누구지? 혹시 택배....?!(두근두근)");

                // 4
                face.Add(2);
                name.Add("집주인");
                script.Add("학생!! 방에 있지?! 나 집주인 아줌만데!!");

                // 5
                face.Add(0);
                name.Add("주인공");
                script.Add("헉... 이 목소리는...!");

                // 6
                face.Add(2);
                name.Add("집주인");
                script.Add("학생이 밀린 월세로 빌딩도 사겠어!! 빨리 좀 나와봐!!!");

                // 7
                face.Add(0);
                name.Add("주인공");
                script.Add("아... 안 돼, 평소처럼 집에 없는 척 [발소리도 내지 말고] 조용히 있어야겠다...");

                // 8
                face.Add(3);
                name.Add("도움말");
                script.Add("집주인이 찾아오면 화면 색상이 [노란색]과 [붉은색]으로 변합니다.\n패널티를 받지 않도록, 집주인이 돌아갈 때까지 움직이지 않도록 합시다.");
                break;
            case 5:
                nextScene = "GamePlay";

                // 0
                face.Add(0);
                name.Add("주인공");
                script.Add("휴... 드디어 포기하셨나봐. 이제 마무리만 지으면...");

                // 1
                face.Add(3);
                name.Add("바선생들");
                script.Add("(뿅!) (뿅!) (뿅!)");

                // 2
                face.Add(0);
                name.Add("주인공");
                script.Add("끼아아아아아아아아아아아아악!!!!!!!");

                // 3
                face.Add(2);
                name.Add("집주인");
                script.Add("학생!! 방금 그 소리 학생이지?! 역시 집에 있었네!!!");

                // 4
                face.Add(3);
                name.Add("휴대전화");
                script.Add("(우웅~) (우웅~) (우웅~)");

                // 5
                face.Add(0);
                name.Add("주인공");
                script.Add("아아악, 정신없어~!!!");
                break;
            case 6:
                nextScene = "StageSelect";

                // 0
                face.Add(0);
                name.Add("주인공");
                script.Add("헉, 헉... 드디어 다 끝냈... 아니, 벌써 저녁이잖아?!");

                // 1
                face.Add(3);
                name.Add("???");
                script.Add("(띵동)");

                // 2
                face.Add(0);
                name.Add("주인공");
                script.Add("서, 설마 또 집주인 아줌마가...?!");

                // 3
                face.Add(1);
                name.Add("???");
                script.Add("나야~ 집에 있어?");

                // 4
                face.Add(0);
                name.Add("주인공");
                script.Add("헉, 이 목소리는... 남자친구?!");

                // 5
                face.Add(1);
                name.Add("남자친구");
                script.Add("보고싶었어~");

                // 6
                face.Add(0);
                name.Add("주인공");
                script.Add("나도~~!!");

                // 7
                face.Add(1);
                name.Add("남자친구");
                script.Add("와, 근데 집이 엄청 깨끗하다. 역시 깔끔하구나!");

                // 8
                face.Add(0);
                name.Add("주인공");
                script.Add("하... 하하...... (땀)");

                // 9
                face.Add(0);
                name.Add("주인공");
                script.Add("그, 그것보다 나 배고파... 밥 먹으러 가자!");

                // 10
                face.Add(1);
                name.Add("남자친구");
                script.Add("그래~");

                // 11
                face.Add(0);
                name.Add("주인공");
                script.Add("(휴... 한때는 어떻게 되는 줄 알았는데\n오늘도 무사히 넘어갔구나.\n다행이다~)");

                // 12
                face.Add(3);
                name.Add("");
                script.Add("-Fin-");
                break;
        }
    }
}

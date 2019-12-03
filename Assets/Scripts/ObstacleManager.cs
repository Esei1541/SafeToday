using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObstacleManager : MonoBehaviour
{
    [HideInInspector] public bool isCalling;

    public static bool isOwnerCome;

    public TextMeshProUGUI phoneText;
    public TextMeshProUGUI bonusText;
    public GameObject owner;

    private float time;
    private Animator phoneAni;
    private float callTime;
    private GameObject player;
    private PlayerManager pm;
    private Camera cam;
    private AudioManager aud;
    private GameManager gm;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        aud = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        player = GameObject.Find("Player");
        phoneAni = GameObject.Find("Phone").GetComponent<Animator>();
        pm = player.GetComponent<PlayerManager>();
    }

    void Start()
    {
        time = 0;
        callTime = 0;
        isCalling = false;
        isOwnerCome = false;
        if (GameManager.obstacleType[0] == 0)
        {
            GameObject.Find("Phone").SetActive(true);
        } else
        {
            GameObject.Find("Phone").SetActive(false);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        //Debug.Log("Time: " + time);
        //Debug.Log("CallTime: " + callTime);
        if (time >= GameManager.obstacleTiming)
        {
            int type = Random.Range(GameManager.obstacleType[0], GameManager.obstacleType[1]);
            Debug.Log(type);
            switch(type)
            {
                case 0:
                    StartCoroutine(PhoneCalling(1.0f));
                    break;
                case 1:
                    StartCoroutine(OwnerCome(5.0f));
                    break;
                default:
                    break;
            }
            time = 0;
        }
    }

    public void CallPhone()
    {
        // 외부에서 호출하여 PhoneCalling 코루틴을 실행해주는 메서드
        StartCoroutine(PhoneCalling(1.0f));
    }

    public void CallOwner()
    {
        StartCoroutine(OwnerCome(5.0f));
    }

    IEnumerator PhoneCalling(float time)
    {
        isCalling = true;
        aud.SetLoop(2, true);
        aud.PlaySE(6, 2);
        while (true)
        {
            callTime += Time.deltaTime;
            phoneAni.SetBool("isCalling", true);
            //Debug.Log(callTime);

            if (!isCalling)
            {
                // 플레이어 조작에 의한 종료
                aud.PlaySE(0);
                FeverManager.FeverIncreaseRaw(20.0f);
                // StartCoroutine(UIHandler.popSlideText(bonusText));
                Debug.Log("Success");
                break;
            }
            if (callTime >= time)
            {
                // 시간 초과에 의한 종료
                aud.PlaySE(4);
                FeverManager.FeverIncreaseRaw(-30.0f);
                Debug.Log("Failed");
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }

        aud.PauseChannel(2);
        aud.SetLoop(2, false);
        Debug.Log("call stop");
        isCalling = false;
        phoneAni.SetBool("isCalling", false);
        callTime = 0;
        yield break;
    }
    


    IEnumerator OwnerCome(float time)
    {
        Color32 currentColor = cam.backgroundColor;
        bool isViberate = false;
        isOwnerCome = true;
        pm.isImmortal = true;
        cam.backgroundColor = new Color32(255, 255, 0, 255);
        
        yield return new WaitForSeconds(1.5f);
        cam.backgroundColor = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.5f);

        aud.SetLoop(2, true);
        aud.PlaySE(7, 2);

        Vector2 playerLocation = new Vector2(player.transform.position.x, player.transform.position.y);

        while (true)
        {
            
            callTime += Time.deltaTime;

            if (System.Math.Truncate(player.transform.position.x * 10) / 10 != System.Math.Truncate(playerLocation.x * 10) / 10 ||
                System.Math.Truncate(player.transform.position.y * 10) / 10 != System.Math.Truncate(playerLocation.y * 10) / 10 && !pm.isStun) 
                // 플레이어가 움직였을 경우를 체크, 물리엔진 상 고정된 오브젝트에 미세하게 밀리는 현상이 있으므로 소숫점 셋째 자리 이하는 버린다.
            {
                aud.PauseChannel(2);
                aud.SetLoop(2, false);
                aud.PlaySE(4);
                pm.isStun = true;
                FeverManager.FeverIncreaseRaw(-30);
                owner.SetActive(true);
                player.transform.position = new Vector2(5.88f, 2.15f);


                for (int i = 0; i < 2.0f / 0.1f; i++)     // stunTime동안 STUNRATE의 간격으로 좌우로 진동하는 연출을 넣어준다.
                {
                    pm.moveVector.x = 0;
                    pm.moveVector.y = 0.1f;

                    if (isViberate)
                    {
                        player.transform.position = new Vector2(player.transform.position.x - 0.1f, player.transform.position.y);
                        isViberate = false;
                    }
                    else
                    {
                        player.transform.position = new Vector2(player.transform.position.x + 0.1f, player.transform.position.y);
                        isViberate = true;
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                if (!isViberate)                                 // position값이 변동된 상태에서 for문이 종료되었을 경우 위치 오차를 교정
                {
                    player.transform.position = new Vector2(player.transform.position.x + 0.1f, player.transform.position.y);
                    isViberate = false;
                }
                break;
            }
            if (callTime >= time)                                // 시간이 종료되었을 경우를 체크
            {
                FeverManager.FeverIncreaseRaw(40);
                aud.PauseChannel(2);
                aud.SetLoop(2, false);
                Debug.Log("She is Gone...");
                break;
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }

        player.transform.position = playerLocation;
        owner.SetActive(false);
        pm.isStun = false;
        pm.isImmortal = false;
        if (FeverManager.isFever)
        {
            cam.backgroundColor = currentColor;
        } else
        {
            cam.backgroundColor = new Color32(0, 0, 0, 255);
        }
        callTime = 0;
        isOwnerCome = false;
        yield break;
    }
}

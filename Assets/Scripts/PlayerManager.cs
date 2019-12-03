using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    public static int junkType;
    [HideInInspector] public bool isStun;
    [HideInInspector] public bool isImmortal;

    private const float STUNRATE = 0.1f;
    private bool isGetJunk;
    private bool isJunkMatch;
    private ObstacleManager om;
    private GameObject gm;
    private SpriteRenderer sr;
    private AudioManager audio;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponent<SpriteRenderer>();
        om = GameObject.Find("GameManager").GetComponent<ObstacleManager>();
        gm = GameObject.Find("GameManager");
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    protected override void Start()
    {
        isImmortal = false;
        isStun = false;
        isGetJunk = false;
        junkType = -1;
        base.Start();
    }

    protected override void Update()
    {
        // CharacterManager 클래스의 Update를 재정의
        DropJunk();
        GetInputAxis();                                 // GetInputAxis 메서드로 키 값을 입력받아서
        base.Update();                                  // = CharacterManager.Move();

    }

    private void GetInputAxis()
    {
        // 상하좌우 키 값을 입력받는 메서드

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        isMoving = ((xInput != 0 || yInput != 0) && !isStun);        // x나 y값 입력이 있고, 스턴 상태가 아니라면 isMoving을 true로 바꿔준다.

        if (isMoving)                                   // x나 y값 입력이 있을 경우
        {
            moveVector = new Vector2(xInput, yInput);   // 부모 클래스에서 선언해둔 moveVector에 x, y 입력값을 전달
        }

        // 해당 메서드에서 입력받은 moveVector의 x, y 값으로 부모 클래스의 Move() 메서드에서 이동을 구현한다. 
    }

    public override void AnimateMovement()
    {
        base.AnimateMovement();
        if (isGetJunk)
        {
            animator.SetBool("isGetJunk", true);
        } else
        {
            animator.SetBool("isGetJunk", false);
        }
    }

    private void GetJunk()
    {
        junkType = Random.Range(0, 3);
        Debug.Log("Get Junk type " + junkType);
        audio.PlaySE(2);
        isGetJunk = true;
    }

    private void PutJunk(int type)
    {
        Debug.Log("Put Junk");
        if (type == junkType)
        {
            audio.PlaySE(0);
            FeverManager.FeverIncrease(3);
            gm.GetComponent<GameManager>().ScoreIncrease(1000);
            isGetJunk = false;
        } else
        {
            audio.PlaySE(4);
            gm.GetComponent<GameManager>().ScoreIncreaseRaw(-500);
            gm.GetComponent<GameManager>().ComboReset();
            FeverManager.FeverIncreaseRaw(-5);
            isGetJunk = false;
        }
    }

    private bool CheckJunkMatch(int type)
    {
        if (type == junkType)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void DropJunk()
    {
        if (Input.GetAxisRaw("Cancel") != 0)
        {
            audio.PlaySE(3);
            Debug.Log("Drop Junk");
            isGetJunk = false;
        }
    }

    IEnumerator Stun(float stunTime)
    {
        // 일시적으로 스턴 상태를 부여하는 코루틴

        bool isVisible = true;
        bool isViberate = false;
        isStun = true;                              // 조작 관련 메서드에서 isStun을 체크하여 조작이 불가능하게 설정할 것
        isImmortal = true;                          // 무한 스턴에 걸리지 않도록 스턴 종료 후 일정 시간동안 무적 상태를 설정
        audio.PlaySE(5);

        sr.color = new Color(0.6f, 0.6f, 1.0f, 1.0f);     // 스턴 시간동안 캐릭터의 색을 파란색 톤으로 변경

        for (int i = 0; i < stunTime / STUNRATE; i++)     // stunTime동안 STUNRATE의 간격으로 좌우로 진동하는 연출을 넣어준다.
        {
            if (isViberate)
            {
                this.transform.position = new Vector2(this.transform.position.x - 0.1f, this.transform.position.y);
                isViberate = false;
            } else
            {
                this.transform.position = new Vector2(this.transform.position.x + 0.1f, this.transform.position.y);
                isViberate = true;
            }
            yield return new WaitForSeconds(STUNRATE);
        }
        if (!isViberate)                                 // position값이 변동된 상태에서 for문이 종료되었을 경우 위치 오차를 교정
        {
            this.transform.position = new Vector2(this.transform.position.x + 0.1f, this.transform.position.y);
            isViberate = false;
        }

        isStun = false;

        for (int i = 0; i < 12; i++)
        {
            // 캐릭터를 깜빡이게 해준다
            if (isVisible)
            {
                sr.color = new Color(1.0f, 1.0f, 1.0f, 0f);
                isVisible = false;
            } else
            {
                sr.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                isVisible = true;
            }
            yield return new WaitForSeconds(0.1f);
        }

        sr.color = new Color(1.0f, 1.0f, 1.0f, 1f);
        isVisible = true;
        isImmortal = false;

        yield break;                    // 1회 실행 후 코루틴 종료
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Roach" && !isImmortal)
        {
            StartCoroutine(Stun(2.0f));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetAxisRaw("Submit") != 0)
        {
            if (other.transform.tag == "Phone" && om.isCalling)
            {
                om.isCalling = false;
            }

            if (other.transform.tag == "Junk" && isGetJunk == false)
            {
                GetJunk();
            }
            if (isGetJunk == true)
            {
                switch (other.transform.tag)
                {
                    case "Throw0":
                        PutJunk(0);
                        break;
                    case "Throw1":
                        PutJunk(1);
                        break;
                    case "Throw2":
                        PutJunk(2);
                        break;
                    default:
                        break;
                }
            }
        }

    }

}

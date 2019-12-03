using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterManager : MonoBehaviour
{
    /* 이동 등 오브젝트의 전반적인 내용을 정의하는 추상 클래스. 상속하여 씁시다. */
    [SerializeField]                                        // [SerializeField]가 붙으면 접근제한자가 private라도 인스펙터에서 값을 줄 수 있다. 
    private float speed = 1;                                // 이동 속도를 정의
    protected Rigidbody2D rg2d;
    public Vector2 moveVector;                           
    protected bool isMoving;
    protected float xInput, yInput;
    protected Animator animator;

    // protected Vector2 direction;

    protected virtual void Awake()
    {
        rg2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        isMoving = false;
    }

    protected virtual void Update()                     // virtual은 상속받은 클래스에서 재정의할 것이라고 선언해두는 것
    {
        if (Time.timeScale != 0)                        // 게임이 멈춰있지 않을 때만 실행
        {
            Move();
            AnimateMovement();                  
        }
    }

    /* 이동하는 동작을 정의하는 메서드  */
    public void Move()
    {
        // 이렇게 하면 벽에 박아도 꿈틀대지 않는 훌륭한 움직임을 구현 가능
        if (isMoving)                                   // isMoving은 일종의 움직인다는 신호로, 자식 객체에서 특정 조건 하에 true로 전환해준다.(플레이어 캐릭터의 경우 키 값을 입력받았을 때 등)
        {
            rg2d.MovePosition(new Vector2((transform.position.x + moveVector.x * speed * Time.deltaTime),       
            (transform.position.y + moveVector.y * speed * Time.deltaTime)));                                 // 현재 position 값에 자식 객체에서 받아온 x, y값만큼 이동을 해주는 개념.
        }
    }

    /* 캐릭터의 애니메이션을 정의하는 메서드 */
    public virtual void AnimateMovement()
    {
        animator.SetFloat("dirX", moveVector.x);        // 자식 객체에서 받아온 moveVector의 x, y값으로 애니메이터의 dirX, dirY 패러미터를 설정해준다.
        animator.SetFloat("dirY", moveVector.y);
        if (PlayerManager.junkType >= 0)
        {
            animator.SetInteger("JunkCategory", PlayerManager.junkType);
        }
        if (isMoving)                                   // isMoving이 true일 경우 isWalking 패러미터를 활성화시켜 걷는 모션을 실행
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }
    }
}

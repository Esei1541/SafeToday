using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoachManager : CharacterManager
{
    private int direction;

    public float delay;
    public float moveRange;

    protected override void Start()
    {
        base.Start();
        StartCoroutine("RandMove");
    }

    protected override void Update()
    {
        base.Update();
    }

    IEnumerator RandMove()
    {
        // 랜덤으로 설정한 방향으로 1초간 이동 후 delay만큼 대기하는 코루틴
        direction = 0;
        while (true)
        {
            direction = Random.Range(1, 5);

            switch (direction)
            {
                case 1:                 // 상
                    xInput = 0.0f;
                    yInput = moveRange;
                    break;
                case 2:                 // 하
                    xInput = 0.0f;
                    yInput = moveRange * -1;
                    break;
                case 3:                 // 좌
                    xInput = moveRange * -1;
                    yInput = 0.0f;
                    break;
                case 4:                 // 우
                    xInput = moveRange;
                    yInput = 0.0f;
                    break;
                default:
                    break;
            }

            moveVector = new Vector2(xInput, yInput);
            isMoving = true;

            yield return new WaitForSeconds(1);

            isMoving = false;

            yield return new WaitForSeconds(delay);
        }
        
    }

    IEnumerator MoveBack()
    {
        // 현재 진행중인 방향에서 반대 방향으로 1초간 이동하는 코루틴
        StopCoroutine("RandMove");

        isMoving = false;

        xInput = xInput * -1;
        yInput = yInput * -1;

        moveVector = new Vector2(xInput, yInput);
        isMoving = true;

        yield return new WaitForSeconds(1);

        isMoving = false;

        yield return new WaitForSeconds(delay);

        StartCoroutine("RandMove");
        yield break;  // 1회 실행 후 코루틴 종료
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "RoachRange")
        {
            // RochRange Tag로 경계를 지정해준 영역에 들어갈 시 MoveBack 코루틴을 실행
            StartCoroutine("MoveBack");
        }
    }
}

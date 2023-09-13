using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 이동 속도
    public float speed = 5;
    // CharacterController 컴포넌트
    public CharacterController cc;

    // 중력 가속도의 크기
    public float gravity = -20;
    // 수직 속도
    float yVelocity = 0;

    // 점프 크기
    public float jumpPower = 5;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 사용자의 입력에 따라 전후좌우로 이동
        // 사용자 입력 받음
        float h = VRInput.GetAxis("Horizontal");
        float v = VRInput.GetAxis("Vertical");

        // 방향설정
        Vector3 dir = new Vector3(h, 0, v);
        // 사용자가 바라보는 방향으로 입력 값 변화
        dir = Camera.main.transform.TransformDirection(dir);

        // 중력을 적용한 수직 방향 추가 v=v0+at
        yVelocity += gravity * Time.deltaTime;
        // 바닥에 있을 경우, 수직 항력 속도를 0으로 한다.
        if (cc.isGrounded)
        {
            yVelocity = 0;
        }
        // 점프
        if (VRInput.GetDown(VRInput.Button.Two, VRInput.Controller.RTouch))
        {
            yVelocity = jumpPower;
        }

        // 중력발생
        dir.y = yVelocity;

        // 이동
        cc.Move(dir * speed * Time.deltaTime);
    }
}

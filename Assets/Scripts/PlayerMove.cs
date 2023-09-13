using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // �̵� �ӵ�
    public float speed = 5;
    // CharacterController ������Ʈ
    public CharacterController cc;

    // �߷� ���ӵ��� ũ��
    public float gravity = -20;
    // ���� �ӵ�
    float yVelocity = 0;

    // ���� ũ��
    public float jumpPower = 5;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // ������� �Է¿� ���� �����¿�� �̵�
        // ����� �Է� ����
        float h = VRInput.GetAxis("Horizontal");
        float v = VRInput.GetAxis("Vertical");

        // ���⼳��
        Vector3 dir = new Vector3(h, 0, v);
        // ����ڰ� �ٶ󺸴� �������� �Է� �� ��ȭ
        dir = Camera.main.transform.TransformDirection(dir);

        // �߷��� ������ ���� ���� �߰� v=v0+at
        yVelocity += gravity * Time.deltaTime;
        // �ٴڿ� ���� ���, ���� �׷� �ӵ��� 0���� �Ѵ�.
        if (cc.isGrounded)
        {
            yVelocity = 0;
        }
        // ����
        if (VRInput.GetDown(VRInput.Button.Two, VRInput.Controller.RTouch))
        {
            yVelocity = jumpPower;
        }

        // �߷¹߻�
        dir.y = yVelocity;

        // �̵�
        cc.Move(dir * speed * Time.deltaTime);
    }
}

#define Remote
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마우스 입력에 따라 카메라를 회전 시키고 싶다.
// 필요속성 : 현재각도, 마우스감도
public class CamRotate : MonoBehaviour
{
    // 현재각도
    Vector3 angle;
    // 마우스감도
    public float sensitivity = 200;

    void Start()
    {
        // 시작할때 현재 카메라의 각도 적용
        angle.y = -transform.eulerAngles.x;
        angle.x = transform.eulerAngles.y;
        angle.z = transform.eulerAngles.z;
    }

    void Update()
    {
        // 마우스의 좌우 입력을 받는다.
        //float x = Input.GetAxis("Mouse X");
        //float y = Input.GetAxis("Mouse Y");

        if (VRInput.GetDown(VRInput.Button.One, VRInput.Controller.RTouch))
        {
            angle.x = 0;
            angle.y = 0;

            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        float x = VRInput.GetRotAxis("Horizontal");
        float y = VRInput.GetRotAxis("Vertical");

        // 이동 공식에 대입하여 각 속성별로 회전 값 누적
        angle.x += x * sensitivity * Time.deltaTime;
        angle.y += y * sensitivity * Time.deltaTime;

        angle.y = Mathf.Clamp(angle.y, -90, 90);

        // 카메라의 회전값에 새로 만들어진 회전 값을 할당
        transform.eulerAngles = new Vector3(-angle.y, angle.x, transform.eulerAngles.z);


    }

}

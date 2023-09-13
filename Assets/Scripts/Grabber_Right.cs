using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber_Right : MonoBehaviour
{
    // 물체를 잡고 있는지의 여부
    bool isGrabbing = false;
    // 잡고 있는 물체
    public GameObject grabbedObject;
    // 잡을 물체의 종류
    public LayerMask grabbedLayer;
    // 잡을 수 있는 거리
    public float grabRange = 1.0f;

    // 이전 위치
    Vector3 prevPos;
    // 던질 힘
    public float throwPower = 10f;

    // 이전 회전
    Quaternion prevRot;
    // 회전력
    public float rotPower = 2f;

    // 원거리 물체 잡기 거리
    public float remoteGrabDistance = 2;

    bool isCanGrab;

    public Transform crosshair; // 크로스헤어를 위한 속성

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        VRInput.DrawCrosshair(crosshair, true, VRInput.Controller.RTouch);

        // 물체를 잡지 않고 있을 경우
        if (isGrabbing == false)
        {
            // 물체 잡기
            TryGrab();
        }
        else
        {
            // 물체 놓기
            TryUngrab();
        }
    }

    private void TryGrab()
    {
        if (VRInput.GetDown(VRInput.Button.HandTrigger, VRInput.Controller.RTouch))
        {
            // 손 방향으로 Ray 제작
            Ray ray = new Ray(VRInput.RHandPosition, VRInput.RHandDirection);
            RaycastHit hitInfo;

            // SphereCast를 이용해 물체 충돌을 체크
            if (Physics.SphereCast(ray, 0.25f, out hitInfo, remoteGrabDistance, grabbedLayer))
            {

                // 잡은 상태로 전환
                isGrabbing = true;

                // 잡은 물체에 대한 기억
                grabbedObject = hitInfo.transform.gameObject;

                // 물체가 끌려오는 기능 실행
                StartCoroutine(GrabbingAnimation());
            }

            return;

        }
    }

    private void TryUngrab()
    {

        // 던질 방향
        Vector3 throwDirection = (VRInput.RHandPosition - prevPos);
        Vector3 throwDirection2 = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        // 위치 기억
        prevPos = VRInput.RHandPosition;

        // 회전 방향 = current - previous
        Quaternion deltaRotation = VRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        // 회전 기억
        prevRot = VRInput.RHand.rotation;


        // 버튼을 놓았다면
        if (VRInput.GetUp(VRInput.Button.HandTrigger, VRInput.Controller.RTouch))
        {
            // 잡지 않은 상태로 전환
            isGrabbing = false;
            // 물리 기능 활성화
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            // 손에서 오브젝트 제거
            grabbedObject.transform.parent = null;

            // 던지기
            grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

            // 각속도 적용
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity * rotPower;

            // 잡은 물체가 없도록 설정
            grabbedObject = null;
        }
    }

    IEnumerator GrabbingAnimation()
    {
        // 물리 기능 정지
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        // 초기 위치 값 지정
        prevPos = VRInput.RHandPosition;
        // 초기 회전 값 지정
        prevRot = VRInput.RHand.rotation;

        Vector3 startLocation = grabbedObject.transform.position;
        Vector3 targetLocation = VRInput.RHandPosition + VRInput.RHandDirection * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;

        // 경과율
        float elapsedRate = currentTime / finishTime;

        while (elapsedRate < 1)
        {
            if (grabbedObject == null) break;

            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;

            grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);

            yield return null;
        }

        if (grabbedObject != null)
        {
            // 잡은 물체를 손의 자식으로 등록
            grabbedObject.transform.position = VRInput.RHandPosition;
            grabbedObject.transform.parent = VRInput.RHand;
        }

    }
}

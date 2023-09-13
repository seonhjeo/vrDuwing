using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    // 물체를 잡고 있는지의 여부
    bool isGrabbing = false;
    // 잡고 있는 물체
    public GameObject grabbedObject;
    // 잡을 물체의 종류
    public LayerMask grabbedLayer;
    // 잡을 수 있는 거리
    public float grabRange = 0.5f;

    // 이전 위치
    Vector3 prevPos;
    // 던질 힘
    public float throwPower = 10;

    // 이전 회전
    Quaternion prevRot;
    // 회전력
    public float rotPower = 10;

    // 원거리 물체 잡기
    //public bool isRemoteGrab = true;
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
        //크로스헤어
        VRInput.DrawCrosshair(crosshair);

        // 물체를 잡지 않고 있을 경우
        if (isGrabbing == false)
        {
            // 잡기 시도
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
            
            // 원거리 물체 잡기
            //if (isRemoteGrab)
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

            /*
            // 영역 안 오브젝트 검출
            Collider[] hitObjects = Physics.OverlapSphere(VRInput.RHandPosition, grabRange, grabbedLayer);
            // 가장 가까운 오브젝트 인덱스
            int closest = 0;

            // 손과 가장 가까운 물체 선택
            for (int i = 1; i < hitObjects.Length; i++)
            {
                // 손과 가장 가까운 물체와의 거리
                Vector3 closestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(closestPos, VRInput.RHandPosition);

                // 다음 물체와 손의 거리
                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, VRInput.RHandPosition);

                // 거리 비교 후 인덱스 교체
                if (nextDistance < closestDistance)
                {
                    closest = i;
                }
            }

            // 검출된 물체가 있을 경우
            if (hitObjects.Length > 0)
            {
                // 잡은 상태로 전환
                isGrabbing = true;
                // 잡은 물체에 대한 기억
                grabbedObject = hitObjects[closest].gameObject;
                // 잡은 물체를 손의 자식으로 등록
                grabbedObject.transform.parent = VRInput.RHand;
                // 물리 기능 정지
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                // 초기 위치 값
                prevPos = VRInput.RHandPosition;
                // 초기 회전 값
                prevRot = VRInput.RHand.rotation;
            }
            */
        }
    }

    private void TryUngrab()
    {
        
        // 던질 방향
        Vector3 throwDirection = (VRInput.RHandPosition - prevPos);
        Vector3 throwDirection2= OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
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
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = rotPower * angularVelocity / 100f;

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

    void Grabbing()
    {
        // 초기 위치 값 지정
        prevPos = VRInput.RHandPosition;
        // 초기 회전 값 지정
        prevRot = VRInput.RHand.rotation;

        if (isCanGrab==true&& grabbedObject!=null)
        {
            Vector3 startLocation = grabbedObject.transform.position;
            Vector3 targetLocation = VRInput.RHandPosition + VRInput.RHandDirection * 0.1f;

            if (Vector3.Distance(grabbedObject.transform.position, targetLocation)>0.1f)
            {
                grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, 0.1f);
            }
            else if(Vector3.Distance(grabbedObject.transform.position, targetLocation) <= 0.1f)
            {
                grabbedObject.transform.position = targetLocation;
            }

            // 물리 기능 정지, 부모등록
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.transform.parent = VRInput.RHand;
        }

    }
}

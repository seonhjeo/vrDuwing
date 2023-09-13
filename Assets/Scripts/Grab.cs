using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    // ��ü�� ��� �ִ����� ����
    bool isGrabbing = false;
    // ��� �ִ� ��ü
    public GameObject grabbedObject;
    // ���� ��ü�� ����
    public LayerMask grabbedLayer;
    // ���� �� �ִ� �Ÿ�
    public float grabRange = 0.5f;

    // ���� ��ġ
    Vector3 prevPos;
    // ���� ��
    public float throwPower = 10;

    // ���� ȸ��
    Quaternion prevRot;
    // ȸ����
    public float rotPower = 10;

    // ���Ÿ� ��ü ���
    //public bool isRemoteGrab = true;
    // ���Ÿ� ��ü ��� �Ÿ�
    public float remoteGrabDistance = 2;

    bool isCanGrab;

    public Transform crosshair; // ũ�ν��� ���� �Ӽ�

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //ũ�ν����
        VRInput.DrawCrosshair(crosshair);

        // ��ü�� ���� �ʰ� ���� ���
        if (isGrabbing == false)
        {
            // ��� �õ�
            TryGrab();
        }
        else
        {
            // ��ü ����
            TryUngrab();
        }
    }

    private void TryGrab()
    {
        if (VRInput.GetDown(VRInput.Button.HandTrigger, VRInput.Controller.RTouch))
        {
            
            // ���Ÿ� ��ü ���
            //if (isRemoteGrab)
            {

                // �� �������� Ray ����
                Ray ray = new Ray(VRInput.RHandPosition, VRInput.RHandDirection);
                RaycastHit hitInfo;

                // SphereCast�� �̿��� ��ü �浹�� üũ
                if (Physics.SphereCast(ray, 0.25f, out hitInfo, remoteGrabDistance, grabbedLayer))
                {
                    // ���� ���·� ��ȯ
                    isGrabbing = true;

                    // ���� ��ü�� ���� ���
                    grabbedObject = hitInfo.transform.gameObject;

                    // ��ü�� �������� ��� ����
                    StartCoroutine(GrabbingAnimation());
                }
                return;
            }

            /*
            // ���� �� ������Ʈ ����
            Collider[] hitObjects = Physics.OverlapSphere(VRInput.RHandPosition, grabRange, grabbedLayer);
            // ���� ����� ������Ʈ �ε���
            int closest = 0;

            // �հ� ���� ����� ��ü ����
            for (int i = 1; i < hitObjects.Length; i++)
            {
                // �հ� ���� ����� ��ü���� �Ÿ�
                Vector3 closestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(closestPos, VRInput.RHandPosition);

                // ���� ��ü�� ���� �Ÿ�
                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, VRInput.RHandPosition);

                // �Ÿ� �� �� �ε��� ��ü
                if (nextDistance < closestDistance)
                {
                    closest = i;
                }
            }

            // ����� ��ü�� ���� ���
            if (hitObjects.Length > 0)
            {
                // ���� ���·� ��ȯ
                isGrabbing = true;
                // ���� ��ü�� ���� ���
                grabbedObject = hitObjects[closest].gameObject;
                // ���� ��ü�� ���� �ڽ����� ���
                grabbedObject.transform.parent = VRInput.RHand;
                // ���� ��� ����
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;

                // �ʱ� ��ġ ��
                prevPos = VRInput.RHandPosition;
                // �ʱ� ȸ�� ��
                prevRot = VRInput.RHand.rotation;
            }
            */
        }
    }

    private void TryUngrab()
    {
        
        // ���� ����
        Vector3 throwDirection = (VRInput.RHandPosition - prevPos);
        Vector3 throwDirection2= OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        // ��ġ ���
        prevPos = VRInput.RHandPosition;
        
        // ȸ�� ���� = current - previous
        Quaternion deltaRotation = VRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        // ȸ�� ���
        prevRot = VRInput.RHand.rotation;
        

        // ��ư�� ���Ҵٸ�
        if (VRInput.GetUp(VRInput.Button.HandTrigger, VRInput.Controller.RTouch))
        {
            // ���� ���� ���·� ��ȯ
            isGrabbing = false;
            // ���� ��� Ȱ��ȭ
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            // �տ��� ������Ʈ ����
            grabbedObject.transform.parent = null;

            // ������
            grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower;

            // ���ӵ� ����
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = rotPower * angularVelocity / 100f;

            // ���� ��ü�� ������ ����
            grabbedObject = null;
        }
    }

    IEnumerator GrabbingAnimation()
    {
        // ���� ��� ����
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        // �ʱ� ��ġ �� ����
        prevPos = VRInput.RHandPosition;
        // �ʱ� ȸ�� �� ����
        prevRot = VRInput.RHand.rotation;

        Vector3 startLocation = grabbedObject.transform.position;
        Vector3 targetLocation = VRInput.RHandPosition + VRInput.RHandDirection * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;

        // �����
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
            // ���� ��ü�� ���� �ڽ����� ���
            grabbedObject.transform.position = VRInput.RHandPosition;
            grabbedObject.transform.parent = VRInput.RHand;
        }

    }

    void Grabbing()
    {
        // �ʱ� ��ġ �� ����
        prevPos = VRInput.RHandPosition;
        // �ʱ� ȸ�� �� ����
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

            // ���� ��� ����, �θ���
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.transform.parent = VRInput.RHand;
        }

    }
}

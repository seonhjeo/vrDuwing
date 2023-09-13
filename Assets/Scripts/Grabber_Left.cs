using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber_Left : MonoBehaviour
{
    // ��ü�� ��� �ִ����� ����
    bool isGrabbing = false;
    // ��� �ִ� ��ü
    public GameObject grabbedObject;
    // ���� ��ü�� ����
    public LayerMask grabbedLayer;
    // ���� �� �ִ� �Ÿ�
    public float grabRange = 1.0f;

    // ���� ��ġ
    Vector3 prevPos;
    // ���� ��
    public float throwPower = 10;

    // ���� ȸ��
    Quaternion prevRot;
    // ȸ����
    public float rotPower = 2;

    // ���Ÿ� ��ü ��� �Ÿ�
    public float remoteGrabDistance = 2;

    public Transform crosshair; // ũ�ν��� ���� �Ӽ�

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        VRInput.DrawCrosshair(crosshair, true, VRInput.Controller.LTouch);

        // ��ü�� ���� �ʰ� ���� ���
        if (isGrabbing == false)
        {
            // ��ü ���
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
        if (VRInput.GetDown(VRInput.Button.HandTrigger, VRInput.Controller.LTouch))
        {
            // �� �������� Ray ����
            Ray ray = new Ray(VRInput.LHandPosition, VRInput.LHandDirection);
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
    }

    private void TryUngrab()
    {

        // ���� ����
        Vector3 throwDirection = (VRInput.LHandPosition - prevPos);
        Vector3 throwDirection2 = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        // ��ġ ���
        prevPos = VRInput.LHandPosition;

        // ȸ�� ���� = current - previous
        Quaternion deltaRotation = VRInput.LHand.rotation * Quaternion.Inverse(prevRot);
        // ȸ�� ���
        prevRot = VRInput.LHand.rotation;


        // ��ư�� ���Ҵٸ�
        if (VRInput.GetUp(VRInput.Button.HandTrigger, VRInput.Controller.LTouch))
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
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity * rotPower;

            // ���� ��ü�� ������ ����
            grabbedObject = null;
        }
    }

    IEnumerator GrabbingAnimation()
    {
        // ���� ��� ����
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
        // �ʱ� ��ġ �� ����
        prevPos = VRInput.LHandPosition;
        // �ʱ� ȸ�� �� ����
        prevRot = VRInput.LHand.rotation;

        Vector3 startLocation = grabbedObject.transform.position;
        Vector3 targetLocation = VRInput.LHandPosition + VRInput.LHandDirection * 0.1f;

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
            grabbedObject.transform.position = VRInput.LHandPosition;
            grabbedObject.transform.parent = VRInput.LHand;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber_Right : MonoBehaviour
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
    public float throwPower = 10f;

    // ���� ȸ��
    Quaternion prevRot;
    // ȸ����
    public float rotPower = 2f;

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
        VRInput.DrawCrosshair(crosshair, true, VRInput.Controller.RTouch);

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
        if (VRInput.GetDown(VRInput.Button.HandTrigger, VRInput.Controller.RTouch))
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
    }

    private void TryUngrab()
    {

        // ���� ����
        Vector3 throwDirection = (VRInput.RHandPosition - prevPos);
        Vector3 throwDirection2 = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
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
}

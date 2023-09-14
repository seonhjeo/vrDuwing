using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_newfilter : Interactable
{
    Rigidbody rb;

    public GameObject tool;
    public GameObject lhand;
    public GameObject rhand;

    public bool rot;

    public GameObject beforOil;
    public GameObject afterOil;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rot)
        {
            transform.Rotate(new Vector3(0, 7 * Time.deltaTime, 0));
        }
    }

    override public void Interact()
    {
        if(QuestManager.instance.questProgress==2)
        {
            Collider[] colliders =
                   Physics.OverlapSphere(transform.position, 0.4f);

            foreach (Collider col in colliders)
            {
                if (col.transform != QuestManager.instance.filterSoket)
                {
                    grabber.GrabEnd();
                    transform.SetParent(QuestManager.instance.filterSoket);
                    transform.localPosition = new Vector3(0, 0, 0);
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                    rb.isKinematic = true;
                    GetComponent<OVRGrabbable>().enabled = false;

                    QuestManager.instance.questProgress = 3;
                    break;
                }
            }

            QuestManager.instance.cvs_leftright3.SetActive(true);
            lhand.SetActive(true);
            rhand.SetActive(true);
            //load();
        }
    }

    public void locking()
    {
        tool.SetActive(true);

        QuestManager.instance.Lhand.SetActive(false);
        QuestManager.instance.Rhand.SetActive(false);
        StartCoroutine(RotateFilter(2f));
    }

    public void load()
    {
        lhand.SetActive(true);
        rhand.SetActive(true);

        QuestManager.instance.Lhand.SetActive(false);
        QuestManager.instance.Rhand.SetActive(false);
        StartCoroutine(RotateFilter2(4f));
    }

    IEnumerator RotateFilter(float time)
    {
        rot = true;
        yield return new WaitForSeconds(time);
        rot = false;
        tool.SetActive(false);

        QuestManager.instance.Lhand.SetActive(true);
        QuestManager.instance.Rhand.SetActive(true);
    }

    IEnumerator RotateFilter2(float time)
    {
        rot = true;
        yield return new WaitForSeconds(time);
        rot = false;
        lhand.SetActive(false);
        rhand.SetActive(false);

        QuestManager.instance.Lhand.SetActive(true);
        QuestManager.instance.Rhand.SetActive(true);

        if(QuestManager.instance.bOiled==false)
        {
            QuestManager.instance.PlayAlarm(1);
        }

        QuestManager.instance.PlayInfo(4);
    }

    public void ChangeColor()
    {
        beforOil.SetActive(false);
        afterOil.SetActive(true);
    }
}

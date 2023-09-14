using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_oldfilter : Interactable
{
    Rigidbody rb;

    public GameObject tool;
    public GameObject lhand;
    public GameObject rhand;

    public bool rot;

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
            transform.Rotate(new Vector3(0, -7*Time.deltaTime, 0));
        }
    }

    public void Unlock()
    {
        tool.SetActive(true);

        QuestManager.instance.Lhand.SetActive(false);
        QuestManager.instance.Rhand.SetActive(false);
        StartCoroutine(RotateFilter(2f));
    }

    public void Unload()
    {
        QuestManager.instance.questProgress = 2;

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
        rb.isKinematic = false;

        GetComponent<OVRGrabbable>().enabled = true;

        QuestManager.instance.Lhand.SetActive(true);
        QuestManager.instance.Rhand.SetActive(true);
    }
}

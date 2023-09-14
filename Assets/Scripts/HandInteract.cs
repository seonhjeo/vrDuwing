using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInteract : MonoBehaviour
{
    public OVRGrabber grabber;

    public VRInput.Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        grabber = GetComponent<OVRGrabber>();
    }

    // Update is called once per frame
    void Update()
    {
        if(VRInput.GetDown(VRInput.Button.IndexTrigger, controller))
        {
            Interact();
        }

        if (VRInput.GetDown(VRInput.Button.HandTrigger, controller))
        {
            Grab();
        }

    }

    void Interact()
    {
        if(grabber.grabbedObject!=null)
        {
            grabber.grabbedObject.GetComponent<Interactable>().Interact();
        }

        if(grabber.grabbedObject==null && QuestManager.instance.questProgress==1)
        {
            Collider[] colliders =
                                 Physics.OverlapSphere(transform.position, 0.2f);

            foreach (Collider col in colliders)
            {
                if (col.GetComponent<Interactable_oldfilter>() != null)
                {
                    if(col.GetComponent<Interactable_oldfilter>().rot==false)
                    {
                        QuestManager.instance.cvs_leftright2.SetActive(true);
                        col.GetComponent<Interactable_oldfilter>().lhand.SetActive(true);
                        col.GetComponent<Interactable_oldfilter>().rhand.SetActive(true);

                        //col.GetComponent<Interactable_oldfilter>().Unload();
                    }
                    break;
                }
            }
        }
        else if (QuestManager.instance.questProgress == 2)
        {
            Collider[] colliders =
                   Physics.OverlapSphere(transform.position, 0.3f);

            foreach (Collider col in colliders)
            {
                if (col.transform == QuestManager.instance.filterTop)
                {
                    col.transform.parent.GetComponent<Interactable_newfilter>().ChangeColor();
                    QuestManager.instance.bOiled = true;
                    break;
                }
            }
        }

        /*
        else if (grabber.grabbedObject == null && QuestManager.instance.questProgress == 3)
        {
            Collider[] colliders =
                     Physics.OverlapSphere(transform.position, 0.2f);

            foreach (Collider col in colliders)
            {
                if (col.GetComponent<Interactable_newfilter>() != null)
                {
                    if (col.GetComponent<Interactable_newfilter>().rot == false)
                    {
                        col.GetComponent<Interactable_newfilter>().load();
                    }

                    QuestManager.instance.questProgress = 4;
                    break;
                }
            }
        }
        */

    }

    void Grab()
    {
        Invoke("grabact",0.1f);
    }

    void grabact()
    {
        if (grabber.grabbedObject != null)
        {
            grabber.grabbedObject.GetComponent<Interactable>().Grabbed();
            grabber.grabbedObject.GetComponent<Interactable>().grabber = grabber;
        }
    }

}

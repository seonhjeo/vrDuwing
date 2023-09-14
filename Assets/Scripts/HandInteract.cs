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
    }

    void Interact()
    {
        if(grabber.grabbedObject!=null)
        {
            grabber.grabbedObject.GetComponent<Interactable>().Interact();
        }

        Debug.Log(controller);
    }
}

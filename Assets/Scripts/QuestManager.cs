using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public OVRGrabber left_Grabber;
    public OVRGrabber right_Grabber;

    public int questProgress = 0;


    private static QuestManager q_instance;

    public static QuestManager instance
    {
        get
        {
            if (q_instance == null)
            {
                q_instance = FindObjectOfType<QuestManager>();
            }

            return q_instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public OVRGrabber left_Grabber;
    public OVRGrabber right_Grabber;

    public GameObject Lhand;
    public GameObject Rhand;

    public Transform filterSoket;

    public bool bOiled;
    public bool bSafe;

    public int safeCount;

    public int questProgress = 0;


    public GameObject tuto_panel;

    private static QuestManager q_instance;

    public GameObject[] infos;

    public GameObject[] alarms;

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
        PlayInfo(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameClear()
    {
        StartCoroutine(ClearProcess());
    }

    IEnumerator ClearProcess()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("게임종료");
    }

    public void ManualOpen()
    {
        tuto_panel.SetActive(true);
    }
    
    public void PlayInfo(int i)
    {
        infos[i].SetActive(true);
    }

    public void PlayAlarm(int i)
    {
        alarms[i].SetActive(true);
    }
}

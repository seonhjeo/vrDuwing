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
    public Transform filterTop;

    public bool bOiled;
    public bool bSafe;

    public int safeCount;

    public int questProgress = 0;


    public GameObject tuto_panel;

    private static QuestManager q_instance;

    public GameObject[] infos;

    public GameObject[] alarms;

    public GameObject cvs_leftright1;
    public GameObject cvs_leftright2;
    public GameObject cvs_leftright3;
    public GameObject cvs_leftright4;

    public Interactable_oldfilter oldfilter;
    public Interactable_newfilter newfilter;

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
        for(int j=0; j< infos.Length;j++)
        {
            infos[j].SetActive(false);
        }

        infos[i].SetActive(true);
    }

    public void PlayAlarm(int i)
    {
        alarms[i].SetActive(true);
    }

    public void leftright1(int i)
    {
        if(i==0)
        {
            //정답
            oldfilter.Unlock();
            instance.questProgress = 1;

            cvs_leftright1.SetActive(false);
        }
        else
        {
            PlayAlarm(2);
        }
    }

    public void leftright2(int i)
    {
        if (i == 0)
        {
            //정답
            oldfilter.Unload();
            instance.questProgress = 2;

            cvs_leftright2.SetActive(false);
        }
        else
        {
            PlayAlarm(2);
        }
    }

    public void leftright3(int i)
    {
        if (i == 0)
        {
            PlayAlarm(2);
        }
        else
        {
            //정답
            newfilter.load();
            cvs_leftright3.SetActive(false);
        }
    }

    public void leftright4(int i)
    {
        if (i == 0)
        {
            PlayAlarm(2);
        }
        else
        {
            //정답
            newfilter.locking();
            instance.GameClear();

            cvs_leftright4.SetActive(false);

        }
    }
}

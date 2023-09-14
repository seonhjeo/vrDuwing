using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_tool : Interactable
{
    public Transform jaw;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Interact()
    {
        //��üó��
        if (QuestManager.instance.questProgress == 0)
        {
            Collider[] colliders =
                   Physics.OverlapSphere(jaw.position, 0.25f);

            foreach (Collider col in colliders)
            {
                if(col.GetComponent<Interactable_oldfilter>()!=null)
                {
                    col.GetComponent<Interactable_oldfilter>().Unlock();
                    QuestManager.instance.questProgress = 1;
                    break;
                }
            }
        }
        else if (QuestManager.instance.questProgress == 3)
        {
            //ü��ó��
            Collider[] colliders =
                   Physics.OverlapSphere(jaw.position, 0.25f);

            foreach (Collider col in colliders)
            {
                if (col.GetComponent<Interactable_newfilter>() != null)
                {
                    col.GetComponent<Interactable_newfilter>().locking();
                    QuestManager.instance.GameClear();
                    break;
                }
            }

        }
    }

    override public void Grabbed()
    {
        if (QuestManager.instance.bSafe == true)
        {
            //�׳� ������
        }
        else
        {
            //�г�Ƽ ó��
            QuestManager.instance.PlayAlarm(0);
            QuestManager.instance.bSafe = true;
        }
    }
}

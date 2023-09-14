using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_safety : Interactable
{
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

        if(QuestManager.instance.safeCount>=3)
        {
            Destroy(gameObject);

            //장착 코드

            //안전확보
            QuestManager.instance.bSafe=true;
        }
        else
        {
            //장착 코드
            Destroy(gameObject);
            QuestManager.instance.safeCount++;
        }
    }
}

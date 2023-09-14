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

        if(QuestManager.instance.questProgress<3)
        {
            Destroy(gameObject);

            //퀘스트 진척 확인
            QuestManager.instance.questProgress++;

            //퀘스트 완료 이벤트 처리
        }
    }
}

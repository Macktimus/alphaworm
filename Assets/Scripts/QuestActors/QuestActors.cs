using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestActors : MonoBehaviour
{
    //check if the quest is completed
    protected bool m_Completed = false;

    public bool GetQuestStatus ()
    {
        return m_Completed;
    }

    public virtual List<GameObject> CheckForObjects (List<GameObject> list)
    {
        return list;
    }
}

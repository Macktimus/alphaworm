using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBirdsQuestActor : QuestActors
{
    public GameObject m_GameObjectType;
    public int m_LocalRequirement;
    int m_LocalProgress;

    public override List<GameObject> CheckForObjects(List<GameObject> list)
    {
        foreach(GameObject go in list)
        {
            if( go.tag == m_GameObjectType.tag)
            {
                m_LocalProgress++;
                list.Remove(go); //finally a good use for this "lists exists everywhere" stuff
            }
            if(m_LocalProgress >= m_LocalRequirement)
            {
                m_Completed = true;
                break;
            }
        }

        //add this object to the tracked quest objects
        //or wait do we add this object to tracked quest objects when they're spawned?
        //when they're spawned, but we also need to have the level config know what the requirements are to complete the level

        return base.CheckForObjects(list);
    }
}

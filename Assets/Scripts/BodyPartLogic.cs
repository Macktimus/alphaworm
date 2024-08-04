using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartLogic : MonoBehaviour
{
    Vector3 m_NextPosition = new Vector3(); //unused
    Vector3 m_LastPosition = new Vector3();
    BodyPartLogic m_NextInQueue;
    public char m_MyChar;
    FoodObject m_MyFoodObject;

    public void BodyPartSetup (Vector3 newPos)
    {
        //Debug.Log("Setting up: "+newPos);
        gameObject.transform.position = newPos;
        gameObject.layer = 7;
        m_MyFoodObject = gameObject.GetComponent<FoodObject>();
        if(gameObject.GetComponent<LetterFoodObject>() != null)
        {
            m_MyChar = gameObject.GetComponent<LetterFoodObject>().GetLetter();
        }
        
    }

    public void AddPart (BodyPartLogic bpl)
    {
        //Debug.Log(gameObject.name + " is Adding Part: " + bpl.gameObject.name);
        m_NextInQueue = bpl;
        m_NextInQueue.BodyPartSetup(m_LastPosition);
    }

    public void UpdateChain( BodyPartLogic bpl)
    {
        m_NextInQueue = bpl;
    }

    public void UpdatePositionAndStep( Vector3 _newPos)//, Vector3 _newStep )
    {
        Debug.LogFormat("I am {0} and my letter is {1}. I was at {2} but move to {3}.", gameObject.name, m_MyChar, gameObject.transform.position, _newPos);
        if (m_NextInQueue != null) //if there's someone behind us
        {
            m_NextInQueue.UpdatePositionAndStep(m_LastPosition); //tell them where we were so they can go there
        }
        gameObject.transform.position = _newPos;
        m_LastPosition = _newPos;
    }

    public void ClearNextInQueue()
    {
        Debug.LogFormat("I am {0} and my letter is {1}. I am clearing my next.", gameObject.name, m_MyChar);
        m_NextInQueue = null;
    }

    public void TakeStep ( Vector3 nextStep )
    {
        m_LastPosition = gameObject.transform.position; //save current position into last position for sending to next part
        gameObject.transform.position = nextStep; //set the current position

        if( m_NextInQueue != null ) //if there's someone behind us
        {
            m_NextInQueue.TakeStep(m_LastPosition); //tell them where we were so they can go there
        }
    }

    public BodyPartLogic GetNextInQueue ()
    {
        return m_NextInQueue;
    }

    //we want to be able to get what type of food logic is attached to this thing, and then return it
    //someone or something needs to process each spelled food item's type. Maybe we don't need to actually 
    //check them, just have them send themselves off to a manager once they've been used in a word. 
}

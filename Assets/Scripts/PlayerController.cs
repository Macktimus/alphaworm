using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool m_SnakeMovementReference = false;

    //do we control speed here or at a level manager? maybe a level manager. Here for now. Simplicity's sake.
    float m_SpeedReference = 1f;
    //do we want to do this smooth movement style or should we go more snake like 
    //with a cooldown between movements and a step size?

    float m_StepTimeReference = 1f;
    float m_StepCounterReference;
    float m_StepSizeReference = 1f;

    Vector3 m_Direction = new Vector3();
    Vector3 m_LastStep = new Vector3();
    Vector3 m_LastStepPosition = new Vector3(); //for snake mode

    List<BodyPartLogic> m_SnakeBody = new List<BodyPartLogic>();

    // Start is called before the first frame update
    void Start()
    {
        m_Direction = Vector3.left;
        m_LastStep = Vector3.left;
        GameStateBoss.Instance.SetPlayerReference(this);
    }

    // Update is called once per frame
    void Update()
    {
        if( GameStateBoss.Instance.m_CurrentState == GameStateBoss.PlayStates.play )
        {
            //take player input
            //change direction (up-down, left-right)
            //take next step
            //communicate next step to next body in chain
            if (m_LastStep == Vector3.forward || m_LastStep == Vector3.back)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    m_Direction = Vector3.left;
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    m_Direction = Vector3.right;
                }
            }
            else if (m_LastStep == Vector3.left || m_LastStep == Vector3.right)
            {
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_Direction = Vector3.forward;
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m_Direction = Vector3.back;
                }
            }

            if (!m_SnakeMovementReference)
            {
                transform.position += m_Direction * m_SpeedReference * Time.deltaTime;
                m_LastStep = m_Direction;
            }
            else
            {
                m_StepCounterReference -= Time.deltaTime;
                if (m_StepCounterReference <= 0f)
                {
                    m_StepCounterReference = m_StepTimeReference;
                    m_LastStepPosition = transform.position;
                    transform.position += m_Direction * m_StepSizeReference;
                    m_LastStep = m_Direction;
                    if (m_SnakeBody.Count > 0)
                    {
                        m_SnakeBody[0].TakeStep(m_LastStepPosition);
                    }
                }
            }
        }
        
    }

    public void SetStepSizeType (bool snakeMoveOn, float speed, float stepTime, float stepSize)
    {
        m_SnakeMovementReference = snakeMoveOn;
        m_SpeedReference = speed;
        m_StepTimeReference = stepTime;
        m_StepSizeReference = stepSize;
    }

    public void IncreaseStep(float TimeChange, float MIN_StepTime)
    {
        //Debug.Log("Change: " + TimeChange + " MIN: " + MIN_StepTime);
        m_StepTimeReference = Mathf.Max(m_StepTimeReference - TimeChange, MIN_StepTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit something. "+ other.gameObject.name);
        //6 player head
        //7 player body
        //8 food
        //9 wall
        if(other.gameObject.layer == LayerMask.NameToLayer("Food") )
        {
            other.gameObject.GetComponent<FoodObject>().GetFoodObject();
            AddToBody(other.gameObject);
            //get the food object
            //tell it what's up
            //give it a bodypartlogic
            //add it to the body, move it to last position, update layer
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.layer == LayerMask.NameToLayer("PlayerBody"))
        {
            Debug.LogError("We dead. Hit: "+other.gameObject.name);
        }
    }

    void AddToBody (GameObject go)
    {
        go.AddComponent<BodyPartLogic>();
        if( m_SnakeBody.Count == 0 )
        {
            go.GetComponent<BodyPartLogic>().BodyPartSetup(m_LastStepPosition);
            m_SnakeBody.Add(go.GetComponent<BodyPartLogic>());
        }
        else if( m_SnakeBody.Count > 0 )
        {
            m_SnakeBody.Add(go.GetComponent<BodyPartLogic>());
            m_SnakeBody[m_SnakeBody.Count - 2].AddPart(go.GetComponent<BodyPartLogic>());
        }
        
    }

    public void RemoveFromBody (char[] removeChars)
    {
        List<GameObject> _returnObjects = new List<GameObject>();
        for (int y = 0; y < removeChars.Length; y++)
        {
            //Debug.LogFormat("For loop - X: {0}", x);
            for (int x = 0; x < m_SnakeBody.Count; x++)
            {
                //Debug.LogFormat("For loop - y: {0}", y);
                if ( m_SnakeBody[x].m_MyChar == removeChars[y])
                {
                    Debug.LogFormat("Found char object {0} with char {1}", m_SnakeBody[x].name, m_SnakeBody[x].m_MyChar);
                    if ( x > 0 && m_SnakeBody[x].GetNextInQueue() != null)
                    {
                        //link the previous entry (if not first entry) to the next entry
                        Debug.LogFormat("Link BODY {0} to {1}", x - 1, x + 1);
                        m_SnakeBody[x - 1].UpdateChain(m_SnakeBody[x + 1]);
                        m_SnakeBody[x+1].UpdatePositionAndStep(m_SnakeBody[x].transform.position);
                        //Debug.LogFormat("Give {0} the position of {1}", x + 1, x);
                        //m_SnakeBody[x + 1].transform.position = m_SnakeBody[x].transform.position;
                    }
                    else if(m_SnakeBody[x].GetNextInQueue() != null)
                    {
                        //give next entry the current part's position
                        //link the previous entry (if not first entry) to the next entry
                        Debug.LogFormat("Position Update {0}", x + 1);
                        m_SnakeBody[x + 1].UpdatePositionAndStep(m_SnakeBody[x].transform.position);
                        //Debug.LogFormat("Give {0} the position of {1}", x + 1, x);
                        //m_SnakeBody[x + 1].transform.position = m_SnakeBody[x].transform.position;
                    }
                    else if(x > 0 && m_SnakeBody[x].GetNextInQueue() == null)
                    {
                        Debug.LogFormat("Last Letter in chain, clear next reference");
                        m_SnakeBody[x - 1].ClearNextInQueue();

                    }
                    /*if (m_SnakeBody[x].GetNextInQueue() != null)
                    {
                        Debug.Log("Current body: " + m_SnakeBody[x].name + " x: " + x);
                        //link the previous entry to the next entry (if not last entry)
                        m_SnakeBody[x - 1].AddPart(m_SnakeBody[x + 1]);
                    }*/





                    //delete the current object
                    Debug.LogFormat("DELETE body {0} with index {1} and letter {2}", m_SnakeBody[x].name, x, m_SnakeBody[x].m_MyChar);

                    
                    GameObject _objectHolder = m_SnakeBody[x].gameObject;
                    //put the object in a list of objects for sending out
                    _returnObjects.Add(_objectHolder);

                    m_SnakeBody.Remove(m_SnakeBody[x]);
                    
                    Destroy(_objectHolder);
                    x--;
                    //break foreach
                    Debug.Log("Break");
                    break;
                }
            }
            
        }
        LevelAndScoreBoss.Instance.CheckUsedLetterForQuests(_returnObjects);
    }

    public List<Vector3> ReturnBodyPartPositions()
    {
        List<Vector3> _parts = new List<Vector3>();
        _parts.Add(gameObject.transform.position);
        foreach( BodyPartLogic bpl in m_SnakeBody )
        {
            _parts.Add(bpl.transform.position);
        }

        return _parts;
    }

//    public List
    //on collision
    //if food
    //add to pool
    //add to chain
    //link penultimate and ultimate pieces
    //change tag/layer to body
    //if wall/body
    //dead

    //manage the list of bodies
    //body piece removal
    //for each, remove
    //take list after all removals
    //reassign position (offset from player piece)
    //relink chain (a->b->c->d->e, ace, a->c->e)
}

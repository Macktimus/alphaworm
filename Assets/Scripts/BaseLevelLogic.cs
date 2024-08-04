using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLevelLogic : MonoBehaviour
{
    //what are the common factors that go into a level?

    //level setup

    //item for quest is collected
    //some levels will have permutations on this (ie. key for lock, or collecting the right amount of things)
    //a mission display
    //gets into royal match, match 3 area where we want to communicate this VERY clearly
    //collect X of thing
    //unlock X locks
    //make X cakes

    //progress

    //checking if the level is considered done or not
    bool m_LevelComplete = false;

    //triggering environmental things in the level or telling things to pause 

    //level specific items to spawn
    public List<GameObject> m_LevelSpawnObjects = new List<GameObject>();

    //the rate at which they spawn
    public float m_SpawnChance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestComponent : ScriptableObject
{
    //put what we need to spawn in here
    //we can limit the number of objects and whether they're auto spawned
    public GameObject m_SpawnObject;
    public float m_SpawnChance;
    public bool m_LimitSpawnCount = false;
    public bool m_StartSpawned = false;
}

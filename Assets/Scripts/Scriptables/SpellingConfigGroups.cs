using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpellingConfigGroups : ScriptableObject
{
    public List<SpellingConfigs> m_ConfigGroup = new List<SpellingConfigs>();
    public int m_ScoreBetweenChanges = 30;
    public int m_MaxScore = 100;


    //public float m_SpeedIncrement = 1f;
    public float m_StepTimeDecrement = 0.1f;
    public float m_MinStepTime = 0.1f;

    public float m_BadFoodChance = 10f; //as a percentage out of 100f

}

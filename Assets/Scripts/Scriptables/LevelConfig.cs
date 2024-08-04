using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelConfig : ScriptableObject
{
    public List<QuestConfig> m_Quests;
    public List<SpellingConfigs> m_LetterLists;
    public List<FoodObject> m_LetterFoodObjects;
}

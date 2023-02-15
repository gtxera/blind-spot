using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonBehaviour<QuestManager>
{
    public HashSet<Quest> CurrentQuests { get; private set; } = new HashSet<Quest>();

    public void RemoveCurrentQuest(Quest quest)
    {
        CurrentQuests.Remove(quest);
    }

    public void AddNewQuest(Quest quest)
    {
        CurrentQuests.Add(quest);
    }
    
    
}

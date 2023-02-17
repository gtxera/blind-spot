using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : SingletonBehaviour<QuestManager>
{
    private Dictionary<Quest, QuestPhase> _currentQuests = new();

    private Dictionary<QuestPhase, List<GameEventListener>> _currentPhaseListners = new(); 

    private readonly HashSet<Quest> _finishedQuests = new();
    
    public event Action<Quest> OnQuestStarted, OnQuestFinished;

    public event Action<Quest, QuestPhase> OnPhaseTransition;
    
    public bool HasActiveQuest => _currentQuests.Any();

    public bool HasMandatoryActiveQuest => _currentQuests.Keys.Any(quest => quest.IsMandatory);

    public void StartQuest(Quest newQuest)
    {
        if (_finishedQuests.Contains(newQuest)) return;
        
        _currentQuests.Add(newQuest, newQuest.QuestPhases[0]);
        OnQuestStarted?.Invoke(newQuest);
        PhaseTransition(newQuest, newQuest.QuestPhases[0]);
    }

    private void FinishQuest(Quest endingQuest)
    {
        _finishedQuests.Add(endingQuest);
        _currentQuests.Remove(endingQuest);
        OnQuestFinished?.Invoke(endingQuest);
    }

    private void PhaseTransition(Quest currentQuest, QuestPhase newPhase)
    {

        if (_currentPhaseListners.ContainsKey(_currentQuests[currentQuest]))
        {
            foreach (var listener in _currentPhaseListners[_currentQuests[currentQuest]])
            {
                listener.DisableListener();
            }
            
            _currentPhaseListners.Remove(_currentQuests[currentQuest]);
        }

        if (!newPhase.NextPhases.Keys.Any())
        {
            FinishQuest(currentQuest);
            return;
        }
        
        _currentPhaseListners.Add(newPhase, GeneratePhaseListeners(newPhase));
        
        OnPhaseTransition?.Invoke(currentQuest, newPhase);
        
        _currentQuests[currentQuest] = newPhase;
    }

    private List<GameEventListener> GeneratePhaseListeners(QuestPhase phase)
    {
        List<GameEventListener> listeners = new();

        foreach (var gameEvent in phase.NextPhases.Keys)
        {
            var listener = new GameEventListener(gameEvent, () =>
            {
                PhaseTransition(phase.ParentQuest, phase.NextPhases[gameEvent]);
            });
            
            listener.EnableListener();
            
            listeners.Add(listener);
        }

        return listeners;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestUIManager : MonoBehaviour
{ 
    [SerializeField] private GameObject _questUI;
    
    [SerializeField] private GameObject _phaseTextParent, _phaseTextPrefab;

    private Dictionary<Quest, TextMeshProUGUI> _phasesText = new();

    private Dictionary<Quest, GameObject> _questTextGameObjects = new();

    private GameObjectPool _questTextObjectsPool;

    private void Start()
    {
        _questTextObjectsPool = new GameObjectPool(_phaseTextPrefab, _phaseTextParent);

        QuestManager.Instance.OnQuestStarted += CreateQuestText;
        QuestManager.Instance.OnPhaseTransition += UpdateText;
        QuestManager.Instance.OnQuestFinished += DestroyQuestText;
    }

    private void CreateQuestText(Quest quest)
    {
        if (!_questUI.activeSelf)
        {
            _questUI.SetActive(true);
        }

        var questTextGameObject = _questTextObjectsPool.GetObject(true);
        var questText = questTextGameObject.GetComponent<TextMeshProUGUI>();

        questText.color = quest.IsMandatory ? Color.yellow : Color.white;
        
        _phasesText.Add(quest, questText);
        _questTextGameObjects.Add(quest, questTextGameObject);
    }

    private void DestroyQuestText(Quest quest)
    {
        if (!QuestManager.Instance.HasActiveQuest)
        {
            _questUI.SetActive(false);
        }

        _phasesText.Remove(quest);
        _questTextObjectsPool.DisableObject(_questTextGameObjects[quest]);
        _questTextGameObjects.Remove(quest);
    }

    private void UpdateText(Quest quest, QuestPhase newPhase)
    {
        var textMesh = _phasesText[quest];

        textMesh.text = newPhase.PhaseText;
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _questUIGameObject, _phaseTextParent, _phaseTextPrefab;

    [SerializeField] private Transform _arrowIndicatorTransform, _playerTransform;

    private float _arrowAngle;

    private float _arrowCircleOffset = 1;

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
        if(!_questUIGameObject.activeSelf) _questUIGameObject.SetActive(true);

        var questTextGameObject = _questTextObjectsPool.GetObject(true);
        var questText = questTextGameObject.GetComponent<TextMeshProUGUI>();

        questText.color = quest.IsMandatory ? Color.yellow : Color.white;
        
        _phasesText.Add(quest, questText);
        _questTextGameObjects.Add(quest, questTextGameObject);
    }

    private void DestroyQuestText(Quest quest)
    {
        if(!QuestManager.Instance.HasActiveQuest) _questUIGameObject.SetActive(false);

        _phasesText.Remove(quest);
        _questTextObjectsPool.DisableObject(_questTextGameObjects[quest]);
        _questTextGameObjects.Remove(quest);
        
    }

    private void UpdateText(Quest quest, QuestPhase newPhase)
    {
        var textMesh = _phasesText[quest];

        textMesh.text = newPhase.PhaseText;
    }

    /*private void LateUpdate()
    {
        var arrowPositionOffset = new Vector3(Mathf.Cos(_arrowAngle) * _arrowCircleOffset,
            _playerTransform.position.y + 0.1f,
            Mathf.Sin(_arrowAngle) * _arrowCircleOffset);

        _arrowIndicatorTransform.position = _playerTransform.position + arrowPositionOffset;
        _arrowAngle += Time.deltaTime * 10;
    }*/
}

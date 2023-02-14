using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SingletonBehaviour<DialogueManager>
{

    [SerializeField] private GameObject _dialogueUI, _normalLineContainer, _characterLineContainer, _choiceButtonPrefab;

    private Transform _choicesContainer;

    private TextMeshProUGUI _currentLineText, _normalLineText, _characterLineText;

    private Image _characterImage;

    private DialogueLineSO _currentLine;

    private HandledCoroutine _showTextRoutine;

    private GameObjectPool _buttonsPool;

    private void Start()
    {
        PlayerInputs.Instance.MouseLeftButtonDownEvent += OnMouseClick;

        _choicesContainer = _dialogueUI.transform.Find("Choices");

        _normalLineText = _normalLineContainer.GetComponentInChildren<TextMeshProUGUI>();
        
        _characterLineText = _characterLineContainer.GetComponentInChildren<TextMeshProUGUI>();
        _characterImage = _characterLineContainer.transform.GetChild(0).GetComponent<Image>();

        _buttonsPool = new GameObjectPool(_choiceButtonPrefab, _choicesContainer.gameObject);
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        PlayerMovementStateMachine.Instance.ChangeMovementState(new StaticMovementState());
        PlayerInteraction.Instance.IgnoreInteraction(true);

        _dialogueUI.SetActive(true);

        _currentLine = dialogue.DialogueLines[0];
        ShowCurrentLine();
    }


    private void TryLoadNextLine(DialogueLineSO nextLine)
    {
        if (nextLine == null)
        {
            EndDialogue();
            return;
        }

        _currentLine = nextLine;
        ShowCurrentLine();
    }

    private void EndDialogue()
    {
        PlayerMovementStateMachine.Instance.ChangeDefaultMovementState();
        PlayerInteraction.Instance.IgnoreInteraction(false);
        _dialogueUI.SetActive(false);
    }

    private void ShowCurrentLine()
    {
        if (_currentLine is DialogueCharacterSimpleLine or DialogueCharacterChoiceLine)
        {
            _normalLineContainer.SetActive(false);
            _characterLineContainer.SetActive(true);

            switch (_currentLine)
            {
                case DialogueCharacterSimpleLine characterSimpleLine:
                    _characterImage.sprite = characterSimpleLine.Character.Portraits[characterSimpleLine.PortraitKey];
                    break;
                case DialogueCharacterChoiceLine characterChoiceLine:
                    _characterImage.sprite = characterChoiceLine.Character.Portraits[characterChoiceLine.PortraitKey];
                    break;
            }
            
            
            _currentLineText = _characterLineText;
        }

        else
        {
            _normalLineContainer.SetActive(true);
            _characterLineContainer.SetActive(false);
            
            _currentLineText = _normalLineText;
        }
        
        switch (_currentLine)
        {
            case DialogueSimpleLine simpleLine:
                _currentLineText.text = simpleLine.Text;
                _showTextRoutine = this.StartHandledCoroutine(ShowFullText, ShowFullText,LineTextReveal());
                break;

            case DialogueChoiceLine choiceLine:
                _currentLineText.text = choiceLine.Text;
                StartCoroutine(ChoicesAndTextReveal(choiceLine));
                break;
            
            case DialogueEventLine eventLine:
                eventLine.DialogueEvent.RaiseEvent();
                TryLoadNextLine(eventLine.NextDialogue);
                break;
        }
    }

    private IEnumerator LineTextReveal()
    {
        _currentLineText.maxVisibleCharacters = 1;

        for (int i = 1; i < _currentLineText.text.Length + 1; i++)
        {
            _currentLineText.maxVisibleCharacters = i;
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator ChoicesAndTextReveal(DialogueChoiceLine choiceLine)
    {
        _showTextRoutine = this.StartHandledCoroutine(ShowFullText, ShowFullText, LineTextReveal());

        while (_showTextRoutine.Running)
        {
            yield return new WaitForEndOfFrame();
        }

        foreach (var choice in choiceLine.Choices)
        {
            InstantiateChoiceButton(choice);
        }
    }

    private void InstantiateChoiceButton(DialogueChoice choice)
    {
        var button = _buttonsPool.GetObject(true);

        button.GetComponent<Button>().onClick.AddListener(delegate
        {
            TryLoadNextLine(choice.NextDialogue);
            RemoveChoices();
            
        });
        
        button.GetComponentInChildren<TextMeshProUGUI>().text = choice.ChoiceText;
        
    }

    private void RemoveChoices()
    {
        foreach (var button in _buttonsPool.Pool)
        {
            _buttonsPool.DisableObject(button);
        }
    }

    private void OnMouseClick()
    {
        if (!_dialogueUI.activeSelf) return;

        if (_showTextRoutine.Running)
        {
            this.StopHandledCoroutine(_showTextRoutine);
        }

        else
        {
            switch (_currentLine)
            {
                case DialogueSimpleLine simpleLine:
                    _currentLine = simpleLine.NextDialogue;
                    TryLoadNextLine(_currentLine);
                    break;
            }
        }
    }

    private void ShowFullText()
    {
        _currentLineText.maxVisibleCharacters = _currentLineText.text.Length;
    }
}

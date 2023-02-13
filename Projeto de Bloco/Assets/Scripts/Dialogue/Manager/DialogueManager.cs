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

    private void Start()
    {
        PlayerInputs.Instance.MouseLeftButtonDownEvent += OnMouseClick;

        _choicesContainer = _dialogueUI.transform.Find("Choices");

        _normalLineText = _normalLineContainer.GetComponentInChildren<TextMeshProUGUI>();
        
        _characterLineText = _characterLineContainer.GetComponentInChildren<TextMeshProUGUI>();
        _characterImage = _normalLineContainer.GetComponentInChildren<Image>();
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        PlayerMovementStateMachine.Instance.ChangeMovementState(new StaticMovementState());

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
        _dialogueUI.SetActive(false);
    }

    private void ShowCurrentLine()
    {
        if (_currentLine is DialogueCharacterSimpleLine or DialogueCharacterChoiceLine)
        {
            _normalLineContainer.SetActive(false);
            _characterLineContainer.SetActive(true);
            
            var characterLine = _currentLine as DialogueCharacterSimpleLine;
            _characterImage.sprite = characterLine.Character.Portraits[characterLine.PortraitKey];
            
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
                _showTextRoutine = this.StartHandledCoroutine(LineTextReveal());
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
        _showTextRoutine = this.StartHandledCoroutine(LineTextReveal());

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
        var button = Instantiate(_choiceButtonPrefab, _choicesContainer, true);

        button.GetComponent<Button>().onClick.AddListener(delegate { TryLoadNextLine(choice.NextDialogue); });
        
        button.GetComponentInChildren<TextMeshProUGUI>().text = choice.ChoiceText;
        
    }

    private void OnMouseClick()
    {
        if (!_dialogueUI.activeSelf) return;

        if (_showTextRoutine.Running)
        {
            _currentLineText.maxVisibleCharacters = _currentLineText.text.Length;
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
}

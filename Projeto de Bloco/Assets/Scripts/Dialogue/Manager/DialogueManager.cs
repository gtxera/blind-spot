using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    private Task _showTextTask;

    private GameObjectPool _buttonsPool;

    private CancellationTokenSource _showTextTokenSource;
    private CancellationToken _showTextToken;

    private bool _startDialogueCalled;

    public event Action<DialogueSO> OnDialogueStarted, OnDialogueEnded;

    private void Start()
    {
        PlayerInputs.Instance.MouseLeftButtonDownEvent += OnMouseClick;

        _choicesContainer = _dialogueUI.transform.Find("Choices");

        _normalLineText = _normalLineContainer.GetComponentInChildren<TextMeshProUGUI>();
        
        _characterLineText = _characterLineContainer.GetComponentInChildren<TextMeshProUGUI>();
        _characterImage = _characterLineContainer.transform.GetChild(0).GetComponent<Image>();

        _buttonsPool = new GameObjectPool(_choiceButtonPrefab, _choicesContainer.gameObject);
        
    }

    private void OnDisable()
    {
        PlayerInputs.Instance.MouseLeftButtonDownEvent -= OnMouseClick;
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        _startDialogueCalled = true;

        _showTextTokenSource = null;
        _showTextTask = null;

        switch (PlayerMovementStateMachine.Instance.CurrentMovementState)
        {
            case CarMovementState carMovementState:
                break;
            default:
                PlayerMovementStateMachine.Instance.ResetCurrentMovementState();
                PlayerMovementStateMachine.Instance.ChangeMovementState(new StaticMovementState());
                PlayerInteraction.Instance.IgnoreInteraction(true);
                break;
                
        }
        
        OnDialogueStarted?.Invoke(dialogue);

        _dialogueUI.SetActive(true);

        _currentLine = dialogue.DialogueLines[0];
        ShowCurrentLine();
    }


    private void TryLoadNextLine(DialogueLineSO nextLine)
    {
        _startDialogueCalled = false;
        
        if (nextLine == null)
        {
            EndDialogue();
            return;
        }

        _currentLine = nextLine;
        ShowCurrentLine();
    }

    private void EndDialogue(DialogueSO dialogue = null)
    {
        PlayerMovementStateMachine.Instance.ChangeDefaultMovementState();
        PlayerInteraction.Instance.IgnoreInteraction(false);
        
        OnDialogueEnded?.Invoke(dialogue);
        
        _showTextTokenSource.Cancel();
        
        _dialogueUI.SetActive(false);
    }

    private void ShowCurrentLine()
    {
        _showTextTokenSource?.Cancel();
        _showTextTokenSource = new CancellationTokenSource();
        _showTextToken = _showTextTokenSource.Token;

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
                _showTextTask = LineTextReveal(_showTextToken);
                break;

            case DialogueChoiceLine choiceLine:
                _currentLineText.text = choiceLine.Text;
                _showTextTask = ChoicesAndTextReveal(choiceLine, _showTextToken);
                break;
            
            case DialogueEventLine eventLine:
                eventLine.DialogueEvent.RaiseEvent();
                
                if (!_startDialogueCalled)
                {
                    TryLoadNextLine(eventLine.NextDialogue);
                }
                
                break;
        }
    }

    private async Task LineTextReveal(CancellationToken token)
    {
        _currentLineText.maxVisibleCharacters = 1;

        for (int i = 1; i < _currentLineText.text.Length + 1; i++)
        {
            try
            {
                await Task.Delay(50, token);
                _currentLineText.maxVisibleCharacters = i;
            }
            catch(OperationCanceledException) when(token.IsCancellationRequested)
            {
                ShowFullText();
                throw new TaskCanceledException();
            }
        }
    }

    private async Task ChoicesAndTextReveal(DialogueChoiceLine choiceLine, CancellationToken token)
    {
        _showTextTask = LineTextReveal(token);
        try
        {
            await _showTextTask;
        }
        catch (TaskCanceledException){ }

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

        if (!_showTextTokenSource.IsCancellationRequested && !_showTextTask.IsCompleted)
        {
            _showTextTokenSource.Cancel();
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

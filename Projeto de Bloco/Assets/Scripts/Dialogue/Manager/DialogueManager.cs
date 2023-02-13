using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : SingletonBehaviour<DialogueManager>
{

    [SerializeField] private GameObject _dialogueUI;

    private TextMeshProUGUI _lineText;

    private DialogueLineSO _currentLine;

    private Coroutine _showTextRoutine;

    private void Start()
    {
        PlayerInputs.Instance.MouseLeftButtonDownEvent += OnMouseClick;
        
        _lineText = _dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        PlayerMovementStateMachine.Instance.ChangeMovementState(new StaticMovementState());

        _dialogueUI.SetActive(true);

        _currentLine = dialogue.DialogueLines[0];
        ShowCurrentLine();
    }
    

    public void TryLoadNextLine(DialogueLineSO nextLine)
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
        switch (_currentLine)
        {
            case DialogueSimpleLine:
                var simpleLine = _currentLine as DialogueSimpleLine;
                _lineText.text = simpleLine.Text;
                _showTextRoutine = StartCoroutine(LineTextReveal());
                break;
        }
    }

    private IEnumerator LineTextReveal()
    {
        _lineText.maxVisibleCharacters = 1;

        for (int i = 1; i < _lineText.text.Length + 1; i++)
        {
            _lineText.maxVisibleCharacters = i;
            
            yield return new WaitForSeconds(0.05f);
        }

        _showTextRoutine = null;
    }

    private void OnMouseClick()
    {
        if (!_dialogueUI.activeSelf) return;
        
        if (_showTextRoutine != null)
        {
            _lineText.maxVisibleCharacters = _lineText.text.Length;
            StopCoroutine(_showTextRoutine);
            _showTextRoutine = null;
        }
        
        else
        {
            if (_currentLine is not DialogueSimpleLine simpleLine) return;

            TryLoadNextLine(simpleLine.NextDialogue);
        }
    }
}

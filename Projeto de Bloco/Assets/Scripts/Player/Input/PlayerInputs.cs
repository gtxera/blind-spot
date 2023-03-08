using System;
using UnityEngine;

public class PlayerInputs : SingletonBehaviour<PlayerInputs> {

    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
        
    public Vector3 MovementDirection { get; private set; }
    private bool _isMoving;
    public event Action MovementStartedEvent, MovementStoppedEvent;
    
    public bool CarBreaksPressed { get; private set; }

    private bool _interactKeyDown;
    public event Action InteractKeyDownEvent;

    private bool _runKeyDown;
    public event Action RunKeyDownEvent;
        
    private bool _runKeyUp;
    public event Action RunKeyUpEvent;

    private bool _mouseLeftButtonDown;
    public event Action MouseLeftButtonDownEvent;
    
    private float _mouseWheelMovement;
    public event Action<float> MouseWheelMove;

    private bool _radioKeyDown;
    public event Action RadioKeyDown;

    private bool _inventoryKeyDown;
    public event Action InventoryKeyDown;
    
    private void Update()
    {
        GetInputs();
        RaiseEvents();
    }

    private void GetInputs()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");
            
        MovementDirection = new Vector3(HorizontalInput, 0,VerticalInput);

        CarBreaksPressed = Input.GetKey(InputBindings.Bindings[InputBindings.PlayerActions.CarBreaks]);

        _interactKeyDown = Input.GetKeyDown(InputBindings.Bindings[InputBindings.PlayerActions.Interact]);
            
        _runKeyDown = Input.GetKeyDown(InputBindings.Bindings[InputBindings.PlayerActions.Run]);
        _runKeyUp = Input.GetKeyUp(InputBindings.Bindings[InputBindings.PlayerActions.Run]);

        _mouseLeftButtonDown = Input.GetMouseButtonDown(0);

        _mouseWheelMovement = Input.GetAxis("Mouse ScrollWheel");

        _radioKeyDown = Input.GetKeyDown(InputBindings.Bindings[InputBindings.PlayerActions.Radio]);

        _inventoryKeyDown = Input.GetKeyDown(InputBindings.Bindings[InputBindings.PlayerActions.OpenInventory]);
    }

    private void RaiseEvents()
    {
        if (MovementDirection != Vector3.zero && !_isMoving)
        { 
            _isMoving = true; 
            MovementStartedEvent?.Invoke();
        }
        else if (MovementDirection == Vector3.zero && _isMoving)
        { 
            _isMoving = false;
            MovementStoppedEvent?.Invoke();
        }
            
        if(_interactKeyDown) InteractKeyDownEvent?.Invoke();
            
        if(_runKeyDown) RunKeyDownEvent?.Invoke();
        if(_runKeyUp) RunKeyUpEvent?.Invoke();

        if (_mouseLeftButtonDown) MouseLeftButtonDownEvent?.Invoke();
        
        if(_mouseWheelMovement != 0) MouseWheelMove?.Invoke(_mouseWheelMovement);
        
        if(_radioKeyDown) RadioKeyDown?.Invoke();
        
        if(_inventoryKeyDown) InventoryKeyDown?.Invoke();
    }
}

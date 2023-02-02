using System;
using UnityEngine;

public class PlayerInputs : SingletonBehaviour<PlayerInputs> {

    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }
        
    public Vector3 MovementDirection { get; private set; }
    private bool _isMoving;
    public event Action MovementStartedEvent, MovementStoppedEvent;

    public bool SpaceBarPressed { get; private set; }
        
    private bool _eKeyDown;
    public event Action EKeyDownEvent;

    private bool _shiftKeyDown;
    public event Action ShiftKeyDownEvent;
        
    private bool _shiftKeyUp;
    public event Action ShiftKeyUpEvent;
    

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

        SpaceBarPressed = Input.GetKey(KeyCode.Space);
            
        _eKeyDown = Input.GetKeyDown(KeyCode.E);
            
        _shiftKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
        _shiftKeyUp = Input.GetKeyUp(KeyCode.LeftShift);
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
            
        if(_eKeyDown) EKeyDownEvent?.Invoke();
            
        if(_shiftKeyDown) ShiftKeyDownEvent?.Invoke();
        if(_shiftKeyUp) ShiftKeyUpEvent?.Invoke(); 
    }
}

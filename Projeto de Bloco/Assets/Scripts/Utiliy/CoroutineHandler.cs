using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineHandler
{
    public static HandledCoroutine StartHandledCoroutine(this MonoBehaviour obj, params IEnumerator[] routines)
    {
        var handledCoroutine = new HandledCoroutine(obj);

        obj.StartCoroutine(handledCoroutine.StartHandledRoutine(routines));

        return handledCoroutine;
    }

    public static void StopHandledCoroutine(this MonoBehaviour obj, HandledCoroutine handledCoroutine)
    {
        handledCoroutine.StopHandledRoutine();
        obj.StopCoroutine(handledCoroutine.HandledRoutineRef);
    }
}

public class HandledCoroutine
{
    public HandledCoroutine(MonoBehaviour obj)
    {
        _obj = obj;
    }
    
    public bool Running { get; private set; }
    public bool Finished { get; private set; }
    public bool Stopped { get; private set; }

    private MonoBehaviour _obj;

    private Coroutine _currentRoutine;

    public Coroutine HandledRoutineRef;

    public IEnumerator StartHandledRoutine(IEnumerator[] routines)
    {
        yield return HandledRoutineRef = _obj.StartCoroutine(HandledRoutine(routines));
    }
    
    private IEnumerator HandledRoutine(IEnumerator[] routines)
    {
        Stopped = false;
        Finished = false;
        Running = true;

        foreach (var routine in routines)
        {
            yield return _currentRoutine = _obj.StartCoroutine(routine);
        }

        Finished = true;
        Running = false;
    }

    public void StopHandledRoutine()
    {
        Running = false;
        Finished = false;
        Stopped = true;
        
        _obj.StopCoroutine(_currentRoutine);
    }
}

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : MonoBehaviour
{
    [SerializeField]
    TextControl _textControl = default;
    [SerializeField]
    ActorControl _actorControl = default;
    [SerializeField]
    BackgroundControl _background = default;
    [SerializeField]
    DataLoadController _dataLoadController = default;
    [SerializeField]
    AllCharacterData _characterData = default;
    IStoryControl[] _allControl = default;
    bool _actionNow = false;
    bool _check = false;
    private void Start()
    {
        IStoryControl[] allControl = { _textControl, _actorControl, _background };
        _allControl = allControl;
        _actorControl.AddActor(_characterData.GetCharacter(0), 9);
        _actorControl.AddActor(_characterData.GetCharacter(1), 8);
        _dataLoadController.SetControl(_textControl, _actorControl, _background);
        _textControl.OnTextEnd += _textControl.StartStory;
        _textControl.StartStory();
        _actionNow = true;
    }

    public void OnClickNext()
    {
        if (!_actionNow || _check)
        {
            return;
        }
        _check = true;
        foreach (var control in _allControl)
        {
            control.Skip();
        }
        StartCoroutine(SkipCheck());
    }
    IEnumerator SkipCheck()
    {
        while (CheckAction())
        {
            yield return null;
        }
        _check = false;
        _actionNow = true;
    }
    bool CheckAction()
    {
        foreach (var control in _allControl)
        {
            if (control.ActionNow)
            {
                control.Skip();
                return true;
            }
        }
        return false;
    }
    IEnumerator WaitAllAsync(IEnumerator[] asyncs, Action[] actions)
    {
        var canceler = new Canceler(asyncs.Length);
        for (int i = 0; i < asyncs.Length; i++)
        {
            StartCoroutine(Await(asyncs[i],canceler.Cancel));
        }
        while (!canceler.IsCanceld)
        {     
            yield return null;
        }
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    }
    IEnumerator Await(IEnumerator async, Action action)
    {
        yield return async;
        action?.Invoke();
    }
    IEnumerator WaitAnyAsync(IEnumerator[] asyncs, Action[] actions)
    {
        var canceler = new Canceler(0);
        for (int i = 0; i < asyncs.Length; i++)
        {
            StartCoroutine(Await(asyncs[i], canceler.Cancel));
        }
        while (!canceler.IsCanceld)
        {
            yield return null;
        }
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    }
   
}

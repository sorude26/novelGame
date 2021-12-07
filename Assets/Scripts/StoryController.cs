using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : MonoBehaviour
{
    [SerializeField]
    TextControl m_textControl = default;
    [SerializeField]
    ActorControl m_actorControl = default;
    [SerializeField]
    BackgroundControl m_background = default;
    [SerializeField]
    AllCharacterData m_characterData = default;
    IStoryControl[] m_allControl = default;
    bool m_actionNow = false;
    bool m_check = false;
    private void Start()
    {
        IStoryControl[] allControl = { m_textControl, m_actorControl, m_background };
        m_allControl = allControl;
        m_textControl.StartSet();
        m_actorControl.AddActor(m_characterData.GetCharacter(0), 1);
        m_actorControl.AddActor(m_characterData.GetCharacter(1), 6);
        IEnumerator[] events = { WaitAllAsync( new IEnumerator[]{ m_textControl.ViewText(), m_background.CrossFadeChange(5f, 2) },new Action[] { () => { } }) };
        m_textControl.EventControl.AddEvent(events);
        events = new IEnumerator[] { WaitAllAsync(new IEnumerator[] { m_textControl.ViewText(), m_background.CrossFadeChange(5f, 1) }, new Action[] { () => { } }) };
        m_textControl.EventControl.AddEvent(events);
        events = new IEnumerator[] { WaitAllAsync(new IEnumerator[] { m_background.CrossFadeChange(5f, 0) }, new Action[] { () => { } }) };
        m_textControl.EventControl.AddEvent(events);
        events = new IEnumerator[] { WaitAllAsync(new IEnumerator[] { m_textControl.ViewText(), m_background.CrossFadeChange(5f, 2) }, new Action[] { () => { } }) };
        m_textControl.EventControl.AddEvent(events);
        events = new IEnumerator[] { WaitAllAsync(new IEnumerator[] { m_textControl.ViewText(), m_background.CrossFadeChange(5f, 1) }, new Action[] { () => { } }) };
        m_textControl.EventControl.AddEvent(events);
        events = new IEnumerator[] { WaitAllAsync(new IEnumerator[] { m_textControl.ViewText(), m_background.CrossFadeChange(5f, 1) }, new Action[] { () => { } }) };
        m_textControl.EventControl.AddEvent(events); 
        events = new IEnumerator[] { WaitAllAsync(new IEnumerator[] { m_textControl.ViewText(), m_background.CrossFadeChange(5f, 1) }, new Action[] { () => { } }) };
        m_textControl.EventControl.AddEvent(events);
        m_textControl.OnTextEnd += m_textControl.StartStory;
        m_textControl.StartStory();
        m_actionNow = true;
    }

    public void OnClickNext()
    {
        if (!m_actionNow || m_check)
        {
            return;
        }
        m_check = true;
        foreach (var control in m_allControl)
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
        m_check = false;
        m_actionNow = true;
    }
    bool CheckAction()
    {
        foreach (var control in m_allControl)
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
    IEnumerator WaitAnyAsync(IEnumerator[] e, Action[] actions)
    {
        bool async = true;
        while (async)
        {
            for (int i = 0; i < e.Length; i++)
            {
                if (!e[i].MoveNext())
                {
                    async = false;
                }
            }
            yield return null;
        }
        foreach (var action in actions)
        {
            action?.Invoke();
        }
    }
   
}

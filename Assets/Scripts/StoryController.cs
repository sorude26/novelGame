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
    StoryEventControl m_event = default;
    bool m_actionNow = false;
    bool m_check = false;
    private void Start()
    {
        IStoryControl[] allControl = { m_textControl, m_actorControl, m_background };
        m_allControl = allControl;
        m_event = new StoryEventControl(m_textControl.TextCount);
        m_event.AddEvent(0, Test1);
        m_event.AddEvent(1, Test2);
        m_event.AddEvent(2, Test3);
        m_event.AddEvent(3, Test4);
        m_event.AddEvent(4, Test5);
        m_event.AddEvent(5, Test6);
        m_actorControl.AddActor(m_characterData.GetCharacter(0), 1);
        m_actorControl.AddActor(m_characterData.GetCharacter(1), 6);
        m_actorControl.AddActor(m_characterData.GetCharacter(1), 4);
        m_textControl.OnViewLineStart += m_event.PlayEvent;
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
    void Test1()
    {
        m_actorControl.SelectActor(0).FadeIn(2);
        m_actorControl.SelectActor(1).FadeIn(2,() =>
        m_background.StartCrossFadeBackground(1, 4));
    }
    void Test2()
    {
        m_background.StartCrossFadeBackground(3, 2);
        m_actorControl.SelectActor(0).FadeIn(2, () =>
         {
             m_actorControl.SelectActor(0).StartChangeColor(0, Color.clear);
             m_actorControl.SelectActor(1).FadeIn(2, () =>
             {
                 m_background.StartCrossFadeBackground(2, 0);
                 m_actorControl.SelectActor(0).FadeIn(2);
             });
         });
    }
    void Test3()
    {
        m_background.StartChangeBackground(1, 1);
        m_actorControl.SelectActor(1).StartMoveStraight(4, m_actorControl.GetPos(6), m_actorControl.GetPos(3));
    }
    void Test4()
    {
        m_background.StartCrossFadeBackground(3, 3);
        m_actorControl.SelectActor(0).StartChangeColor(3, Color.black);
    }
    void Test5()
    {
        m_background.StartChangeBackground(1, 0,() =>
        m_actorControl.SelectActor(0).StartChangeColor(1, Color.white,() =>
        m_actorControl.SelectActor(1).StartChangeColor(3, Color.black,() => 
        m_actorControl.SelectActor(1).StartMoveStraight(0.5f, m_actorControl.GetPos(3), m_actorControl.GetPos(6)))));
    }
    void Test6()
    {
        m_actorControl.SelectActor(0).FadeOut(3);
        m_actorControl.SelectActor(1).FadeOut(1);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventControl
{
    private Action[] AllEvent;
    public StoryEventControl(int eventCount)
    { 
        AllEvent = new Action[eventCount];
    }
    public void AddEvent(int eventNum, Action action)
    {
        if (eventNum < 0 || eventNum >= AllEvent.Length)
        {
            return;
        }
        AllEvent[eventNum] += action;
    }
    public void PlayEvent(int eventNum)
    {
        if (eventNum < 0 || eventNum >= AllEvent.Length)
        {
            return;
        }
        AllEvent[eventNum]?.Invoke();
    }
}

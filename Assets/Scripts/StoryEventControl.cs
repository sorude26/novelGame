using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventControl
{
    private List<IEnumerator[]> m_allEvent;
    public int AllEventCount { get => m_allEvent.Count; }
    public StoryEventControl()
    { 
        m_allEvent = new List<IEnumerator[]>();
    }
    public void AddEvent(IEnumerator[] action)
    {
        m_allEvent.Add(action);
    }
    public IEnumerator PlayEvent(int eventNum)
    {
        if (eventNum >= 0 && eventNum < m_allEvent.Count)
        {
            for (int i = 0; i < m_allEvent[eventNum].Length; i++)
            {
                yield return m_allEvent[eventNum][i];
            }
        }
    }
}

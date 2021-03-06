using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour, IStoryControl
{
    [SerializeField]
    string[] m_allText = default;
    [SerializeField]
    Text m_text = default;
    [SerializeField]
    Text m_nameText = default;
    [SerializeField]
    float m_defaultViewSpeed = 0.1f;
    [SerializeField]
    DataLoadController m_event = default;

    private int m_currentIndexCount = 0;
    float m_viewSpeed = default;
    string m_viewText = default;
    bool m_rine = default;
    bool m_skip = default;
    public event Action OnViewLetter;
    public event Action<int> OnViewLineStart;
    public event Action<int> OnViewLineEnd;
    public event Action OnTextEnd;
    public int EventCount { get; private set; }
    public int TextCount { get => m_allText.Length; }
    public bool View { get; private set; }
    bool IStoryControl.ActionNow { get => m_rine; }
    public void StartSet()
    {
        
    }
    public void StartStory()
    {
        if (View)
        {
            return;
        }
        StartCoroutine(ViewStory());
    }
    public void SetName(string name)
    {
        m_nameText.text = name;
    }
    public void ChangeSpeed(float speed)
    {
        m_viewSpeed = speed;
    }
    IEnumerator ViewStory()
    {
        View = true;
        EventCount = 0;
        m_currentIndexCount = 0;
        m_viewSpeed = m_defaultViewSpeed;
        while (EventCount < m_event.StoryCount)
        {
            m_viewText = "";
            m_text.text = m_viewText;
            OnViewLineStart?.Invoke(EventCount);
            yield return m_event.GetEvent(EventCount);
            OnViewLineEnd?.Invoke(EventCount);
            EventCount++;
            m_rine = false;
            m_skip = false;
            yield return WaitInput();
        }
        View = false;
        OnTextEnd?.Invoke();
    }
    public IEnumerator ViewText(IEnumerator textEnd = null)
    {
        m_rine = true;
        int letterCount = 0;
        while (letterCount < m_allText[m_currentIndexCount].Length && !m_skip)
        {
            m_viewText += m_allText[m_currentIndexCount][letterCount];
            m_text.text = m_viewText;
            OnViewLetter?.Invoke();
            yield return WaitTime(m_viewSpeed / m_allText[m_currentIndexCount].Length);
            letterCount++;
        }
        m_viewText = m_allText[m_currentIndexCount];
        m_text.text = m_viewText;
        yield return textEnd;
        m_currentIndexCount++;
        OnViewLetter = null;
        m_skip = false;
    }
    public IEnumerator ViewText(string text,IEnumerator textEnd = null)
    {
        m_rine = true;
        int letterCount = 0;
        while (letterCount < text.Length && !m_skip)
        {
            m_viewText += text[letterCount];
            m_text.text = m_viewText;
            OnViewLetter?.Invoke();
            yield return WaitTime(m_viewSpeed);
            letterCount++;
        }
        m_viewText = text;
        m_text.text = m_viewText;
        yield return textEnd;
        m_currentIndexCount++;
        OnViewLetter = null;
        m_skip = false;
    }
    IEnumerator WaitInput()
    {
        while (!m_skip )
        {
            yield return null;
        }
        m_skip = false;
    }
    IEnumerator WaitTime(float time)
    {
        float timer = 0;
        while (!m_skip && timer < time)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
    public void Skip()
    {
        if (View)
        {
            m_skip = true;
        }
    }
}

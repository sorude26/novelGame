using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextControl : MonoBehaviour
{
    [SerializeField]
    string[] m_allText = default;
    [SerializeField]
    Text m_text = default;
    [SerializeField]
    Text m_letter = default;
    [SerializeField]
    float m_defaultViewSpeed = 0.1f;
    float m_viewSpeed = default;
    public bool View { get; private set; }
    public int CurrentIndexCount { get; private set; }
    string m_viewText = default;
    bool m_skip = false;

    public event Action OnViewLetter;
    public event Action<int> OnViewLineStart;
    public event Action<int> OnViewLineEnd;
    public event Action OnTextEnd;
    public void NextText()
    {
        if (View)
        {
            Skip();
        }
        else
        {
            StartCoroutine(ViewStory());
        }
    }
    public void ChangeSpeed(float speed)
    {
        m_viewSpeed = speed;
    }
    IEnumerator ViewStory()
    {
        View = true;
        CurrentIndexCount = 0;
        m_viewSpeed = m_defaultViewSpeed;
        while (CurrentIndexCount < m_allText.Length)
        {
            m_viewText = "";
            m_text.text = m_viewText;
            OnViewLineStart?.Invoke(CurrentIndexCount);
            yield return ViewText();
            OnViewLineEnd?.Invoke(CurrentIndexCount - 1);
            yield return WaitInput();
        }
        View = false;
        OnTextEnd?.Invoke();
    }
    IEnumerator ViewText()
    {
        int letterCount = 0;
        while (letterCount < m_allText[CurrentIndexCount].Length && !m_skip)
        {
            m_viewText += m_allText[CurrentIndexCount][letterCount];
            m_text.text = m_viewText;
            OnViewLetter?.Invoke();
            yield return WaitTime(m_viewSpeed);
            letterCount++;
        }
        m_viewText = m_allText[CurrentIndexCount];
        m_text.text = m_viewText;
        CurrentIndexCount++;
        OnViewLetter = null;
        m_skip = false;
    }
    IEnumerator WaitInput()
    {
        while (!m_skip)
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
    void Skip()
    {
        m_skip = true;
    }
}
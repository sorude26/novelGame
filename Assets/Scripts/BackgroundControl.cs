using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundControl : MonoBehaviour
{
    [SerializeField]
    Image m_backgroundImage = default;
    [SerializeField]
    Sprite[] m_backgroundSprite = default;
    Color m_currentColor = Color.white;
    bool m_fadeChange = false;
    bool m_changeColor = false;
    bool m_skip = false;
    
    public void Skip()
    {
        if (m_skip)
        {
            return;
        }
        m_skip = true;
        StartCoroutine(CheckSkip());
    }
    public void StartChangeBackground(float time,int number)
    {
        if (m_fadeChange)
        {
            return;
        }
        m_fadeChange = true;
        StartCoroutine(FadeChangeBackground(time, number));
    }
    void ChangeBackground(int number)
    {
        if (m_backgroundImage == null || number >= m_backgroundSprite.Length || number < 0)
        {
            return;
        }
        m_backgroundImage.sprite = m_backgroundSprite[number];
    }
    IEnumerator FadeChangeBackground(float time, int number)
    {
        yield return ChangeColor(time / 2, Color.black);
        ChangeBackground(number);
        yield return ChangeColor(time / 2, Color.white);
        m_fadeChange = false;
    }
    IEnumerator ChangeColor(float time, Color color)
    {
        float a = 0;
        if (time <= 0)
        {
            a = 1;
            time = 1;
        }    
        float b = 1 / time;
        while (a < 1)
        {
            a += b * Time.deltaTime;
            if (a >= 1 && !m_skip)
            {
                a = 1;
            }
            var c = Color.Lerp(m_currentColor, color, a);
            m_backgroundImage.color = c;
            yield return null;
        }
        m_currentColor = color;
        m_backgroundImage.color = color;
        m_changeColor = false;
    }
    IEnumerator CheckSkip()
    {
        while (m_fadeChange || m_changeColor)
        {
            yield return null;
        }
        m_skip = false;
    }
}

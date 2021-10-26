using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour, IStoryControl
{
    [SerializeField]
    int m_id = default;
    [SerializeField]
    RectTransform m_rect = default;
    [SerializeField]
    Image m_characterImage = default;
    [SerializeField]
    Image m_faceImage = default;
    [SerializeField]
    CharacterData m_data = default;
    [SerializeField]
    Animator m_animator = default;
    public int ID { get => m_id; }
    public string Name { get => m_data.Name; }
    public RectTransform Rect { get => m_rect; }
    Color m_currentColor = Color.white;
    public bool ActionNow { get => m_move || m_changeColor || m_action; }
    bool m_move = false;
    bool m_changeColor = false;
    bool m_action = false;
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
    public void ChangeImage(int number)
    {
        if (m_characterImage == null || number >= m_data.CharacterSprite.Length || number < 0)
        {
            return;
        }
        m_characterImage.sprite = m_data.CharacterSprite[number];
    }
    public void ChangeFace(int number)
    {
        if (m_faceImage == null || number >= m_data.FaceSprite.Length || number < 0)
        {
            return;
        }
        m_faceImage.sprite = m_data.FaceSprite[number];
    }
    public void StartChangeColor(float time, Color color)
    {
        if (m_changeColor)
        {
            return;
        }
        m_changeColor = true;
        StartCoroutine(ChangeColor(time, color));
    }
    public void StartChangeColor(float time, Color color,Action action)
    {
        if (m_changeColor)
        {
            return;
        }
        m_changeColor = true;
        m_action = true;
        StartCoroutine(ChangeColor(time, color, action));
    }
    public void StartMoveStraight(float time, Vector2 start, Vector2 goal)
    {
        if (m_move)
        {
            return;
        }
        m_move = true;
        StartCoroutine(MoveStraight(time, start, goal));
    }
    public void StartMoveStraight(float time, Vector2 start, Vector2 goal,Action action)
    {
        if (m_move)
        {
            return;
        }
        m_move = true;
        m_action = true;
        StartCoroutine(MoveStraight(time, start, goal, action));
    }
    public void FadeIn(float time)
    {
        m_currentColor = Color.clear;
        StartChangeColor(time, Color.white);
    }
    public void FadeIn(float time,Action action)
    {
        m_currentColor = Color.clear;
        StartChangeColor(time, Color.white, action);
    }
    public void FadeOut(float time)
    {
        StartChangeColor(time, Color.clear);
    }
    public void FadeOut(float time,Action action)
    {
        StartChangeColor(time, Color.clear, action);
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
            if (a >= 1 || m_skip)
            {
                a = 1;
            }
            var c = Color.Lerp(m_currentColor, color, a);
            m_characterImage.color = c;
            m_faceImage.color = c;
            yield return null;
        }
        m_currentColor = color;
        m_characterImage.color = color;
        m_faceImage.color = color;
        m_changeColor = false;
    }
    IEnumerator MoveStraight(float time, Vector2 start, Vector2 goal)
    {
        float a = 0;
        if (time <= 0)
        {
            a = 1;
            time = 1;
        }
        float b = 1 / time;
        while (a < 1 && !m_skip)
        {
            a += b * Time.deltaTime;
            if (a >= 1)
            {
                a = 1;
            }
            transform.position = Vector2.Lerp(start, goal, a);
            yield return null;
        }
        transform.position = goal;
        m_move = false;
    }
    IEnumerator CheckSkip()
    {
        while (m_move || m_changeColor || m_action)
        {
            yield return null;
        }
        m_skip = false;
    }
    IEnumerator ChangeColor(float time, Color color,Action action)
    {
        yield return ChangeColor(time, color);
        action?.Invoke();
        m_action = false;
    }
    IEnumerator MoveStraight(float time, Vector2 start, Vector2 goal,Action action)
    {
        yield return MoveStraight(time, start, goal);
        action?.Invoke();
        m_action = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StoryData : ScriptableObject
{
    [SerializeField]
    string[] m_story = default;
    [SerializeField]
    int[] m_storyEvent = default;
    [SerializeField]
    int[] m_talkCharacter = default;
    [SerializeField]
    int[] m_background = default;
    public string[] Story { get => m_story; }
    public int[] StoryEvent { get => m_storyEvent; }
    public int[] TalkCharacter { get => m_talkCharacter; }
    public int[] Background { get => m_background; }
}

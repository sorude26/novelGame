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
    private void Start()
    {
        m_actorControl.AddActor(m_characterData.GetCharacter(0), 1);

        m_actorControl.AddActor(m_characterData.GetCharacter(1), 6);
    }
}

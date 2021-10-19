using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorControl : MonoBehaviour
{
    [SerializeField]
    RectTransform m_rect = default;
    [SerializeField]
    RectTransform[] m_actorPos = default;
    List<CharacterControl> m_actorList = default;
    private void Awake()
    {
        m_actorList = new List<CharacterControl>();
    }
    public void AddActor(CharacterControl character,int posNumber)
    {
        if (posNumber >= m_actorPos.Length || posNumber < 0)
        {
            return;
        }
        var actor = Instantiate(character);
        actor.Rect.SetParent(m_rect);
        actor.Rect.position = m_actorPos[posNumber].position;
        m_actorList.Add(actor);
    }
}

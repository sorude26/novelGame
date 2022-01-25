using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorControl : MonoBehaviour, IStoryControl
{
    [SerializeField]
    RectTransform m_rect = default;
    [SerializeField]
    RectTransform[] m_actorPos = default;
    List<CharacterControl> m_actorList = default;
    public Vector2 GetPos(int number)
    {
        return m_actorPos[number].position;
    }
    bool IStoryControl.ActionNow
    {
        get
        {
            foreach (var actor in m_actorList)
            {
                if (actor.ActionNow)
                {
                    return true;
                }
            }
            return false;
        }
    }

    private void Awake()
    {
        m_actorList = new List<CharacterControl>();
    }
    public void AddActor(CharacterControl character, int posNumber)
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
    public CharacterControl TargetActor(int id)
    {
        return m_actorList.Where(c => c.ID == id).FirstOrDefault();
    }
    public CharacterControl SelectActor(int number)
    {
        if (number >= m_actorList.Count || number < 0)
        {
            return null;
        }
        return m_actorList[number];
    }
    public IEnumerator SelectActorMove(int number,int target, float time)
    {
        return SelectActor(number).StartMoveStraight(time, SelectActor(number).transform.position, m_actorPos[target].position);
    }
    public void Skip()
    {
        foreach (var actor in m_actorList)
        {
            actor.Skip();
        }
    }
}

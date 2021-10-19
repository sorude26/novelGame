using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AllCharacterData : ScriptableObject
{
    [SerializeField]
    CharacterControl[] m_allCharacter = default;
    public CharacterControl GetCharacter(int id)
    {
        return m_allCharacter.Where(c => c.ID == id).FirstOrDefault();
    }
}

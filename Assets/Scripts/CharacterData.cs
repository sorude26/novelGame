using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterData : ScriptableObject
{
    [SerializeField]
    string m_characterName = default;
    [SerializeField]
    Sprite[] m_characterSprite = default;
    [SerializeField]
    Sprite[] m_faceSprites = default;
    public string Name { get => m_characterName; }
    public Sprite[] CharacterSprite { get => m_characterSprite; }
    public Sprite[] FaceSprite { get => m_faceSprites; }
}

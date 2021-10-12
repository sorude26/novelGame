using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    [SerializeField]
    int m_id = default;
    [SerializeField]
    string m_name = default;
    [SerializeField]
    Image m_characterImage = default;
    [SerializeField]
    Image m_faceImage = default;
    [SerializeField]
    Sprite[] m_characterSprites = default;
    [SerializeField]
    Sprite[] m_faceSprites = default;
    [SerializeField]
    Animator m_animator = default;
    public int ID { get => m_id; }
    public string Name { get => m_name; }
    public void ChangeImage(int number)
    {
        if (m_characterImage == null || number >= m_characterSprites.Length || number < 0)
        {
            return;
        }
        m_characterImage.sprite = m_characterSprites[number];
    }
    public void ChangeFace(int number)
    {
        if (m_faceImage == null || number >= m_faceSprites.Length || number < 0)
        {
            return;
        }
        m_faceImage.sprite = m_faceSprites[number];
    }
}

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
    public void ChangeBackground(int number)
    {
        if (m_backgroundImage == null || number >= m_backgroundSprite.Length || number < 0)
        {
            return;
        }
        m_backgroundImage.sprite = m_backgroundSprite[number];
    }
}

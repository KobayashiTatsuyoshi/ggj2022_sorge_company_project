using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyBehaviour : MonoBehaviour
{
    [SerializeField] Text m_text;
    [SerializeField] float m_animTime;
    int prev = 0;
    
    public int View { 
        set
        {
            if (prev != value)
            {
                AnimationText(value);
            }
        }
    }

    void AnimationText(int value)
    {
        StartCoroutine(IntegerAnimation(value));
    }

    IEnumerator IntegerAnimation(int value)
    {
        int _prev = prev;
        prev = value;
        float timer = m_animTime;
        while (timer > 0)
        {
            float t = 1f - timer / m_animTime;
            t = t * t * t;
            SetText((int)Mathf.Lerp(_prev, value, t));
            yield return null;
            timer -= Time.deltaTime;
        }
        SetText(value);
    }

    void SetText(int value)
    {
        m_text.text = $"${value:00000}";
    }
}

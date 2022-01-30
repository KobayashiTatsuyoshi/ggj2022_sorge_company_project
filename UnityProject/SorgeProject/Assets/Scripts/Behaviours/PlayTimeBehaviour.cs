using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimeBehaviour : MonoBehaviour
{
    [SerializeField] Text m_text;

    public int View
    {
        set
        {
            var init = new DateTime(1900, 1, 1);
            var date = init.AddDays(value);
            m_text.text = $"{date.Year % 100:00}.{date.Month:00}.{date.Day:00}";
        }
    }
}
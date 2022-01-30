using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSlideIn : MonoBehaviour
{
    [SerializeField] Vector2 diff;
    [SerializeField] float animTime;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        float alpha = 0;
        float timer = animTime;
        var dst = rectTransform.anchoredPosition;
        var src = dst - diff;

        canvasGroup.alpha = alpha;
        rectTransform.anchoredPosition = src;

        while (timer > 0)
        {
            float t = 1f - timer / animTime;
            t = Mathf.Sin(0.5f * t * Mathf.PI);
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);
            rectTransform.anchoredPosition = Vector2.Lerp(src, dst, t);
            yield return null;
            timer -= Time.deltaTime;
        }

        canvasGroup.alpha = 1;
        rectTransform.anchoredPosition = dst;
    }
}

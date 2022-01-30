using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public enum NATION_NAME
{
    ALPHA,
    BETA,
    NONE,
}

public class NationMeterController : MonoBehaviour
{

    [SerializeField]
    Slider alphaPowerSlider, betaPowerSlider, alphaMoralSlider, betaMoralSlider;

    [SerializeField]
    Image alphaTrustMeter, betaTrustMeter;

    [SerializeField]
    Sprite[] trustMeterSprites = new Sprite[5];

    [SerializeField]
    Image alphaPowerGauge, betaPowerGauge, alphaMoralGauge, betaMoralGauge;

    [SerializeField]
    Color originColor;
    [SerializeField]
    Color upColor;
    [SerializeField]
    Color downColor;

    const float TRUST_MIN = 0;
    const float TRUST_MAX = 100;

    public enum METER_TYPE
    {
        POWER,
        MORAL,
        TRUST,
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (alphaMoralSlider && betaMoralSlider && alphaPowerSlider && betaPowerSlider && alphaTrustMeter && betaTrustMeter)
        {
            alphaPowerSlider.minValue = betaPowerSlider.minValue = alphaMoralSlider.minValue = betaMoralSlider.minValue = 0f;
            alphaPowerSlider.maxValue = betaPowerSlider.maxValue = alphaMoralSlider.maxValue = betaMoralSlider.maxValue = 100f;
        }
        else
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Null Error", "Set Meter UI variables", "OK");
#endif
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMeterValue(METER_TYPE eType, NATION_NAME eNation, float value)
    {
        switch(eType)
        {
            case METER_TYPE.MORAL:
                switch(eNation)
                {
                    case NATION_NAME.ALPHA:
                        SetSliderValue(alphaMoralSlider, alphaMoralGauge, value);
                        break;
                    case NATION_NAME.BETA:
                        SetSliderValue(betaMoralSlider, betaMoralGauge, value);
                        break;
                }
                break;
            case METER_TYPE.POWER:
                switch (eNation)
                {
                    case NATION_NAME.ALPHA:
                        SetSliderValue(alphaPowerSlider, alphaPowerGauge, value);
                        break;
                    case NATION_NAME.BETA:
                        SetSliderValue(betaPowerSlider, betaPowerGauge, value); ;
                        break;
                }
                break;
            case METER_TYPE.TRUST:
                switch (eNation)
                {
                    case NATION_NAME.ALPHA:
                        SetImageSprite(alphaTrustMeter, value);
                        break;
                    case NATION_NAME.BETA:
                        SetImageSprite(betaTrustMeter, value); ;
                        break;
                }
                break;
        }
    }

    void SetSliderValue(Slider sliderToSet, Image gaugeToSet, float value)
    {
        if (sliderToSet)
        {
            var prev = sliderToSet.value;
            sliderToSet.value = value;
            if (prev != value) StartCoroutine(GaugeColorAnimation(gaugeToSet, prev < value ? upColor : downColor));
        }
    }

    IEnumerator GaugeColorAnimation(Image gaugeToSet, Color tint)
    {
        float fadeTime = 0.3f;
        float keepTime = 1.5f;

        float timer = fadeTime;
        Color prev = gaugeToSet.color;
        while(timer > 0)
        {
            float t = 1f - timer / fadeTime;
            gaugeToSet.color = Color.Lerp(prev, tint, t);
            yield return null;
            timer -= Time.deltaTime;
        }

        yield return new WaitForSeconds(keepTime);

        timer = fadeTime;
        while (timer > 0)
        {
            float t = 1f - timer / fadeTime;
            gaugeToSet.color = Color.Lerp(tint, originColor, t);
            yield return null;
            timer -= Time.deltaTime;
        }
        gaugeToSet.color = originColor;
    }

    void SetImageSprite(Image imageToSet, float value)
    {
        value = Mathf.Clamp(value, TRUST_MIN, TRUST_MAX);
        int index = (int)((value / TRUST_MAX) * (trustMeterSprites.Length));

        index = Mathf.Clamp(index, 0, trustMeterSprites.Length - 1);
        if (index < trustMeterSprites.Length)
        {
            imageToSet.sprite = trustMeterSprites[index];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using SorgeProject.Util;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using SorgeProject.Data;

namespace SorgeProject.Object
{
    [RequireComponent(typeof(AudioSource))]
    public class InfomationBehaviour : Draggable
    {
        float FadeAwayTime = 1.0f;
        Coroutine lifeSpanCoroutine;
        Coroutine colorTransitionCoroutine;

        public NATION_NAME NationName => nationName;
        NATION_NAME nationName;
        [SerializeField] CircleLimitRenderer timerRenderer;
        [SerializeField] Image informationColorTap;
        [SerializeField] Color alphaniaColor, betalandColor, nuetralColor;
        [SerializeField] float colorTransitionTime = 10.0f;

        AudioSource audioSource;
        [SerializeField] AudioClip pickClip, dropClip, destroyClip, purchaseFailClip;
        public string flavorString { get; private set; }

        public float LifeTime { get; private set; }

        public int Cost { get; private set; }
        public int SellCost { get; private set; }

        public int Power { get; private set; }
        public int Moral { get; private set; }
        public int Trust { get; private set; }
        public string Title { get; private set; }

        public int EventId { get; private set; }

        public bool IsHand { get; set; } = false;

        public override void OnChanged(IDropable prev, IDropable next)
        {
            if (next is HandBehaviour)
            {
                if (prev is RegionBehaviour)
                {
                    print("購入イベント");
                    Controller.PlayerDataConroller.Instance.Purchase(this);
                    IsHand = true;
                    StopLifeSpan();
                    StartColorTransition();
                }
            }
            else if (next is RegionBehaviour nextRegion)
            {
                if (prev is RegionBehaviour prevRegion)
                {
                    print("移動イベント");
                    Controller.PlayerDataConroller.Instance.Move(this, prevRegion, nextRegion);
                    StopLifeSpan();
                }
                else if (prev is HandBehaviour)
                {
                    print("売却イベント");
                    IsHand = false;
                    Controller.PlayerDataConroller.Instance.Sell(this, nextRegion);
                    StopColorTransition();
                }
                StartFadeAwayAndDestroy(CanvasGroup, FadeAwayTime);
            }
            audioSource.clip = dropClip;
            audioSource.Play();
        }

        public void SetData(InfoData data, float lifeTime, NATION_NAME nation_name)
        {
            LifeTime = lifeTime;
            nationName = nation_name;

            SetNationColor(nation_name);

            Cost = data.price;

            SellCost = (int) Mathf.Round(Mathf.Max((float)Cost, 10f)  * (data.profit + 1f));

            Power = data.power;
            Moral = data.moral;
            Trust = data.trust;

            Title = data.title;
            flavorString = data.flavor;

            EventId = data.event_id;

            GetComponent<View.InfomationView>().SetData(this);
        }

        void Start()
        {
            lifeSpanCoroutine = StartCoroutine(LifeSpan(LifeTime));
        }

        void StopLifeSpan()
        {
            if (lifeSpanCoroutine != null)
            {
                StopCoroutine(lifeSpanCoroutine);
                timerRenderer.Fade();
            }
        }

        public void Destroy()
        {
            StartFadeAwayAndDestroy(CanvasGroup, FadeAwayTime);
        }

        public IEnumerator LifeSpan(float lifeTime)
        {
            float startTime = Time.time;
            float time = lifeTime;

            while (Time.time - startTime < lifeTime)
            {
                time -= Time.deltaTime;
                float value = time / lifeTime;
                timerRenderer.SetValue(value);
               yield return null;
            }

            StartFadeAwayAndDestroy(CanvasGroup, FadeAwayTime);
        }

        void StartFadeAwayAndDestroy(CanvasGroup canvasGroup, float lifeTime)
        {
            if (CanvasGroup)
            {
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
                StartCoroutine(FadeAwayAndDestroy(CanvasGroup, FadeAwayTime));
                audioSource.clip = destroyClip;
                audioSource.Play();
            }
        }

        public IEnumerator FadeAwayAndDestroy(CanvasGroup canvasGroup, float lifeTime)
        {
            if (canvasGroup == null)
                yield break;

            float startTime = Time.time;
            float time = lifeTime;
            float value = 0;

            while (Time.time - startTime < lifeTime)
            {
                time -= Time.deltaTime;
                value = time / lifeTime;
                canvasGroup.alpha = value;
                yield return null;
            }

            GameObject.Destroy(gameObject);
        }

        void StartColorTransition()
        {
            colorTransitionCoroutine = StartCoroutine(ColorTransition());
        }

        void StopColorTransition()
        {
            StopCoroutine(colorTransitionCoroutine);
            SetNationColor(nationName); //変化途中の色を元に戻すため
        }

        IEnumerator ColorTransition()
        {
            if (informationColorTap == null)
                yield break;

            float startTime = Time.time;
            float time = colorTransitionTime;
            float value = 0;
            Color originalColor = informationColorTap.color;

            while (Time.time - startTime < colorTransitionTime)
            {
                time -= Time.deltaTime;
                value = time / colorTransitionTime;
                informationColorTap.color = Color.Lerp(nuetralColor, originalColor, value);
                yield return null;
            }

            nationName = NATION_NAME.NONE;
        }

        void SetNationColor(NATION_NAME eName)
        {
            switch (eName)
            {
                case NATION_NAME.ALPHA:
                    informationColorTap.color = alphaniaColor;
                    break;
                case NATION_NAME.BETA:
                    informationColorTap.color = betalandColor;
                    break;
                case NATION_NAME.NONE:
                default:
                    informationColorTap.color = nuetralColor;
                    break;
            }
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            audioSource.clip = pickClip;
            audioSource.Play();
        }

        public void OnPurchaseFailed()
        {
            audioSource.clip = purchaseFailClip;
            audioSource.Play();
        }
    }
}
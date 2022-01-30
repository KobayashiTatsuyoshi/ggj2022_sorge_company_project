using SorgeProject.Data;
using SorgeProject.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SorgeProject.Controller
{
    public class EventController : MonoBehaviour, IController
    {
        [SerializeField] string sheetName = "event_data";
        [SerializeField] float eventKeepTime = 60;
        [SerializeField] float eventPopTime = 40;
        [SerializeField] Object.EventBehaviour eventPrefab;
        [SerializeField] Transform eventRoot;
        List<EventData> eventDatas;

        Coroutine currentEventTimer;
        Coroutine nextEventTimer;

        public UnityEvent eventStarted;
        public UnityEvent eventCleared;
        public UnityEvent eventFailed;

        public bool IsInitialized { get => eventDatas != null; }

        public void PlayUpdate() {
            if (currentEventTimer == null && nextEventTimer == null)
            {
                nextEventTimer = Wait();
            }
        }

        public void PlayStart() { }

        void IController.Setup()
        {
            StartCoroutine(DataLoad());
            Instance = this;
        }

        private IEnumerator DataLoad()
        {
            Debug.Log("データロード中", this);
            var gen = MasterDataLoaderFromSheet.LoadData<EventData>(sheetName);
            yield return gen;
            var data = gen.Current as List<EventData>;
            eventDatas = data;
            Debug.Log("データロード終了", this);
        }

        private Coroutine Wait()
        {
            return StartCoroutine(Timer(eventPopTime, () => {
                nextEventTimer = null;
                AddEvent();
                currentEventTimer = Pop();
            }));
        }

        private Coroutine Pop()
        {
            return StartCoroutine(Timer(eventKeepTime, () =>
            {
                currentEventTimer = null;
                RemoveEvent();
            }));
        }

        Object.EventBehaviour eventInstance;
        EventData currentData;

        public void AddEvent()
        {
            Debug.Log("イベント開始");
            int eventIdx = Random.Range(0, eventDatas.Count);
            currentData = eventDatas[eventIdx];
            eventInstance = Instantiate(eventPrefab, eventRoot);
            eventInstance.SetData(currentData.title, currentData.flavor);
            eventInstance.Pop();
            eventStarted.Invoke();
        }

        public void RemoveEvent()
        {
            Debug.Log("イベント失敗");

            RegionController.Instance.AddParameter(NATION_NAME.ALPHA, currentData.failed_power, currentData.failed_moral, 0);
            RegionController.Instance.AddParameter(NATION_NAME.BETA, currentData.failed_power, currentData.failed_moral, 0);

            eventInstance.Destroy();
            eventInstance = null;
            currentData = null;
            eventFailed.Invoke();
        }

        public void ClearEvent(NATION_NAME target)
        {
            Debug.Log("イベント成功");

            RegionController.Instance.AddParameter(target, currentData.success_power, currentData.success_moral, 0);

            eventInstance.Destroy();
            eventInstance = null;
            currentData = null;
            eventCleared.Invoke();
        }

        public void CheckEventClear(int event_id, NATION_NAME receive)
        {
            if (eventInstance != null && currentData != null && currentData.id == event_id)
            {
                ClearEvent(receive);
            }
        }

        private IEnumerator Timer(float limit, System.Action Callback)
        {
            float time = limit;
            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }
            Callback();
        }

        public static EventController Instance { get; private set; }
    }
}

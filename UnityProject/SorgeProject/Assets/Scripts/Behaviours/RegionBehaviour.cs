using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SorgeProject.Util;

namespace SorgeProject.Object
{
    public class RegionBehaviour : MonoBehaviour, IDropable
    {
        [SerializeField] NATION_NAME m_name;
        EnumerabledLocator locator;
        private List<InfomationBehaviour> infomations;

        public NATION_NAME Name { get => m_name; }

        private void Start()
        {
            infomations = new List<InfomationBehaviour>();
            locator = GetComponent<EnumerabledLocator>();
            for(int i = 0; i < locator.Count; i++)
            {
                infomations.Add(null);
            }
        }

        public bool IsDropable(Draggable draggable)
        {
            if (draggable is InfomationBehaviour infomation)
            {
                if (infomation.NationName == this.Name) return false;
                if (!infomation.IsHand)
                {
                    bool result = Controller.PlayerDataConroller.Instance.IsPurchasable(infomation);
                    if (!result) infomation.OnPurchaseFailed();
                    return result;
                }
                else return true;
            }
            return false;
        }

        public void OnDrop(Draggable draggable)
        {
            SetPosition(draggable);
        }

        public void OnExit(Draggable draggable)
        {
            draggable.transform.SetParent(transform.parent);
            int idx = infomations.IndexOf(draggable as InfomationBehaviour);
            infomations[idx] = null;
        }

        public void Pop<T>(Draggable draggable) where T : Draggable
        {
            SetPosition(draggable);
            draggable.Initialize(this);
        }

        public void SetPosition(Draggable draggable)
        {
            draggable.transform.SetParent(transform);
            for (int i = 0; i < infomations.Count; i++)
            {
                Debug.Log(infomations[i]);
                if (infomations[i] != null) continue;
                infomations[i] = draggable as InfomationBehaviour;
                (draggable.transform as RectTransform).anchoredPosition = locator.GetPosition(i);
                break;
            }
        }

        public void ReceiveInfomation(InfomationBehaviour infomation)
        {
            Controller.RegionController.Instance.AddParameter(m_name, infomation.Power, infomation.Moral, infomation.Trust);
            Controller.EventController.Instance.CheckEventClear(infomation.EventId, Name);
        }
    }
}

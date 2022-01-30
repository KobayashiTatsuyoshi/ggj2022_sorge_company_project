using UnityEngine;

using SorgeProject.Util;
using SorgeProject.Controller;
using System.Collections.Generic;
using System;

namespace SorgeProject.Object
{
    public class HandBehaviour : MonoBehaviour, IDropable
    {
        private List<InfomationBehaviour> infomations;
        private EnumerabledLocator locator;

        [SerializeField] int handCount = 7;

        private void Start()
        {
            locator = GetComponent<EnumerabledLocator>();
            infomations = new List<InfomationBehaviour>();
            for (int i = 0; i < handCount; i++)
            {
                infomations.Add(null);
            }
        }

        public bool IsDropable(Draggable draggable)
        {
            if (draggable is InfomationBehaviour infomation)
            {
                bool result = infomations.Contains(infomation) || PlayerDataConroller.Instance.IsPurchasable(infomation);
                if (!result) infomation.OnPurchaseFailed();
                return result;
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
        }

        public void SetPosition(Draggable draggable)
        {
            var idx = infomations.IndexOf(draggable as InfomationBehaviour);
            draggable.transform.SetParent(transform);
            (draggable.transform as RectTransform).anchoredPosition = locator.GetPosition(idx);
        }

        public void PushInfomation(InfomationBehaviour infomation)
        {
            for (int i = 0; i < handCount; i++)
            {
                if (infomations[i] == null)
                {
                    infomations[i] = infomation;
                    return;
                }
            }
        }

        internal void RemoveInfomation(InfomationBehaviour infomation)
        {
            int idx = infomations.IndexOf(infomation);
            if (idx == -1)
            {
                Debug.LogError("not have infomation");
                Debug.Break();
            }
            infomations[idx] = null;
        }
    }
}

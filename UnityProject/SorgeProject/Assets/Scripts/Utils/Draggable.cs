using UnityEngine;
using UnityEngine.EventSystems;

namespace SorgeProject.Util
{
    public interface IDropable
    {
        bool IsDropable(Draggable draggable);
        void OnDrop(Draggable draggable);
        void OnExit(Draggable draggable);
    }

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Draggable : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        Vector3 originPos;

        protected CanvasGroup CanvasGroup { get
            {
                if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        private CanvasGroup _canvasGroup;

        public IDropable Parent { get; private set; }

        public void Initialize(IDropable dropable)
        {
            Parent = dropable;
        }

        virtual public void OnBeginDrag(PointerEventData eventData)
        {
            if (Parent != null) Parent.OnExit(this);
            CanvasGroup.blocksRaycasts = false;
            originPos = transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        virtual public void OnEndDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = true;
            var dropObject = eventData.pointerEnter;
            var drop = dropObject?.GetComponent<IDropable>();

            if (drop == null || !drop.IsDropable(this))
            {
                transform.position = originPos;
                if (Parent != null) Parent.OnDrop(this);
                return;
            }

            if (drop != Parent)
            {
                OnChanged(Parent, drop);
                Parent = drop;
            }
            drop.OnDrop(this);
        }

        public abstract void OnChanged(IDropable prev, IDropable next);
    }
}

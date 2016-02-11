using UnityEngine;

using Serialization;
using Events;

namespace FSM
{
    public class FSMRunner : MonoBehaviour
    {
        public TextAsset fsmAsset;

        private Injector injector = new Injector();

        private FSM _fsm;
        public FSM fsm
        {
            get
            {
                return _fsm;
            }

            set
            {
                if (value != null)
                {
                    _fsm = value;

                    injector.inject(fsm, container);

                    fsm.enter();
                }
            }
        }

        protected void Start()
        {
            Load();
        }
        
        public virtual void Load()
        {
            if (fsmAsset != null)
            {
                fsm = fsmAsset.fsm();
            }
        }

        private Container _container;
        public Container container
        {
            get
            {
                if (_container == null)
                {
                    _container = newContainer();
                }
                return _container;
            }
        }

        protected virtual Container newContainer()
        {
            return new Container()
            {
                { typeof(FSM), fsm },
                { typeof(GameObject), gameObject },
                { typeof(TimeManager.ITimeManager), TimeManager.TimeManager.Instance },
                { typeof(EventListener), EventBus.Instance },
            };
        }
    }
}
using UnityEngine;

using Serialization;
using Events;

namespace Elurnity.FSM
{
    public class FSMRunner : MonoBehaviour
    {
        private Injector injector = new Injector();

        private FSM _fsm;
        public FSM fsm
        {
            get
            {
                if (_fsm == null)
                {
                    _fsm = new FSM();
                }
                return _fsm;
            }
            
            set
            {
                _fsm = value;
                
                if (value != null)
                {
                    container[typeof(FSM)] = fsm;
                    
                    injector.inject(fsm, container);

                    fsm.Enter();
                }
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
            
            set
            {
                _container = value;
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

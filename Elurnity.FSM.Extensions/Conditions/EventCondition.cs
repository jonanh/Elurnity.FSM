
using Elurnity.EventSystem;

namespace Elurnity.FSM
{
    public class EventCondition<T> : Condition, IFSM where T : Event
    {
        public string eventName;
        private bool _listened = false;
        private bool active { get; set; }

        public override bool Eval()
        {
            return _listened;
        }

        public override void Enter()
        {
            eventListener.On<T>(OnEvent);
        }

        public override void Exit()
        {
            eventListener.On<T>(OnEvent);
        }

        private void OnEvent(T evt)
        {
            _listened = true;
        }

        public FSM fsm { get; set; }

        public EventListener eventListener { get; set; }
    }
}

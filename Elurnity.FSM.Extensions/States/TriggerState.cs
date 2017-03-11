using Elurnity.EventSystem;

namespace Elurnity.FSM
{
    public class TriggerEventState<T> : State where T : EventSystem.Event
    {
        T evt = default(T);

        public override void Enter()
        {
            EventBus.Instance.Emit(evt);
        }
    }
}

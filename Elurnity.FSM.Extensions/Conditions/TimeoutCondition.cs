
using Elurnity.TimeManager;

namespace Elurnity.FSM
{
    public class TimeoutCondition : Condition, IFSM, ITickUpdate
    {
        public float timeout = 0f;
        private float _timeout = 0f;

        public override bool Eval()
        {
            return _timeout <= 0f;
        }

        public override void Enter()
        {
            _timeout = timeout;
            timeManager.Add(this);
        }

        public override void Exit()
        {
            timeManager.Remove(this);
        }

        bool ITickUpdate.Update(float time)
        {
            _timeout -= time;
            if (Eval())
            {
                fsm.update();
            }
            return false;
        }

        public ITimeManager timeManager { get; set; }

        public FSM fsm { get; set; }
    }
}

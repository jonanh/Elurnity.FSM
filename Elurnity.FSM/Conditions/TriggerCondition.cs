using System;

namespace Elurnity.FSM
{
    public class TriggerCondition : ParamCondition<bool>
    {
        public override bool Eval()
        {
            if (parameter != null)
            {
                return parameter.value;
            }

            return false;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }
}

using System;

namespace Elurnity.FSM
{
    public class DelegateState : State
    {
        public Action<Context> onEnter;
        public Action<Context> onUpdate;
        public Action<Context> onExit;

        public override void Enter(Context context)
        {
            if (onEnter != null)
            {
                onEnter(context);
            }
        }

        public override void Update(Context context)
        {
            if (onUpdate != null)
            {
                onUpdate(context);
            }
        }

        public override void Exit(Context context)
        {
            if (onExit != null)
            {
                onExit(context);
            }
        }
    }
}
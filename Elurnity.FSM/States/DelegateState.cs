using System;

namespace Elurnity.FSM
{
    public class DelegateState : State
    {
        public Action onEnter;
        public Action onUpdate;
        public Action onExit;

        public override void enter()
        {
            if (onEnter != null)
            {
                onEnter();
            }
        }

        public override void update()
        {
            if (onUpdate != null)
            {
                onUpdate();
            }
        }

        public override void exit()
        {
            if (onExit != null)
            {
                onExit();
            }
        }
    }
}
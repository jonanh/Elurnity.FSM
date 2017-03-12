using System;

namespace Elurnity.FSM
{
    public class State
    {
        public string name;

        public virtual void Enter(Context context)
        {
        }

        public virtual void Update(Context context)
        {
        }

        public virtual void Exit(Context context)
        {
        }
    }
}

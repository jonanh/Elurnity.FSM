using System;

namespace Elurnity.FSM
{
    public class State
    {
        public string name;

        public virtual void enter()
        {
        }

        public virtual void update()
        {
        }

        public virtual void exit()
        {
        }
    }
}

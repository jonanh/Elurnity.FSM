using System;
using System.Collections.Generic;

namespace Elurnity.FSM
{
    [Serializable]
    public partial class FSM : State
    {
        public List<State> states = new List<State>();
        public List<Param> parameters = new List<Param>();
        public Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();

        public State DefaultState { get; set; }

        private Context context;

        private State _CurrentState;
        public State CurrentState
        {
            get
            {
                return _CurrentState;
            }

            set
            {
                var previous = _CurrentState;

                _CurrentState = value;

                context?.executionContext?.ChangeState(this, previous, value);
            }
        }

        public override void Enter(Context context)
        {
            this.context = context;

            this.context?.executionContext?.Enter(this);
        }

        public override void Update(Context context)
        {
            this.context?.executionContext?.Update(this);
        }

        public override void Exit(Context context)
        {
            this.context?.executionContext?.Exit(this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace Elurnity.FSM
{
    public class ExecutionContext
    {
        private Context context;
        private int updateLock = 0;
        private int updateRequested = 0;

        private int maxVisitsPerUpdate = 6;
		private readonly Queue<FSM> queue = new Queue<FSM>();
        private readonly Dictionary<State, int> visitedStates = new Dictionary<State, int>();

        public ExecutionContext(Context context)
        {
            this.context = context;
        }

        public void ChangeState(FSM fsm, State previous, State state)
        {
            if (previous != null)
            {
                previous.Exit(context);

                foreach (var transition in fsm.GetTransitions(previous))
                {
                    transition.Exit();
                }
            }

            if (state != null)
            {
                state.Enter(context);

                foreach (var transition in fsm.GetTransitions(state))
                {
                    transition.Enter();
                }
            }
        }

        internal void Enter(FSM fsm)
        {
            if (queue.Peek() != fsm)
            {
                queue.Enqueue(fsm);

                Update(fsm, true);
            }
        }

        internal void Update(FSM fsm)
        {
            Update(fsm, false);
        }

        internal void Exit(FSM fsm)
        {
            if (queue.Peek() == fsm)
            {
                queue.Dequeue();
            }
        }

        private void Update(FSM fsm, bool updateAfterEnter)
        {
            Interlocked.Exchange(ref updateRequested, 1);

            if (Interlocked.Exchange(ref updateLock, 1) == 1)
            {
                return;
            }

            visitedStates.Clear();
            bool switched = false;

            do
            {
                switched = false;

                // 1.- Check the transitions from the "enter" state if we just entered the nested FSM
                if (updateAfterEnter)
                {
                    updateAfterEnter = false;
                    foreach (var state in fsm.EnterStates)
                    {
                        if (switched = EvalTransitions(fsm, state))
                        {
                            break;
                        }
                    }
                }

                // 2.- Check the transitions from the "any" state
                if (!switched)
                {
                    foreach (var state in fsm.AnyStates)
                    {
                        if (switched = EvalTransitions(fsm, state))
                        {
                            break;
                        }
                    }
                }

                // 3.- Check the default state
                if (!switched && fsm.CurrentState == null && fsm.DefaultState != null)
                {
                    fsm.CurrentState = fsm.DefaultState;
                    switched = true;
                }

                // 4.- Check the transitions from the current state
                if (!switched)
                {
                    switched = EvalTransitions(fsm, fsm.CurrentState);
                }

                if (!HasVisitedStateBelowLimit(fsm.CurrentState))
                {
                    break;
                }

                fsm.CurrentState?.Update(context);

                if (fsm.CurrentState == null && fsm.DefaultState == null)
                {
                    break;
                }
            }
            while (switched || Interlocked.Exchange(ref updateRequested, 0) == 1);

            Interlocked.Exchange(ref updateLock, 0);
        }

        public static bool EvalTransitions(FSM fsm, State fromState)
        {
            Transition transition = null;
            foreach (var _transition in fsm.GetTransitions(fromState))
            {
                if (_transition != null && _transition.eval())
                {
                    transition = _transition;
                    break;
                }
            }

            if (transition != null)
            {
                foreach (var trigger in fsm.triggers)
                {
                    trigger.value = false;
                }

                fsm.CurrentState = transition.to;
                return true;
            }
            return false;
        }

        private bool HasVisitedStateBelowLimit(State state)
        {
            int visits = 0;
            if (state != null && !visitedStates.TryGetValue(state, out visits) || visits < maxVisitsPerUpdate)
            {
                visitedStates[state] = visits + 1;
                return true;
            }

            return false;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;

namespace FSM
{
    [Serializable]
    public class FSM : State
    {
        public List<State> states = new List<State>();
        public List<Param> parameters = new List<Param>();
        public Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();

        public State defaultState { get; set; }

        public IEnumerable<State> state(string name)
        {
            return
                from state in states
                where states != null && state.name.ToLower() == name.ToLower()
                select state;
        }
        
        public IEnumerable<Param> param(string name)
        {
            return
                from p in parameters
                where p != null && p.name.ToLower() == name.ToLower()
                select p;
        }
        
        public IEnumerable<Param<T>> param<T>(string name)
        {
            return
                from p in param(name)
                where p is Param<T>
                select p as Param<T>;
        }
        
        public IEnumerable<Trigger> trigger(string name)
        {
            return
                from t in triggers
                where t.name.ToLower() == name.ToLower()
                select t;
        }
        
        public IEnumerable<State> anyStates
        {
            get
            {
                return state("any");
            }
        }

        public IEnumerable<State> enterStates
        {
            get
            {
                return state("enter");
            }
        }

        public IEnumerable<State> exitStates
        {
            get
            {
                return state("exit");
            }
        }
        
        public IEnumerable<Trigger> triggers
        {
            get
            {
                return
                    from param in parameters
                    where param is Trigger
                    select (Trigger)param;
            }
        }

        public IEnumerable<Transition> getTransitions(State state)
        {
            List<Transition> list;
            if (transitions.TryGetValue(state, out list))
            {
                return list;
            }
            return Enumerable.Empty<Transition>();
        }

        public IEnumerable<State> getTransitionsStates(State state)
        {
            return
                from transition in getTransitions(state)
                where transition != null
                select transition.to;
        }

        private State _currentState;
        public State currentState
        {
            get
            {
                return _currentState;
            }

            set
            {
                if (_currentState != null)
                {
                    _currentState.exit();
                    foreach (Transition transition in getTransitions(_currentState))
                    {
                        transition.exit();
                    }
                }
                
                _currentState = value;
                
                if (_currentState != null)
                {
                    _currentState.enter();

                    foreach (Transition transition in getTransitions(_currentState))
                    {
                        transition.enter();
                    }
                }
            }
        }

        public override void enter()
        {
            update();
        }
        
        [fsIgnore]
        public int maxVisitsPerUpdate = 1000;
        private Dictionary<State, int> visitedStates = new Dictionary<State, int>();
        private bool visitedBelowMax(State state)
        {
            int visits = 0;
            if (state != null)
            {
                if (!visitedStates.TryGetValue(state, out visits) || visits < 6)
                {
                    visitedStates[state] = visits + 1;
                    return true;
                }
            }
            return false;
        }

        private bool updateAfterEnter = false;
        private bool updateLocked = false;
        private bool updateRequested = false;
        public override void update()
        {
            updateRequested = true;
            
            if (updateLocked)
            {
                return;
            }
            
            visitedStates.Clear();
            updateLocked = true;
            bool switched = false;
            
            do
            {
                updateRequested = false;
                switched = false;

                if (updateAfterEnter)
                {
                    updateAfterEnter = false;
                    foreach (var state in enterStates)
                    {
                        if (switched = evalTransitions(state))
                        {
                            break;
                        }
                    }
                }
                
                if (!switched)
                {
                    foreach (var state in anyStates)
                    {
                        if (switched = evalTransitions(state))
                        {
                            break;
                        }
                    }
                }
                
                if (!switched && currentState == null && defaultState != null)
                {
                    currentState = defaultState;
                    switched = true;
                }
                
                if (!switched)
                {
                    switched = evalTransitions(currentState);
                }

                if (currentState != null && !visitedBelowMax(currentState))
                {
                    break;
                }
                
                if (currentState != null)
                {
                    currentState.update();
                }

                if (currentState == null && defaultState == null)
                {
                    break;
                }
            }
            while (switched || updateRequested);

            updateLocked = false;
        }

        private bool evalTransitions(State fromState)
        {
            Transition transition = null;
            foreach (var _transition in getTransitions(fromState))
            {
                if (_transition != null && _transition.eval())
                {
                    transition = _transition;
                    break;
                }
            }
            
            foreach (Trigger trigger in triggers)
            {
                trigger.value = false;
            }
            
            if (transition != null)
            {
                currentState = transition.to;
                return true;
            }
            return false;
        }
    }
}
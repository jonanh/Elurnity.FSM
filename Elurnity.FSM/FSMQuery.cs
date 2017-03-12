using System;
using System.Linq;
using System.Collections.Generic;

namespace Elurnity.FSM
{
    public partial class FSM
    {
        public IEnumerable<State> States(string name)
        {
            return
                from state in states
                where states != null && state.name.ToLower() == name.ToLower()
                select state;
        }

        public IEnumerable<Param> Params(string name)
        {
            return
                from p in parameters
                where p != null && p.name.ToLower() == name.ToLower()
                select p;
        }

        public IEnumerable<Param<T>> Params<T>(string name)
        {
            return
                from p in Params(name)
                where p is Param<T>
                select p as Param<T>;
        }

        public IEnumerable<Trigger> Triggers(string name)
        {
            return
                from t in triggers
                where t.name.ToLower() == name.ToLower()
                select t;
        }

        public State State(string name)
        {
            return States(name).FirstOrDefault();
        }

        public Param Param(string name)
        {
            return Params(name).FirstOrDefault();
        }

        public Trigger Trigger(string name)
        {
            return Triggers(name).FirstOrDefault();
        }

        public IEnumerable<State> AnyStates
        {
            get
            {
                return States("any");
            }
        }

        public IEnumerable<State> EnterStates
        {
            get
            {
                return States("enter");
            }
        }

        public IEnumerable<State> ExitStates
        {
            get
            {
                return States("exit");
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

        public IEnumerable<Transition> GetTransitions(State state)
        {
            List<Transition> list;
            if (transitions.TryGetValue(state, out list))
            {
                return list;
            }
            return Enumerable.Empty<Transition>();
        }

        public IEnumerable<State> GetTransitionsStates(State state)
        {
            return
                from transition in GetTransitions(state)
                where transition != null
                select transition.to;
        }
    }
}

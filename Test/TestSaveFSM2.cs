using UnityEngine;
using System.Collections.Generic;
using FSM;

namespace Tests
{
    public class TestSaveFSM2 : MonoBehaviour
    {
        public TextAsset fsmAsset;

        public void Start()
        {
            var triggerParam = new Trigger()
            {
                name = "Reset"
            };

            var states = new List<State>()
            {
                new State()
                {
                    name = "0",
                },
                new State()
                {
                    name = "1",
                },
                new State()
                {
                    name = "2",
                },
                new State()
                {
                    name = "Any",
                },
            };

            var transition01 = new List<Transition>()
            {
                new Transition()
                {
                    name = "0 -> 1",
                    to = states[1],
                    conditions = new Condition[]
                    {
                        new TimeoutCondition()
                        {
                            timeout = 2f
                        }
                    }
                },
            };
            
            var transition12 = new List<Transition>()
            {
                new Transition()
                {
                    name = "1 -> 2",
                    to = states[2],
                    conditions = new Condition[]
                    {
                        new TimeoutCondition()
                        {
                            timeout = 2f
                        }
                    }
                },
            };
            
            var transition20 = new List<Transition>()
            {
                new Transition()
                {
                    name = "2 -> 0",
                    to = states[0],
                    conditions = new Condition[]
                    {
                        new TimeoutCondition()
                        {
                            timeout = 2f
                        }
                    }
                },
            };

            var transitionAny = new List<Transition>()
            {
                new Transition()
                {
                    name = "Any -> 0",
                    to = states[0],
                    conditions = new Condition[]
                    {
                        new TriggerCondition
                        {
                            param = "Reset",
                        }
                    }
                },
            };
            
            var fsm = new FSM.FSM()
            {
                states = states,

                parameters =
                {
                    triggerParam,
                },

                defaultState = states[0],

                transitions = {
                    { states[0], transition01 },
                    { states[1], transition12 },
                    { states[2], transition20 },
                    { states[3], transitionAny },
                }
            };

            // Save FSM
            fsmAsset.fsm(fsm);
        }
    }
}
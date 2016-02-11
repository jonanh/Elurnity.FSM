using UnityEngine;
using FSM;

namespace Tests
{
    public class TestSaveFSM : MonoBehaviour
    {
        public TextAsset fsmAsset;

        public void Start()
        {
            var boolParam = new Param<bool>()
            {
                name = "Parameter",
                value = true,
            };

            var intParam = new Param<int>()
            {
                name = "Parameter",
                value = 1
            };

            var simpleStates = new State()
            {
                name = "Foo State",
            };

            var fsm = new FSM.FSM()
            {
                states =
                {
                    simpleStates
                },

                parameters =
                {
                    boolParam,
                    intParam,
                },

                defaultState = simpleStates,
            };

            // Save FSM
            fsmAsset.fsm(fsm);
        }
    }
}
using UnityEngine;
using FSM;
namespace Tests
{
    public class TestSaveFSM2 : MonoBehaviour
    {
        public TextAsset fsmAsset;

        public void Start()
        {
            fsmAsset.fsm(ExampleFSM.fsm());
        }
    }
}
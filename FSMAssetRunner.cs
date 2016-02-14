using UnityEngine;

namespace FSM
{
    public class FSMAssetRunner : FSMRunner
    {
        public TextAsset fsmAsset;
        
        protected void Start()
        {
            fsm = fsmAsset.fsm();
        }
    }
}
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

using Serialization;

namespace FSM
{
    public static class FSMTextAssetExtensions
    {
        public static FSM fsm(this TextAsset textAsset)
        {
            return textAsset.Deserialize<FSM>();
        }

        public static void fsm(this TextAsset textAsset, FSM fsm)
        {
            textAsset.Serialize(fsm);
        }
    }
}

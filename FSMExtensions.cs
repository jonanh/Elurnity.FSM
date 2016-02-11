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
            return JSON.Deserialize<FSM>(textAsset.text);
        }

        public static void fsm(this TextAsset textAsset, FSM fsm)
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(textAsset);
            var dir = Application.dataPath.Replace("Assets", "");
            path = Path.Combine(dir, path);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(JSON.Serialize<FSM>(fsm, true));
                writer.Close();
            }
#endif
        }
    }
}

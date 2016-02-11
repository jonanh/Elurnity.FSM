using UnityEngine;
using UnityEngine.SceneManagement;
using Events;
using AssetBundles;

namespace FSM
{
    public class State
    {
        public string name;

        public virtual void enter()
        {
        }

        public virtual void update()
        {
        }

        public virtual void exit()
        {
        }
    }
    
    public class TriggerEventState<T> : State where T : Events.Event
    {
        T evt = null;

        public override void enter()
        {
            gameobject.Trigger(evt);
        }
        
        public GameObject gameobject { get; set; }
    }
    
    public class LoadSceneState : State
    {
        public string sceneAssetBundle;
        public string levelName;

        public bool isAdditive = true;
        
        public override void enter()
        {
            AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(sceneAssetBundle, levelName, isAdditive);
        }
    }
    
    public class UnloadSceneState : State
    {
        public string levelName;

        public bool isAdditive = true;
        
        public override void enter()
        {
            SceneManager.UnloadScene(levelName);
        }
    }
}
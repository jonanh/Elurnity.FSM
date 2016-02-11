using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using FSM;

namespace Tests
{
    [RequireComponent(typeof(FSMRunner))]
    public class TestRunFSM : MonoBehaviour
    {
        public Text prefab;
        Text[] texts;

        IEnumerator Start()
        {
            yield return null;

            List<Text> listTexts = new List<Text>();

            foreach (var state in fsm.states)
            {
                var text = Instantiate(prefab);
                text.gameObject.SetActive(true);
                text.name = state.name;
                text.text = state.name;
                text.transform.SetParent(transform, false);
                listTexts.Add(text);
            }

            texts = listTexts.ToArray();
        }

        void Update()
        {
            if (texts != null)
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    if (fsm.states[i] == fsm.currentState)
                    {
                        texts[i].color = Color.green;
                    }
                    else
                    {
                        texts[i].color = Color.white;
                    }
                }
            }
        }

        private FSM.FSM _fsm;

        protected FSM.FSM fsm
        {
            get
            {
                if (_fsm == null)
                {
                    _fsm = GetComponent<FSMRunner>().fsm;
                }
                return _fsm;
            }
        }

        public void Reset()
        {
            fsm.trigger("Reset").First().value = true;
        }
    }
}
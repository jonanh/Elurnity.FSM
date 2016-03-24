using System;
using System.Linq;
using Events;

using FullSerializer;

namespace FSM
{
    public abstract class Condition
    {
        public string name;

        public abstract bool eval();

        public abstract void enter();

        public abstract void exit();
    }

    public abstract class ParamCondition<T> : Condition, IFSM
    {
        public string param;

        protected Param<T> parameter
        {
            get
            {
                return
                    (from p in fsm.parameters
                     where p is Param<T>
                     where p.name == param
                     select p as Param<T>).FirstOrDefault();
            }
        }

        [fsIgnore]
        public FSM fsm { get; set; }
    }

    public class Condition<T> : ParamCondition<T>, IFSM where T : IComparable
    {
        public enum ComparisonType { Equals, NotEquals, GreaterOrEqualThan, GreaterThan, LessThan, LessOrEqualThan }

        public T value;
        public ComparisonType comparisonType;

        public override bool eval()
        {
            if (parameter != null)
            {
                switch (comparisonType)
                {
                    case ComparisonType.Equals:
                        return value.CompareTo(parameter.value) == 0;

                    case ComparisonType.GreaterOrEqualThan:
                        return value.CompareTo(parameter.value) >= 0;

                    case ComparisonType.GreaterThan:
                        return value.CompareTo(parameter.value) > 0;

                    case ComparisonType.LessOrEqualThan:
                        return value.CompareTo(parameter.value) <= 0;

                    case ComparisonType.LessThan:
                        return value.CompareTo(parameter.value) < 0;

                    case ComparisonType.NotEquals:
                        return value.CompareTo(parameter.value) != 0;
                }
            }

            return false;
        }

        public override void enter()
        {
        }

        public override void exit()
        {
        }
    }

    public class TriggerCondition : ParamCondition<bool>
    {
        public override bool eval()
        {
            if (parameter != null)
            {
                return parameter.value;
            }

            return false;
        }

        public override void enter()
        {
        }

        public override void exit()
        {
        }
    }
    
    public class TimeoutCondition : Condition, IFSM, TimeManager.ITickUpdate
    {
        public float timeout = 0f;
        private float _timeout = 0f;

        public override bool eval()
        {
            return _timeout <= 0f;
        }

        public override void enter()
        {
            _timeout = timeout;
            timeManager.Add(this);
        }

        public override void exit()
        {
            timeManager.Remove(this);
        }

        void TimeManager.ITickUpdate.Update(float time)
        {
            _timeout -= time;
            if (eval())
            {
                fsm.update();
            }
        }

        [fsIgnore]
        public TimeManager.ITimeManager timeManager { get; set; }
        
        [fsIgnore]
        public FSM fsm { get; set; }
    }

    public class EventCondition<T> : Condition, IFSM where T : Event
    {
        public string eventName;
        private bool _listened = false;
        private bool active { get; set; }

        public override bool eval()
        {
            return _listened;
        }

        public override void enter()
        {
            eventListener.On<T>(OnEvent);
        }

        public override void exit()
        {
            eventListener.On<T>(OnEvent);
        }
        
        private void OnEvent(T evt)
        {
            _listened = true;
        }

        [fsIgnore]
        public FSM fsm { get; set; }
        
        [fsIgnore]
        public EventListener eventListener { get; set; }
    }
}

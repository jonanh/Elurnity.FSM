using System;
using System.Linq;

namespace Elurnity.FSM
{
    public abstract class Condition
    {
        public string name;

        public abstract bool Eval();

        public abstract void Enter();

        public abstract void Exit();
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

        public FSM fsm { get; set; }
    }

    public class Condition<T> : ParamCondition<T>, IFSM where T : IComparable
    {
        public enum ComparisonType { Equals, NotEquals, GreaterOrEqualThan, GreaterThan, LessThan, LessOrEqualThan }

        public T value;
        public ComparisonType comparisonType;

        public override bool Eval()
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

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }
    }
}

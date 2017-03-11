
namespace Elurnity.FSM
{
    public abstract class Param
    {
        public string name;
    }

    public class Param<T> : Param, IFSM
    {
        private T _value;
        public T value
        {
            get
            {
                return _value;
            }

            set
            {
                if (_value == null || !_value.Equals(value))
                {
                    this._value = value;
                    if (fsm != null)
                    {
                        fsm.update();
                    }
                }
            }
        }

        public FSM fsm { get; set; }
    }

    public sealed class Trigger : Param<bool>
    {
    }
}
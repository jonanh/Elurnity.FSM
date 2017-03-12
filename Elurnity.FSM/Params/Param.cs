
using System;

namespace Elurnity.FSM
{
    public abstract class Param
    {
        public string name;
    }

    public class Param<T> : Param
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

                    OnUpdate?.Invoke();
                }
            }
        }

        public event Action OnUpdate;
    }

    public sealed class Trigger : Param<bool>
    {
    }
}

namespace Elurnity.FSM
{
    public sealed class Transition
    {
        public string name;
        public State to;
        public Condition[] conditions;

        public bool eval()
        {
            foreach (var condition in conditions)
            {
                if (condition.Eval())
                {
                    return true;
                }
            }
            return false;
        }

        public void Enter()
        {
            foreach (var condition in conditions)
            {
                condition.Enter();
            }
        }

        public void Exit()
        {
            foreach (var condition in conditions)
            {
                condition.Exit();
            }
        }
    }
}


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

        public void enter()
        {
            foreach (var condition in conditions)
            {
                condition.Enter();
            }
        }

        public void exit()
        {
            foreach (var condition in conditions)
            {
                condition.Exit();
            }
        }
    }
}

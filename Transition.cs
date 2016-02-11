
namespace FSM
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
                if (condition.eval())
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
                condition.enter();
            }
        }

        public void exit()
        {
            foreach (var condition in conditions)
            {
                condition.exit();
            }
        }
    }
}

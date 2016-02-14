namespace Tests
{
    public class TestRunFSM2 : TestRunFSM
    {
        protected override void Start()
        {
            fsm = ExampleFSM.fsm();
            base.Start();
        }
    }
}
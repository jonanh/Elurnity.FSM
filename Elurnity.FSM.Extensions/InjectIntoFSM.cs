
using System.Collections.Generic;

namespace Elurnity.Serialization
{
    public static class InjectIntoFSMExtension
    {
        public static void InjectIntoFSM(this FSM.FSM fsm, Container container)
        {
            var netFSMContainer = new Container(container, new Dictionary<System.Type, object>
            {
                { typeof(FSM.FSM), fsm },
            });

            var injector = new Injector();

            injector.inject(fsm, netFSMContainer);
        }
    }
}

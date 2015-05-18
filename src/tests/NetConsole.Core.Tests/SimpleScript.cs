using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Actions;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Tests
{
    public class SimpleScript : IScript
    {
        public string Name { get; private set; }
        
        public IAction[] Actions { get; private set; }

        public SimpleScript()
        {
            Name = "simple";

            var echo = new EchoCommand();
            var prompt = new PromptCommand();
            Actions = new[]
            {
                new AndAction(
                    new CommandAction(echo, echo.FindDefaultAction(), new object[]{"Hello World"}),
                    new PipeAction(new[]
                    {
                        new CommandAction(prompt, prompt.FindAction("set")),
                        new CommandAction(echo, echo.FindAction("echoed")) 
                    }, "$")),
            };
        }


        public object[] Perform()
        {
            return Actions.Aggregate(new object[0], (accum, curr) => accum.Concat(curr.Perform()).ToArray());
        }
    }
}

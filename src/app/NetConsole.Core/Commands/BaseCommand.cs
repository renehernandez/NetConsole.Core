using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetConsole.Core.Attributes;
using NetConsole.Core.Extensions;
using NetConsole.Core.Interfaces;

namespace NetConsole.Core.Commands
{
    public abstract class BaseCommand : ICommand
    {

        # region Public Properties

        public IOptionAccessor Accessor { get; protected set; }

        public int Status { get; protected set; }

        public string Name { get; protected set; }

        public string Overview { get; protected set; }

        # endregion

        # region Constructors

        protected BaseCommand()
        {
            Accessor = new OptionAccessor(this);
            Accessor.ExtendOptions(new Option
            {
                Name = "help",
                DeclarableOnly = true,
                Permanent = true,
                OverrideExecution = true
            }, new Option
            {
                Name = "list",
                DeclarableOnly = true,
                Permanent = true,
                OverrideExecution = true
            }
            );

            Accessor.AddOptionValue("help");
            Accessor.AddOptionValue("list");
        }

        # endregion

        # region Methods for options

        [ActionForOption("help")]
        protected string Help(string actionName = null)
        {
            var outputHelp = new StringBuilder();
            if (actionName != null)
            {
                GenerateHelpForMethod(outputHelp, this.FindAction(actionName));
                return outputHelp.ToString();
            }

            foreach (var action in this.FindActions())
            {
                GenerateHelpForMethod(outputHelp, action);
            }
            return outputHelp.ToString();
        }


        [ActionForOption("list")]
        protected string List(string action = null)
        {
            var outputList = new StringBuilder();

            this.FindActions().ToList().ForEach(m => outputList.AppendLine(m.Name.ToLower()));

            return outputList.ToString();
        }

        # endregion

        # region Private Methods

        private void GenerateHelpForMethod(StringBuilder outputHelp, MethodInfo action)
        {
            var att = action.GetCustomAttributes(true).OfType<ActionHelpAttribute>().FirstOrDefault();
            bool isDefault = action.GetCustomAttributes(true).OfType<DefaultActionAttribute>().Any();
            var optionDeclarations = action.GetCustomAttributes(true).OfType<OptionHelpAttribute>().ToArray();
            var paramsDeclarations = action.GetCustomAttributes(true).OfType<ParamHelpAttribute>().ToArray();

            outputHelp.AppendLine(string.Format("Usage of {0}:{1}", this.Name, action.Name.ToLower()));
            outputHelp.AppendLine(string.Format("Default action: {0}", isDefault));
            outputHelp.AppendLine(att != null ? att.Content : "No action help documented");

            outputHelp.AppendLine("Options usage:");
            if (optionDeclarations.Length == 0)
            {
                outputHelp.AppendLine("No option are used.");
            }
            else
            {
                foreach (var decl in optionDeclarations)
                {
                    var option = this.Accessor.GetOptionDefinition(decl.Name);
                    var optionInfo = "--" + option.Name;
                    if (!option.DeclarableOnly)
                        optionInfo += "=VALUE";

                    outputHelp.AppendLine(string.Format("{0}{1}", optionInfo.PadRight(50 - optionInfo.Length), decl.Content));
                }
            }

            outputHelp.AppendLine("Parameters usage:");
            if (paramsDeclarations.Length == 0)
            {
                outputHelp.AppendLine("No parameters are used.");
            }
            else
            {
                foreach (var param in paramsDeclarations)
                {
                    outputHelp.AppendLine(string.Format("{0}{1}", param.Name.PadRight(25 - param.Name.Length), param.Content));
                }
            }

        }

        # endregion
    }
}

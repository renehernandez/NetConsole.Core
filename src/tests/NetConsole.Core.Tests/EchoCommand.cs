using System;
using System.Linq;
using NetConsole.Core.Attributes;
using NetConsole.Core.Commands;
using NetConsole.Core.Converters;

namespace NetConsole.Core.Tests
{
    public class EchoCommand : BaseCommand
    {

        # region Constructors

        public EchoCommand()
        {
            Name = "echo";
            Overview = "It returns the same input to the console";
            Status = 0;

            Accessor.ExtendOptions(new Option
            {
                DeclarableOnly = false,
                Name = "case",
                OverrideExecution = false,
                Permanent = false
            });
        }

        # endregion

        # region Public Methods

        [DefaultAction]
        [ActionHelp("Redirects the standard input to the standard output int the console")]
        [OptionHelp("case", "If value equals 1 then lower all text; if value equals 2 then upper all text")]
        public string Echoed(params object[] input)
        {
            string output;
            try
            {
                if (Accessor.HasOptionValue("case"))
                {
                    var option = Accessor.GetOption("case");
                    var tup = Converter.TryInt(option);
                    if (tup.Item1)
                    {
                        var num = (int) option;
                        if (num == 1)
                        {
                            output =
                                input.Select(x => x.ToString().ToLower()).Aggregate((accum, curr) => accum + " " + curr);
                        }
                        else
                        {
                            output =
                                input.Select(x => x.ToString().ToUpper()).Aggregate((accum, curr) => accum + " " + curr);
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    output = input.Select(x => x.ToString()).Aggregate((accum, curr) => accum + " " + curr);
                }
                Status = 0;
            }
            catch (Exception e)
            {
                Status = 1;
                output = e.Message;
            }
            return output;
        }

        # endregion
    }
}
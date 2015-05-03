using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace NetConsole.Core.Errors
{
    public class LexerErrorListener: IAntlrErrorListener<int>
    {

        public static LexerErrorListener Instance = new LexerErrorListener();

        private LexerErrorListener()
        {
            
        }

        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg,
            RecognitionException e)
        {
            throw new ParseCanceledException(string.Format("Line {0}: {1} {2}", line, charPositionInLine, msg));
        }
    }
}

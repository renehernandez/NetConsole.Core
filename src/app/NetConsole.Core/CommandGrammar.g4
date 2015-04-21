grammar CommandGrammar;

/*
 * Parser Rules
 */

compile
	: or_instruction EOF
	;

or_instruction
	:	and_instruction (OR and_instruction)*;

and_instruction
	:	atomic_instruction (AND atomic_instruction)*;

atomic_instruction
	:	command
	|	pipe_instruction
	|	redirect_instruction;

pipe_instruction
	:	command (PIPE ID)+;

redirect_instruction
	: command REDIRECT (STRING | ID);

command 
	:	ID (ID | STRING | INT | DOUBLE)*;

/*
 * Lexer Rules
 */

AND         : '&&';
OR          : '||';
PIPE        : '|';
REDIRECT    : '>';
DOT         : '.';
UNDERSCORE  : '_';
MINUS       : '-';
QUOTES      : '"';

// Lexer rules

fragment
	DIGIT : [0-9];

fragment
	LETTER : [a-zA_Z];

DOUBLE : DIGIT+ DOT DIGIT+;
INT : DIGIT+ ;

ID	: LETTER (LETTER | DIGIT | UNDERSCORE | MINUS )*;

STRING : QUOTES (.)*? QUOTES;

WS
	:	(' ' | '\r' | '\t' | '\n')+ -> channel(HIDDEN)
	;

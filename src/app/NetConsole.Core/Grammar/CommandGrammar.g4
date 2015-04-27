grammar CommandGrammar;

/*
 * Parser Rules
 */

compile
	:	instruction EOF
	;

instruction
	:	instruction OR instruction			# OrInstruction
	|	instruction AND instruction			# AndInstruction
	|	atomic_instruction					# AtomicInstruction
	;

atomic_instruction
	:	command								# SingleCommand
	|	command ( PIPE command_header)+		# PipeCommand
	|	command REDIRECT (STRING | ID)		# RedirectCommand
	;

command 
	:	command_header list_params;

command_header
	:	ID (COLON ID)?;

list_params
	:	command_param* 
	;

command_param
	:	st = (STRING | ID)	# StringParam
	|	INT					# IntParam 
	|	DOUBLE				# DoubleParam
	|	BOOL				# BoolParam
	;

/*
 * Lexer Rules
 */

AND         : '&&';
OR          : '||';
PIPE        : '|';
REDIRECT    : '>';
DOT         : '.';
COLON		: ':';
UNDERSCORE  : '_';
MINUS       : '-';

fragment
	DIGIT : [0-9];

fragment
	LETTER : [a-zA-Z];

fragment
	QUOTES : '"';

DOUBLE : DIGIT+ DOT DIGIT+;
INT : DIGIT+ ;

BOOL : 'true' | 'false';

ID	: LETTER (LETTER | DIGIT | UNDERSCORE | MINUS )*;

STRING : QUOTES .*? QUOTES;

WS
	:	(' ' | '\r' | '\t' | '\n')+ -> channel(HIDDEN)
	;

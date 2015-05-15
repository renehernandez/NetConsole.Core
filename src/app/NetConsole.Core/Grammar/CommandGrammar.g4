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
	|	command REDIRECT text				# RedirectCommand
	|	command_header INPUT text			# InputCommand
	;

command 
	:	command_header list_params;

command_header
	:	ID (COLON ID)?;

list_params
	:	command_param* 
	;

command_param
	:	text								# StringParam
	|	DOUBLE_HYPHEN ID (EQUAL text)?		# OptionParam
	;

text 
	:	STRING		# StringText
	|	ID			# IDText
	;

/*
 * Lexer Rules
 */

AND				: '&&';
OR				: '||';
PIPE			: '|';
REDIRECT		: '>';
INPUT			: '<';
DOT				: '.';
COLON			: ':';
UNDERSCORE		: '_';
HYPHEN			: '-';
DOUBLE_HYPHEN   : '--';
EQUAL			: '=';

fragment
	DIGIT : [0-9];

fragment
	LETTER : [a-zA-Z];

fragment
	QUOTES : '"';

ID	: LETTER (LETTER | DIGIT | UNDERSCORE | HYPHEN )*;

STRING : QUOTES .*? QUOTES;

WS
	:	(' ' | '\r' | '\t' | '\n')+ -> channel(HIDDEN)
	;

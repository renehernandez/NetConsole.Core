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
	:	command										# SingleCommand
	|	command ( PIPE command_header)+				# PipeCommand
	|	command REDIRECT st = (STRING | ID)			# RedirectCommand
	|	command_header INPUT st = (STRING | ID)		# InputCommand
	;

command 
	:	command_header list_params;

command_header
	:	ID (COLON ID)?;

list_params
	:	command_param* 
	;

command_param
	:	st = (STRING | ID)								# StringParam
	|	INT												# IntParam 
	|	DOUBLE											# DoubleParam
	|	BOOL											# BoolParam
	|	DOUBLE_HYPHEN ID (EQUAL val = option_value)?	# OptionParam
	;

option_value
	:	st = (STRING | ID)		# StringOption
	|	DOUBLE					# DoubleOption
	|	INT						# IntOption
	|	BOOL					# BoolOption
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

DOUBLE : HYPHEN? DIGIT+ DOT DIGIT+;

INT : HYPHEN? DIGIT+ ;

BOOL : 'true' | 'false';

ID	: LETTER (LETTER | DIGIT | UNDERSCORE | HYPHEN )*;

STRING : QUOTES .*? QUOTES;

WS
	:	(' ' | '\r' | '\t' | '\n')+ -> channel(HIDDEN)
	;

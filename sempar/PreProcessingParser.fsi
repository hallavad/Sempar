// Signature file for parser generated by fsyacc
module PreProcessingParser
type token = 
  | PREAITEM of (string * string)
  | EOF
  | STRING
  | PIPE
  | COLON
  | COMMENT
  | CONSTRAINT of (string)
  | PREACODE of (string)
  | PERCENT
  | DOUBLEPERCENT
  | CODE of (string)
  | ID of (string)
type tokenId = 
    | TOKEN_PREAITEM
    | TOKEN_EOF
    | TOKEN_STRING
    | TOKEN_PIPE
    | TOKEN_COLON
    | TOKEN_COMMENT
    | TOKEN_CONSTRAINT
    | TOKEN_PREACODE
    | TOKEN_PERCENT
    | TOKEN_DOUBLEPERCENT
    | TOKEN_CODE
    | TOKEN_ID
    | TOKEN_end_of_input
    | TOKEN_error
type nonTerminalId = 
    | NONTERM__startstart
    | NONTERM_start
    | NONTERM_preamble
    | NONTERM_preaItems
    | NONTERM_rules
    | NONTERM_rule
    | NONTERM_cases
    | NONTERM_tokens
    | NONTERM_constraints
/// This function maps tokens to integer indexes
val tagOfToken: token -> int

/// This function maps integer indexes to symbolic token ids
val tokenTagToTokenId: int -> tokenId

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
val prodIdxToNonTerminal: int -> nonTerminalId

/// This function gets the name of a token as a string
val token_to_string: token -> string
val start : (FSharp.Text.Lexing.LexBuffer<'cty> -> token) -> FSharp.Text.Lexing.LexBuffer<'cty> -> (Diagnostics.FSY) 

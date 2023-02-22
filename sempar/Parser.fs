module Parser

open FSharp.Text.Lexing

open HelperFunctions
open ParserType
open PPType

let parse (input: string): FSY = 
    let splits = input.Split ("%%", 2)
    let lexbuf = LexBuffer<char>.FromString splits[1]
    let rules = PreProcessingParser.rules PreProcessingLexer.read lexbuf
    { preamble = splits[0]; rules = rules } : FSY
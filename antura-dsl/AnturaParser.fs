// Implementation file for parser generated by fsyacc
module Parser
#nowarn "64";; // turn off warnings that type variables used in production annotations are instantiated to concrete type
open FSharp.Text.Lexing
open FSharp.Text.Parsing.ParseHelpers
# 1 "inputs/AnturaParser.ppfsy"

open DataModel 

let debug_print x = 
  printfn "" 
  x
open ParserType

# 15 "AnturaParser.fs"
// This type is the type of tokens accepted by the parser
type token = 
  | EOF
  | WHEN
  | GET
  | TASK
  | PROJECT_PROPERTY
  | PRECISION
  | DESTINATION
  | TRANSFORMATION
  | SOURCES
  | RULE
  | PROJECT_TYPES
  | EQUAL
  | DOT
  | COMMA
  | COLON
  | ID of (string)
  | STRING of (string)
  | INT of (int)
// This type is used to give symbolic names to token indexes, useful for error messages
type tokenId = 
    | TOKEN_EOF
    | TOKEN_WHEN
    | TOKEN_GET
    | TOKEN_TASK
    | TOKEN_PROJECT_PROPERTY
    | TOKEN_PRECISION
    | TOKEN_DESTINATION
    | TOKEN_TRANSFORMATION
    | TOKEN_SOURCES
    | TOKEN_RULE
    | TOKEN_PROJECT_TYPES
    | TOKEN_EQUAL
    | TOKEN_DOT
    | TOKEN_COMMA
    | TOKEN_COLON
    | TOKEN_ID
    | TOKEN_STRING
    | TOKEN_INT
    | TOKEN_end_of_input
    | TOKEN_error
// This type is used to give symbolic names to token indexes, useful for error messages
type nonTerminalId = 
    | NONTERM__startstart
    | NONTERM_start
    | NONTERM_precision
    | NONTERM_rule_list
    | NONTERM_rule
    | NONTERM_rule_name
    | NONTERM_types
    | NONTERM_type_list
    | NONTERM_sources
    | NONTERM_source_list
    | NONTERM_source
    | NONTERM_transformations
    | NONTERM_destination
    | NONTERM_property

// This function maps tokens to integer indexes
let tagOfToken (t:token) = 
  match t with
  | EOF  -> 0 
  | WHEN  -> 1 
  | GET  -> 2 
  | TASK  -> 3 
  | PROJECT_PROPERTY  -> 4 
  | PRECISION  -> 5 
  | DESTINATION  -> 6 
  | TRANSFORMATION  -> 7 
  | SOURCES  -> 8 
  | RULE  -> 9 
  | PROJECT_TYPES  -> 10 
  | EQUAL  -> 11 
  | DOT  -> 12 
  | COMMA  -> 13 
  | COLON  -> 14 
  | ID _ -> 15 
  | STRING _ -> 16 
  | INT _ -> 17 

// This function maps integer indexes to symbolic token ids
let tokenTagToTokenId (tokenIdx:int) = 
  match tokenIdx with
  | 0 -> TOKEN_EOF 
  | 1 -> TOKEN_WHEN 
  | 2 -> TOKEN_GET 
  | 3 -> TOKEN_TASK 
  | 4 -> TOKEN_PROJECT_PROPERTY 
  | 5 -> TOKEN_PRECISION 
  | 6 -> TOKEN_DESTINATION 
  | 7 -> TOKEN_TRANSFORMATION 
  | 8 -> TOKEN_SOURCES 
  | 9 -> TOKEN_RULE 
  | 10 -> TOKEN_PROJECT_TYPES 
  | 11 -> TOKEN_EQUAL 
  | 12 -> TOKEN_DOT 
  | 13 -> TOKEN_COMMA 
  | 14 -> TOKEN_COLON 
  | 15 -> TOKEN_ID 
  | 16 -> TOKEN_STRING 
  | 17 -> TOKEN_INT 
  | 20 -> TOKEN_end_of_input
  | 18 -> TOKEN_error
  | _ -> failwith "tokenTagToTokenId: bad token"

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
let prodIdxToNonTerminal (prodIdx:int) = 
  match prodIdx with
    | 0 -> NONTERM__startstart 
    | 1 -> NONTERM_start 
    | 2 -> NONTERM_start 
    | 3 -> NONTERM_precision 
    | 4 -> NONTERM_rule_list 
    | 5 -> NONTERM_rule_list 
    | 6 -> NONTERM_rule 
    | 7 -> NONTERM_rule 
    | 8 -> NONTERM_rule_name 
    | 9 -> NONTERM_rule_name 
    | 10 -> NONTERM_types 
    | 11 -> NONTERM_type_list 
    | 12 -> NONTERM_type_list 
    | 13 -> NONTERM_sources 
    | 14 -> NONTERM_source_list 
    | 15 -> NONTERM_source_list 
    | 16 -> NONTERM_source 
    | 17 -> NONTERM_transformations 
    | 18 -> NONTERM_destination 
    | 19 -> NONTERM_property 
    | _ -> failwith "prodIdxToNonTerminal: bad production index"

let _fsyacc_endOfInputTag = 20 
let _fsyacc_tagOfErrorTerminal = 18

// This function gets the name of a token as a string
let token_to_string (t:token) = 
  match t with 
  | EOF  -> "EOF" 
  | WHEN  -> "WHEN" 
  | GET  -> "GET" 
  | TASK  -> "TASK" 
  | PROJECT_PROPERTY  -> "PROJECT_PROPERTY" 
  | PRECISION  -> "PRECISION" 
  | DESTINATION  -> "DESTINATION" 
  | TRANSFORMATION  -> "TRANSFORMATION" 
  | SOURCES  -> "SOURCES" 
  | RULE  -> "RULE" 
  | PROJECT_TYPES  -> "PROJECT_TYPES" 
  | EQUAL  -> "EQUAL" 
  | DOT  -> "DOT" 
  | COMMA  -> "COMMA" 
  | COLON  -> "COLON" 
  | ID _ -> "ID" 
  | STRING _ -> "STRING" 
  | INT _ -> "INT" 

// This function gets the data carried by a token as an object
let _fsyacc_dataOfToken (t:token) = 
  match t with 
  | EOF  -> (null : System.Object) 
  | WHEN  -> (null : System.Object) 
  | GET  -> (null : System.Object) 
  | TASK  -> (null : System.Object) 
  | PROJECT_PROPERTY  -> (null : System.Object) 
  | PRECISION  -> (null : System.Object) 
  | DESTINATION  -> (null : System.Object) 
  | TRANSFORMATION  -> (null : System.Object) 
  | SOURCES  -> (null : System.Object) 
  | RULE  -> (null : System.Object) 
  | PROJECT_TYPES  -> (null : System.Object) 
  | EQUAL  -> (null : System.Object) 
  | DOT  -> (null : System.Object) 
  | COMMA  -> (null : System.Object) 
  | COLON  -> (null : System.Object) 
  | ID _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
  | STRING _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
  | INT _fsyacc_x -> Microsoft.FSharp.Core.Operators.box _fsyacc_x 
let _fsyacc_gotos = [| 0us;65535us;1us;65535us;0us;1us;1us;65535us;0us;2us;3us;65535us;0us;4us;2us;3us;7us;8us;3us;65535us;0us;7us;2us;7us;7us;7us;2us;65535us;9us;15us;21us;22us;2us;65535us;10us;11us;16us;17us;2us;65535us;23us;24us;25us;26us;2us;65535us;11us;12us;17us;18us;2us;65535us;27us;28us;29us;30us;2us;65535us;27us;29us;29us;29us;2us;65535us;12us;13us;18us;19us;2us;65535us;13us;14us;19us;20us;1us;65535us;40us;41us;|]
let _fsyacc_sparseGotoTableRowOffsets = [|0us;1us;3us;5us;9us;13us;16us;19us;22us;25us;28us;31us;34us;37us;|]
let _fsyacc_stateToProdIdxsTableElements = [| 1us;0us;1us;0us;1us;1us;1us;1us;1us;2us;1us;3us;1us;3us;1us;5us;1us;5us;2us;6us;7us;1us;6us;1us;6us;1us;6us;1us;6us;1us;6us;1us;7us;1us;7us;1us;7us;1us;7us;1us;7us;1us;7us;1us;9us;1us;9us;1us;10us;1us;10us;1us;12us;1us;12us;1us;13us;1us;13us;1us;15us;1us;15us;1us;16us;1us;16us;1us;16us;1us;16us;1us;16us;1us;16us;1us;16us;1us;17us;1us;17us;1us;18us;1us;18us;1us;19us;1us;19us;|]
let _fsyacc_stateToProdIdxsTableRowOffsets = [|0us;2us;4us;6us;8us;10us;12us;14us;16us;18us;21us;23us;25us;27us;29us;31us;33us;35us;37us;39us;41us;43us;45us;47us;49us;51us;53us;55us;57us;59us;61us;63us;65us;67us;69us;71us;73us;75us;77us;79us;81us;83us;85us;87us;|]
let _fsyacc_action_rows = 44
let _fsyacc_actionTableElements = [|2us;16388us;5us;5us;9us;9us;0us;49152us;1us;16388us;9us;9us;0us;16385us;0us;16386us;1us;32768us;17us;6us;0us;16387us;1us;16388us;9us;9us;0us;16389us;2us;16392us;14us;10us;15us;21us;1us;32768us;10us;23us;1us;32768us;8us;27us;1us;32768us;7us;38us;1us;32768us;6us;40us;0us;16390us;1us;32768us;14us;16us;1us;32768us;10us;23us;1us;32768us;8us;27us;1us;32768us;7us;38us;1us;32768us;6us;40us;0us;16391us;1us;16392us;15us;21us;0us;16393us;1us;16395us;16us;25us;0us;16394us;1us;16395us;16us;25us;0us;16396us;1us;16398us;3us;31us;0us;16397us;1us;16398us;3us;31us;0us;16399us;1us;32768us;1us;32us;1us;32768us;15us;33us;1us;32768us;11us;34us;1us;32768us;15us;35us;1us;32768us;2us;36us;1us;32768us;15us;37us;0us;16400us;1us;32768us;15us;39us;0us;16401us;1us;32768us;4us;42us;0us;16402us;1us;32768us;15us;43us;0us;16403us;|]
let _fsyacc_actionTableRowOffsets = [|0us;3us;4us;6us;7us;8us;10us;11us;13us;14us;17us;19us;21us;23us;25us;26us;28us;30us;32us;34us;36us;37us;39us;40us;42us;43us;45us;46us;48us;49us;51us;52us;54us;56us;58us;60us;62us;64us;65us;67us;68us;70us;71us;73us;|]
let _fsyacc_reductionSymbolCounts = [|1us;2us;1us;2us;0us;2us;6us;7us;0us;2us;2us;0us;2us;2us;0us;2us;7us;2us;2us;2us;|]
let _fsyacc_productionToNonTerminalTable = [|0us;1us;1us;2us;3us;3us;4us;4us;5us;5us;6us;7us;7us;8us;9us;9us;10us;11us;12us;13us;|]
let _fsyacc_immediateActions = [|65535us;49152us;65535us;16385us;16386us;65535us;16387us;65535us;16389us;65535us;65535us;65535us;65535us;65535us;16390us;65535us;65535us;65535us;65535us;65535us;16391us;65535us;16393us;65535us;16394us;65535us;16396us;65535us;16397us;65535us;16399us;65535us;65535us;65535us;65535us;65535us;65535us;16400us;65535us;16401us;65535us;16402us;65535us;16403us;|]
let _fsyacc_reductions = lazy [|
# 204 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> ParserType<DataModel.Rules> in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
                      raise (FSharp.Text.Parsing.Accept(Microsoft.FSharp.Core.Operators.box _1))
                   )
                 : 'gentype__startstart));
# 213 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> 'gentype_precision in
            let _2 = parseState.GetInput(2) :?> 'gentype_rule_list in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 34 "inputs/AnturaParser.ppfsy"
                                            parserType {
                       let! semparVar1 = _1
                       let! semparVar2 = _2
                       
                       return ( { precision = Some semparVar1; rules = semparVar2 } )
                     }
                   )
# 34 "inputs/AnturaParser.ppfsy"
                 : ParserType<DataModel.Rules>));
# 230 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> 'gentype_rule_list in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 40 "inputs/AnturaParser.ppfsy"
                                  parserType {
                       let! semparVar1 = _1
                       
                       return ( { precision = None; rules = semparVar1 } )
                     }
                   )
# 40 "inputs/AnturaParser.ppfsy"
                 : ParserType<DataModel.Rules>));
# 245 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _2 = parseState.GetInput(2) :?> int in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 48 "inputs/AnturaParser.ppfsy"
                                      parserType {
                       let semparVar2 = _2
                       errorUnless "Precision must be positive" (semparVar2 >= 1); printfn "here: %A" x; x
                       return ( semparVar2 )
                     }
                   )
# 48 "inputs/AnturaParser.ppfsy"
                 : 'gentype_precision));
# 260 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 55 "inputs/AnturaParser.ppfsy"
                         parserType {
                       
                       return ( [] )
                     }
                   )
# 55 "inputs/AnturaParser.ppfsy"
                 : 'gentype_rule_list));
# 273 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> 'gentype_rule in
            let _2 = parseState.GetInput(2) :?> 'gentype_rule_list in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 60 "inputs/AnturaParser.ppfsy"
                                       parserType {
                       let! semparVar1 = _1
                       let! semparVar2 = _2
                       
                       return ( semparVar1 :: semparVar2 )
                     }
                   )
# 60 "inputs/AnturaParser.ppfsy"
                 : 'gentype_rule_list));
# 290 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _3 = parseState.GetInput(3) :?> 'gentype_types in
            let _4 = parseState.GetInput(4) :?> 'gentype_sources in
            let _5 = parseState.GetInput(5) :?> 'gentype_transformations in
            let _6 = parseState.GetInput(6) :?> 'gentype_destination in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 68 "inputs/AnturaParser.ppfsy"
                                                                             parserType {
                       let! semparVar3 = _3
                       let! semparVar4 = _4
                       let! semparVar5 = _5
                       let! semparVar6 = _6
                       
                       return ( {name = None; types = semparVar3; sources = semparVar4; transformation = semparVar5; destination = semparVar6} )
                     }
                   )
# 68 "inputs/AnturaParser.ppfsy"
                 : 'gentype_rule));
# 311 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _2 = parseState.GetInput(2) :?> 'gentype_rule_name in
            let _4 = parseState.GetInput(4) :?> 'gentype_types in
            let _5 = parseState.GetInput(5) :?> 'gentype_sources in
            let _6 = parseState.GetInput(6) :?> 'gentype_transformations in
            let _7 = parseState.GetInput(7) :?> 'gentype_destination in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 76 "inputs/AnturaParser.ppfsy"
                                                                                       parserType {
                       let! semparVar2 = _2
                       let! semparVar4 = _4
                       let! semparVar5 = _5
                       let! semparVar6 = _6
                       let! semparVar7 = _7
                       
                       return ( {name = Some semparVar2; types = semparVar4; sources = semparVar5; transformation = semparVar6; destination = semparVar7} )
                     }
                   )
# 76 "inputs/AnturaParser.ppfsy"
                 : 'gentype_rule));
# 334 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 87 "inputs/AnturaParser.ppfsy"
                         parserType {
                       
                       return ( "" )
                     }
                   )
# 87 "inputs/AnturaParser.ppfsy"
                 : 'gentype_rule_name));
# 347 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> string in
            let _2 = parseState.GetInput(2) :?> 'gentype_rule_name in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 92 "inputs/AnturaParser.ppfsy"
                                     parserType {
                       let semparVar1 = _1
                       let! semparVar2 = _2
                       
                       return ( semparVar1 + " " + semparVar2 )
                     }
                   )
# 92 "inputs/AnturaParser.ppfsy"
                 : 'gentype_rule_name));
# 364 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _2 = parseState.GetInput(2) :?> 'gentype_type_list in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 100 "inputs/AnturaParser.ppfsy"
                                                parserType {
                       let! semparVar2 = _2
                       
                       return ( semparVar2 )
                     }
                   )
# 100 "inputs/AnturaParser.ppfsy"
                 : 'gentype_types));
# 379 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 107 "inputs/AnturaParser.ppfsy"
                         parserType {
                       
                       return ( [] )
                     }
                   )
# 107 "inputs/AnturaParser.ppfsy"
                 : 'gentype_type_list));
# 392 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> string in
            let _2 = parseState.GetInput(2) :?> 'gentype_type_list in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 112 "inputs/AnturaParser.ppfsy"
                                         parserType {
                       let semparVar1 = _1
                       let! semparVar2 = _2
                       
                       return ( semparVar1 :: semparVar2 )
                     }
                   )
# 112 "inputs/AnturaParser.ppfsy"
                 : 'gentype_type_list));
# 409 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _2 = parseState.GetInput(2) :?> 'gentype_source_list in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 120 "inputs/AnturaParser.ppfsy"
                                            parserType {
                       let! semparVar2 = _2
                       
                       return ( semparVar2 )
                     }
                   )
# 120 "inputs/AnturaParser.ppfsy"
                 : 'gentype_sources));
# 424 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 127 "inputs/AnturaParser.ppfsy"
                         parserType {
                       
                       return ( [] )
                     }
                   )
# 127 "inputs/AnturaParser.ppfsy"
                 : 'gentype_source_list));
# 437 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _1 = parseState.GetInput(1) :?> 'gentype_source in
            let _2 = parseState.GetInput(2) :?> 'gentype_source_list in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 132 "inputs/AnturaParser.ppfsy"
                                           parserType {
                       let! semparVar1 = _1
                       let! semparVar2 = _2
                       
                       return ( semparVar1 :: semparVar2 )
                     }
                   )
# 132 "inputs/AnturaParser.ppfsy"
                 : 'gentype_source_list));
# 454 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _3 = parseState.GetInput(3) :?> string in
            let _5 = parseState.GetInput(5) :?> string in
            let _7 = parseState.GetInput(7) :?> string in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 141 "inputs/AnturaParser.ppfsy"
                                                     parserType {
                       let semparVar3 = _3
                       let semparVar5 = _5
                       let semparVar7 = _7
                       errorUnless "Tasks must be certain values" (semparVar7 = "StartDate" || semparVar7 = "EndDate" || semparVar7 = "Property")
                       return ( (semparVar3, semparVar5, semparVar7) )
                     }
                   )
# 141 "inputs/AnturaParser.ppfsy"
                 : 'gentype_source));
# 473 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _2 = parseState.GetInput(2) :?> string in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 150 "inputs/AnturaParser.ppfsy"
                                          parserType {
                       let semparVar2 = _2
                       
                       return ( Some semparVar2 )
                     }
                   )
# 150 "inputs/AnturaParser.ppfsy"
                 : 'gentype_transformations));
# 488 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _2 = parseState.GetInput(2) :?> 'gentype_property in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 158 "inputs/AnturaParser.ppfsy"
                                             parserType {
                       let! semparVar2 = _2
                       // warnUnless "Destination should probably start with \"Start\"" (semparVar2.StartsWith("Start"))
                       return ( semparVar2 )
                     }
                   )
# 158 "inputs/AnturaParser.ppfsy"
                 : 'gentype_destination));
# 503 "AnturaParser.fs"
        (fun (parseState : FSharp.Text.Parsing.IParseState) ->
            let _2 = parseState.GetInput(2) :?> string in
            Microsoft.FSharp.Core.Operators.box
                (
                   (
# 165 "inputs/AnturaParser.ppfsy"
                                            parserType {
                       let semparVar2 = _2
                       
                       return ( Property semparVar2 )
                     }
                   )
# 165 "inputs/AnturaParser.ppfsy"
                 : 'gentype_property));
|]
# 519 "AnturaParser.fs"
let tables : FSharp.Text.Parsing.Tables<_> = 
  { reductions = _fsyacc_reductions.Value;
    endOfInputTag = _fsyacc_endOfInputTag;
    tagOfToken = tagOfToken;
    dataOfToken = _fsyacc_dataOfToken; 
    actionTableElements = _fsyacc_actionTableElements;
    actionTableRowOffsets = _fsyacc_actionTableRowOffsets;
    stateToProdIdxsTableElements = _fsyacc_stateToProdIdxsTableElements;
    stateToProdIdxsTableRowOffsets = _fsyacc_stateToProdIdxsTableRowOffsets;
    reductionSymbolCounts = _fsyacc_reductionSymbolCounts;
    immediateActions = _fsyacc_immediateActions;
    gotos = _fsyacc_gotos;
    sparseGotoTableRowOffsets = _fsyacc_sparseGotoTableRowOffsets;
    tagOfErrorTerminal = _fsyacc_tagOfErrorTerminal;
    parseError = (fun (ctxt:FSharp.Text.Parsing.ParseErrorContext<_>) -> 
                              match parse_error_rich with 
                              | Some f -> f ctxt
                              | None -> parse_error ctxt.Message);
    numTerminals = 21;
    productionToNonTerminalTable = _fsyacc_productionToNonTerminalTable  }
let engine lexer lexbuf startState = tables.Interpret(lexer, lexbuf, startState)
let start lexer lexbuf : ParserType<DataModel.Rules> =
    engine lexer lexbuf 0 :?> _

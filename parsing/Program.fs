module Program

open FParsec
open Microsoft.FSharp.Core.Option

let undefined<'T> : 'T = failwith "Not implemented yet"

let test p str = 
    match run p str with
    | Success(result,_,_) -> printfn "Success: %A" result
    | Failure(errorMsg,_,_) -> printfn "Failure: %s" errorMsg

type ProjectTypes = 
    | All 
    | Few of projectTypes: string list

type RuleElem =
    | ProjectTypes of ProjectTypes
    // | Source of Sources
    // | Transformation of Transformation
    // | Destionation of Destination

type Rule = {
    projectTypes: ProjectTypes;
    // source: Source;
    // transformation: Transformation;
    // destination: Destination:
}
type maybeRule = {
    projectTypes: ProjectTypes option;
    // source: Source option; 
    // transformation: Transformation option;
    // destination: Destination option:
} 

let emptyRule: maybeRule = {
        projectTypes = None;
        // source: None;
        // transformation: None;
        // destination: None;
}

let maybeRuleToRule ({projectTypes = pt }: maybeRule): Rule option = 
    if (isNone pt) then
        None
    else
        Some({projectTypes = pt.Value})

let combineToRule (elems: RuleElem list): Rule option = 
    List.fold (
        fun (maybeRule: maybeRule) elem -> 
            match elem with 
            | ProjectTypes pt -> {maybeRule with projectTypes = Some(pt)}) (emptyRule) elems
    |> maybeRuleToRule 

let pstrLiteral =
    let normalChar = satisfy (fun c -> c <> '\\' && c <> '"')
    let unescape c = match c with
                     | 'n' -> '\n'
                     | 'r' -> '\r'
                     | 't' -> '\t'
                     | c   -> c
    let escapedChar = pstring "\\" >>. (anyOf "\\nrt\"" |>> unescape)
    between (pstring "\"") (pstring "\"")
            (manyChars (normalChar <|> escapedChar))


//let test = parray 4 (choice [types; sources; transformation; destination])

let pprojectTypes: Parser<ProjectTypes,unit> = 
    pstring "project types:" 
    >>. choice [
        pstring "All" >>% All; 
        many1 (pstrLiteral .>> newline) |>> Few 
    ]   

let pRule: Parser<Rule,unit> = 
    pstring "rule:" .>> newline 
    >>. parray 1 (choice [pprojectTypes |>> ProjectTypes;])
    |>> Array.toList
    |>> combineToRule |>> fun x -> x.Value  // This fails in a bad way, handle it properly
    
 
let pRules: Parser<Rule list,unit> = many pRule






































let _pstrLiteral = pstrLiteral

let _psurroundedBy p s = between s s p

let _pcharsEndedBy s = charsTillString s true System.Int32.MaxValue

let _pprecision = skipString "precision" >>. spaces >>. puint64

let _pruleheader = skipString "rule" >>. spaces >>. _pcharsEndedBy ":"

let _pprojType = _pstrLiteral .>> spaces

let _pprojTypeBlock = pstring "project types:" >>. many _pprojType

let _psource = skipString "task" >>. spaces >>. _pcharsEndedBy "=" .>>. _pcharsEndedBy ":" .>>. _pcharsEndedBy "\n"

let _psources = pstring "sources:" >>. many _psource

let _ptransformation = undefined

let _pdestination = undefined

let _pruleElem = (opt _pprojTypeBlock) <|> _psources <|> (opt _ptransformation) <|> _pdestination

let _prule = _pruleheader .>>. _pruleElem

let _pfile = opt _pprecision .>>. many _prule

printfn "Fuck this"

// undefined
module Parser

open System.Text.RegularExpressions

open FSharp.Text.Lexing

open PPType

let usedVariables (Code code: Code): string list =
    let mutable vars = []
    let mutable includeNext = false
    let mutable startedIncluding = false
    for (c : char) in code do 
        if includeNext then
            if System.Char.IsNumber(c) then
                vars <- string c :: vars
                startedIncluding <- true
            includeNext <- false
        else if startedIncluding then
            if System.Char.IsNumber(c) then
                // We know that the list isn't empty here
                let (head :: tail) = vars
                vars <- (head + string c) :: tail
            else 
                startedIncluding <- false
        else 
            match c with 
            | '$' -> 
                includeNext <- true
            | _ -> 
                includeNext <- false
    List.rev vars

let genVariableDecls (code: Code) (predefTokens: string list): string =
    code 
        |> usedVariables
        |> List.map (fun v -> 
            if List.contains v predefTokens then
                $"  let semparVar{v} = ${v}"
            else 
                $"  let! semparVar{v} = ${v}"
            ) 
        |> concatNewlines
        
let genCode (code: Code) (cs: Constraint list) (tokens: string list): Code =
    Code $"""parserType {{
{genVariableDecls code tokens}
  {cs |> mapToString |> concatNewlines}
  return ({code})
}}
"""

let insertConstraints (fsy: FSY): FSY =
    let { rules = rules; preamble = preamble } = fsy
    let usedTokens = preamble.usedTokens
    let newRules = rules |> List.map (fun rule -> 
        let {cases = cases} = rule
        let newCases = cases |> List.map (fun case -> 
            let {code = code; constraints = constraints } = case
            let usedTokenIndices = case.predefinedTokenIndices usedTokens
            let newCode = genCode code constraints usedTokenIndices
            { case with code = newCode }
        )
        { rule with cases = newCases }
    )
    { fsy with rules = newRules }

let insertImport (fsy: FSY): FSY =
    let (PreaCode code) = fsy.preamble.preaCode
    let newPreaCode = PreaCode(code + "\nopen ParserType")
    let newPreamble = { fsy.preamble with preaCode = newPreaCode }
    { fsy with preamble = newPreamble }

// Must happen after `insertConstraints`
let replaceVars (fsy: FSY): FSY =
    let regex = "\$(?=[0-9]+)"
    let newRules = fsy.rules |> List.map (fun rule -> 
        let newCases = rule.cases |> List.map (fun case ->
            let (Code oldCode) = case.code
            let newCode = Regex.Replace(oldCode, regex, "semparVar")
            let newConstraints = case.constraints |> List.map (fun constr -> 
                let (Constr oldConstraint) = constr
                let newConstraint = Regex.Replace(oldConstraint, regex, "semparVar")
                Constr newConstraint
            )
            { case with code = Code newCode; constraints = newConstraints }
        )
        { rule with cases = newCases }
    )
    { fsy with rules = newRules }
    
let preprocess (input: FSY): FSY = 
    input |> insertConstraints |> insertImport |> replaceVars

let parse (input: string): FSY = 
    let lexbuf = LexBuffer<char>.FromString input
    let FSY = PreProcessingParser.start PreProcessingLexer.read lexbuf
    FSY
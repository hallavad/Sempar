module Parser

open System.Text.RegularExpressions

open FSharp.Text.Lexing

open PPType

let insertConstraints (fsy: FSY): FSY =
    let { rules = rules; preamble = preamble } = fsy
    let usedTokens = preamble.usedTokens
    let newRules = rules |> List.map (
        fun rule -> 
            let {cases = cases} = rule
            let newCases = cases |> List.map (
                fun case -> 
                    let {code = code; constraints = constraints; tokens = tokens } = case
                    let usedTokenIndices = case.predefinedTokenIndices usedTokens
                    let newCode = code.GenCode constraints usedTokenIndices
                    {case with code = newCode})
            {rule with cases = newCases})
    {fsy with rules = newRules}

let insertImport (fsy: FSY): FSY =
    let (PreaCode code) = fsy.preamble.preaCode
    let newPreaCode = PreaCode(code + "\nopen ParserType")
    let newPreamble = { fsy.preamble with preaCode = newPreaCode }
    { fsy with preamble = newPreamble }

// Must happen after `insertConstraints`
let replaceVars (fsy: FSY): FSY =
    let regex = "\$(?:[0-9]+)"
    let newRules = fsy.rules |> List.map (fun r -> 
        let newCases = r.cases |> List.map (fun c ->
            let (Code oldCode) = c.code
            let newCode = Regex.Replace(oldCode, regex, "semparVar")
            { c with code = Code newCode }        
        )
        { r with cases = newCases }
    )
    { fsy with rules = newRules }
    
let parse (input: string): FSY = 
    let lexbuf = LexBuffer<char>.FromString input
    let FSY = PreProcessingParser.start PreProcessingLexer.read lexbuf
    let processedFSY = FSY |> insertConstraints |> insertImport |> replaceVars
    processedFSY
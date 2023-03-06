module Parser

open FSharp.Text.Lexing

open PPType

let parse (input: string): FSY = 
    let lexbuf = LexBuffer<char>.FromString input
    printfn "INPUT: %A" input
    let FSY = PreProcessingParser.start PreProcessingLexer.read lexbuf
    printfn "FSY: %A" FSY
    FSY

let insertConstraints (fsy: FSY): FSY =
    let { rules = rules } = fsy
    let newRules = rules |> List.map (
        fun rule -> 
            let {cases = cases} = rule
            let newCases = cases |> List.map (
                fun case -> 
                    let {code = code; constraints = constraints } = case
                    let newCode = code.AddConstraints(constraints)
                    {case with code = newCode})
            {rule with cases = newCases})
    {fsy with rules = newRules}

let insertImport (fsy: FSY): FSY =
    let (PreaCode code) = fsy.preamble.preaCode
    let newPreaCode = PreaCode(code + "\nopen ParserType")
    let newPreamble = { fsy.preamble with preaCode = newPreaCode }
    { fsy with preamble = newPreamble }
module Parser

open FSharp.Text.Lexing

open PPType

let parse (input: string): FSY = 
    let splits = input.Split ("%%")
    match input.Length with
    | 2 -> let lexbuf = LexBuffer<char>.FromString splits[1]
           let rules = PreProcessingParser.rules PreProcessingLexer.read lexbuf
           { preamble = splits[0]; rules = rules }
    | _ -> { preamble = ""; rules = []}

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
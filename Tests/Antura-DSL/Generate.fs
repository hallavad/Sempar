module Generate

open FsCheck

let genWhitespace: Gen<string> = Gen.elements [" ";"\n";"\t"]

let genWhitespaces = genWhitespace |> Gen.listOf |> Gen.map (List.reduce (+))

let genValidChars = Gen.elements ["A";"B";"C";"D";"E";"F";"G";"H";"I";"J";"K";"L";"M";"N";"O";"P";"Q";"R";"S";"T";"U";"V";"W";"X";"Y";"Z";"Å";"Ä";"Ö";
                                     "a";"b";"c";"d";"e";"f";"g";"h";"i";"j";"k";"l";"m";"n";"o";"p";"q";"r";"s";"t";"u";"v";"w";"x";"y";"z";"å";"ä";"ö";
                                     "-";"_"
                                     ]

let genID = genValidChars |> Gen.nonEmptyListOf |> Gen.map (List.reduce (+))

let genPreamble (ws: bool) = gen {
    let preamble = "PREAMBLE WIHTOUT REAL CONTENT %%"
    match ws with
    | true ->
        let! ws1 = genWhitespaces
        let! ws2 = genWhitespaces
        return (ws1 + preamble + ws2)
    | false -> 
        return preamble } 

let genConstraint (ws: bool) = 
    match ws with
    | true -> gen {
        let! ws1 = genWhitespaces
        let constr = "SOME KIND OF CONSTRAINT. ANY F# CODE CAN GO HERE"
        let! ws2 = genWhitespaces
        return (ws1 + constr + ws2)}
    | false -> 
        genID
    |> Gen.map PPType.Constr

let genCode (ws: bool) = 
    match ws with
    | true -> gen {
        let! ws1 = genWhitespaces
        let code = "FSHARP CODE GOES HERE. NOT TESTED"
        let! ws2 = genWhitespaces
        return (ws1 + code + ws2)}
    | false -> 
        genID
    |> Gen.map PPType.Code

let genToken (ws: bool) = 
    match ws with
    | true -> gen {
        let! ws1 = genWhitespaces
        let! token = genID
        let! ws2 = genWhitespaces
        return (ws1 + token + ws2)}
    | false -> 
        genID
    |> Gen.map PPType.Token

let genName (ws: bool) = 
    match ws with
    | true -> gen {
        let! ws1 = genWhitespaces
        let! name = genID
        return (ws1 + name)}
    | false -> genID
        


let genCase (ws: bool) = 
    gen {   let! tokens = genToken ws |> Gen.listOf
            let! code = genCode ws 
            let! constraints = genConstraint ws |> Gen.listOf
            return ({tokens = tokens; code = code; constraints = constraints}: PPType.RuleCase) }


let genRule (ws: bool): Gen<PPType.Rule> = 
    gen {   let! name = genName ws 
            let! cases = genCase ws |> Gen.nonEmptyListOf 
            return {name = name; cases = cases} }

let genFSY (ws: bool) : Gen<PPType.FSY> = 
    gen {   let! preamble = genPreamble ws 
            let! rules = genRule ws |> Gen.nonEmptyListOf
            return {preamble = preamble; rules = rules} }

let addWSPreamble (preamble: string ) = gen {
    let! ws1 = genWhitespaces
    let! ws2 = genWhitespaces
    return (ws1 + preamble + ws2)
}

let addWSConstraint (PPType.Constr constr) = gen {
    let! ws1 = genWhitespaces
    let! ws2 = genWhitespaces
    return PPType.Constr (ws1 + constr + ws2)
}

let addWSCode (PPType.Code code) = gen {
    let! ws1 = genWhitespaces
    let! ws2 = genWhitespaces
    return PPType.Code (ws1 + code + ws2)
}

let addWSToken (PPType.Token token) = gen {
    let! ws1 = genWhitespaces
    let! ws2 = genWhitespaces
    return PPType.Token (ws1 + token + ws2)
}

let addWSName (name)  = gen {
    let! ws1 = genWhitespaces
    return (ws1 + name)
}


let addWSCase (case: PPType.RuleCase): Gen<PPType.RuleCase> = gen {
    let! newTokens = Gen.collect addWSToken case.tokens
    let! newCode = addWSCode case.code
    let! newConstraints = Gen.collect addWSConstraint case.constraints
    return {tokens = newTokens; code = newCode; constraints = newConstraints}

}


let addWSRule (rule: PPType.Rule): Gen<PPType.Rule> = gen {
    let! newName = addWSName rule.name
    let! newCases = Gen.collect addWSCase rule.cases
    return {name = newName; cases = newCases}
}

let addWSFSY (fsy: PPType.FSY): Gen<PPType.FSY> = gen {
    let! newPreamble = addWSPreamble fsy.preamble
    let! newRules = Gen.collect addWSRule fsy.rules
    return ({preamble = newPreamble; rules = newRules)
}

let addWS ()
module Generate

open FsCheck

let genWhitespace: Gen<string> = Gen.elements [" ";"\n";"\t"]

let genWhitespaces = genWhitespace |> Gen.nonEmptyListOf |> Gen.map (List.reduce (+))

let genValidChars = Gen.elements ["A";"B";"C";"D";"E";"F";"G";"H";"I";"J";"K";"L";"M";"N";"O";"P";"Q";"R";"S";"T";"U";"V";"W";"X";"Y";"Z";"Å";"Ä";"Ö";
                                     "a";"b";"c";"d";"e";"f";"g";"h";"i";"j";"k";"l";"m";"n";"o";"p";"q";"r";"s";"t";"u";"v";"w";"x";"y";"z";"å";"ä";"ö";
                                     "-";"_"
                                     ]

let genID = genValidChars |> Gen.nonEmptyListOf |> Gen.map (List.reduce (+))

let genPreamble = Gen.constant "PREAMBLE WIHTOUT REAL CONTENT"

let genConstraint = genID |> Gen.map PPType.Constr

let genCode = genID |> Gen.map PPType.Code

let genToken = genID |> Gen.map PPType.Token

let genName = genID

let genCase  = 
    gen {   let! tokens = genToken |> Gen.listOf
            let! code = genCode 
            let! constraints = genConstraint |> Gen.listOf
            return ({tokens = tokens; code = code; constraints = constraints}: PPType.RuleCase) }


let genRule : Gen<PPType.Rule> = 
    gen {   let! name = genName 
            let! cases = genCase |> Gen.nonEmptyListOf 
            return {name = name; cases = cases} }

let genFSY : Gen<PPType.FSY> = 
    gen {   let preamble = "PREAMBLE WITHOUT CONTENT" 
            let! rules = genRule |> Gen.nonEmptyListOf
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
    return ({preamble = newPreamble; rules = newRules})
}

type FSYWithWS = PPType.FSY * PPType.Code

let genFSYWithWS: Gen<FSYWithWS> = gen {
    let! fsy = genFSY
    let! fsyWS = addWSFSY fsy
    return (fsy, PPType.Code "KULKUL")
}

type FSYGenerator =
    static member FSY() = 
        {new Arbitrary<PPType.FSY>() with
            override x.Generator = genFSY
            override x.Shrinker t = Seq.empty }

type FSYWithWSGenerator =
    static member FSYWithWS() = 
        {new Arbitrary<FSYWithWS>() with
            override x.Generator = genFSYWithWS
            override x.Shrinker t = Seq.empty }


module PreProcessing

open FParsec
open HelperFunctions

type Processed = P 

type Code = string
type Token = string
type RuleCase = (Token list * Code)


type Rule = {
    constraints: string option
    name: string;
    cases: RuleCase list;
}

type FSY = {
    preamble: string;
    rules: Rule list;
}

let preprocess (contents : string[]) : Processed =
    P 


let pTokenDecls: Parser<string,unit> = manyCharsTill anyChar (pstring "%%")

let pRuleConstraint: Parser<string,unit> = pstring "//!" >>. restOfLine true

let pRuleName: Parser<string,unit> = manyCharsTill anyChar (pchar ':') 

let pRuleCaseTokens: Parser<Token list, unit> = manyTill (manyCharsTill anyChar spaces1) (pchar '{')

let pBetweenCurlys (p: Parser<'a, unit>): Parser<'a, unit> = between (pchar '{') (pchar '}') p

let pStringWithCurlys (p: Parser<string, unit>): Parser<string, unit> = pBetweenCurlys p |>> fun str -> "{" + str + "}"

let pUntilCurly: Parser<string, unit> = manyCharsTill anyChar (pchar '{')
let pAfterCurly: Parser<string, unit> = manyCharsTill anyChar (pchar '}')

let rec pNestedCurlys (p: Parser<string, unit>): Parser<string, unit> = 
    pNestedCurlys (pipe3 pUntilCurly p pAfterCurly 
        (fun before between after -> before + "{" + between + "}" + after)
    )

// let pRuleCaseCode: Parser<Code, unit> = pNestedCurlys ( pUntilCurly .>>. pBetweenCurlys |>> 
//     fun (before, between) -> 
//         match String.length before with
//         | 0 -> between
//         | _ -> before + "{" + between
    ) 
// let pRuleCaseCode: Parser<Code, unit> = manyCharsTill anyChar (pchar '}' >>? spaces >>? )

let pRuleSingleCase: Parser<RuleCase, unit> = pRuleCaseTokens .>>. pRuleCaseCode 

let pRuleCase: Parser<RuleCase, unit> = pchar '|' >>. pRuleCaseTokens .>>. pRuleCaseCode .>> newline

let pRuleCases = (pRuleSingleCase |>> fun x -> [x]) <|> manyTill pRuleCase (noneOf ['|'])

let pRule: Parser<Rule, unit> = 
    spaces >>.
    pipe3 (pRuleConstraint |>> Some <|> preturn None) pRuleName pRuleCases 
        (fun constraints name cases -> {constraints=constraints;name=name;cases=cases})

let pComment: Parser<string, unit> = spaces >>? pstring "//" >>? notFollowedBy (pchar '!') >>. restOfLine true


let pRules: Parser<Rule list, unit> = many ((pComment >>% None) <|> (pRule |>> Some) ) |>> List.choose id

let pFsy: Parser<FSY, unit> = 
    pipe2 pTokenDecls pRules (fun p rs -> {preamble=p;rules=rs})

let preprocess (contents : string) : Processed =
    match run pFsy contents with 
    | Failure (msg, _, _) -> printfn "Parsing failed %A" msg
    | Success (result, _, _) -> printfn "Parsing succeeded %A" result
    undefined
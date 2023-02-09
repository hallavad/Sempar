module PreProcessing

open FParsec
open HelperFunctions

type Processed = P 

type Code = string
type Token = string
type RuleCase = (Token list * Code)


type Rule = {
    name: string;
    cases: RuleCase list;
}

type FSY = {
    preamble: string;
    rules: Rule list;
}


let preprocess (contents : string array) : Processed =
    P 


let pTokenDecls: Parser<string,unit> = manyCharsTill anyChar (pstring "%%")

let pConstraint: Parser<string,unit> = pstring "//!" >>. restOfLine true

let pRuleName: Parser<string,unit> = manyCharsTill anyChar (pchar ':') 

let pRuleCaseTokens: Parser<Token list, unit> = manyTill (manyCharsTill anyChar spaces1) (pchar '{')

let pRuleCaseCode: Parser<Code, unit> = manyCharsTill anyChar (pchar '}')

let pRuleSingleCase: Parser<RuleCase, unit> = pRuleCaseTokens .>>. pRuleCaseCode 

let pRuleCase: Parser<RuleCase, unit> = pchar '|' >>. pRuleCaseTokens .>>. pRuleCaseCode .>> newline

let pRuleCases = (pRuleSingleCase |>> fun x -> [x]) <|> manyTill pRuleCase (noneOf ['|'])

let pRule: Parser<Rule, unit> = pipe2 pRuleName pRuleCases (fun n c -> {name=n;cases=c})

let pRules: Parser<Rule list, unit> = many pRule

let pFsy: Parser<FSY, unit> = pipe2 pTokenDecls pRules (fun p rs -> {preamble=p;rules=rs})

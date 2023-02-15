module PPType

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
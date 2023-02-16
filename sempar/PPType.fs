module PPType

let mapToString (os : 'a list) : string list = 
    List.map (fun x -> x.ToString()) os

let concatSpaces (ss : string list) : string = 
    String.concat " " ss

let concatNewlines (ss : string list) : string = 
    String.concat "\n" ss 

type Code = 
    | Code of string
        override this.ToString() =
            let (Code code) = this
            code
        member this.UsedVariables() =
            let (Code code) = this
            let mutable vars = []
            let mutable includeNext = false
            let mutable startedIncluding = false
            for (c : char) in code do 
                if includeNext then
                    if System.Char.IsNumber(c) then
                        vars <- $"${c}" :: vars
                        startedIncluding <- true
                    includeNext <- false
                else if startedIncluding then
                    if System.Char.IsNumber(c) then
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

type Constraint =
    | Constr of string
        override this.ToString() = 
            let (Constr constr) = this
            constr

type Token = 
    | Token of string
        override this.ToString() =
            let (Token token) = this
            token
        static member ListToString (ts : Token list) : string =
            ts |> mapToString |> concatSpaces

type RuleCase = 
    {
        tokens: Token list;
        code: Code;
        constraints: Constraint list;
    }
    override this.ToString() =
        "| " + Token.ListToString this.tokens + " { " + this.code.ToString() + " }"

type Rule = 
    {
        name: string;
        cases: RuleCase list;
    }
    override this.ToString() =
        "Rule"

type Rules = Rule list

type FSY = 
    {
        preamble: string;
        rules: Rule list;
    }
    override this.ToString() =
        this.preamble + "\n" + concatNewlines (mapToString this.rules)

let testCode = Code("test code that uses $3, $7 and $11")
let testConstraint = Constr("test constraint")
let testToken = Token("test token")
let testRuleCase = { tokens = [testToken]; code = testCode; constraints = [testConstraint] }
let testRule = { name = "Test rule"; cases = [testRuleCase] }
let testFSY = { preamble = "%token important\nTest FSY"; rules = [testRule]; }

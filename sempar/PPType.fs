module PPType

let mapToString (os : 'a list) : string list = 
    List.map (fun x -> x.ToString()) os

let concatSpaces (ss : string list) : string = 
    String.concat " " ss

let concatNewlines (ss : string list) : string = 
    String.concat "\n" ss 

type Constraint =
    | Constr of string
        override this.ToString(): string = 
            let (Constr constr) = this
            constr
        static member ListToString (cs : Constraint list) : string =
            cs |> mapToString |> concatNewlines

type Code = 
    | Code of string
        override this.ToString(): string =
            let (Code code) = this 
            code

        member this.UsedVariables: string list =
            let (Code code) = this
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

type Token = 
    | Token of string
    override this.ToString(): string =
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
    override this.ToString(): string =
        $"//! {Constraint.ListToString this.constraints} \n| {Token.ListToString this.tokens} {{ {this.code.ToString()} }}"
    
    member this.predefinedTokenIndices (usedTokens: string list): string list = 
        this.tokens 
            |> List.indexed 
            |> List.filter 
                (fun (_, (Token t)) -> List.contains t usedTokens) 
            |> List.map 
                (fun (i, _) -> string (i + 1))

type Rule = 
    {
        name: string;
        cases: RuleCase list;
    }
    override this.ToString(): string =
        $"{this.name}:\n {this.cases |> mapToString |> concatNewlines}"

type Rules = Rule list

type PreaItem = 
    {
        name: string;
        value: string;
    }
    override this.ToString(): string = 
        $"%%{this.name} {this.value}"

type PreaCode = 
    | PreaCode of string
    override this.ToString(): string =
        let (PreaCode code) = this
        $"""%%{{
{code}
%%}}
"""

type Preamble = 
    {
        preaCode: PreaCode;
        preaItems: PreaItem list;
    }
    override this.ToString(): string =
        $"""{this.preaCode.ToString()}

{concatNewlines (mapToString this.preaItems)}
"""
    member this.usedTokens: string list = 
        this.preaItems |> List.filter (fun i -> i.name = "token") |> List.map (fun i -> i.value)

type FSY = 
    {
        preamble: Preamble;
        rules: Rule list;
    }
    override this.ToString(): string =
        $"{this.preamble.ToString()} %%%%\n {concatNewlines (mapToString this.rules)}"
    

let testCode = Code("test code that uses $3, $7 and $11")
let testConstraint = Constr("test constraint that uses $2 and $5")
let testTokenA = Token("tokenA")
let testTokenB = Token("tokenB")
let testTokenC = Token("tokenC")
let testRuleCase = { tokens = [testTokenA; testTokenB; testTokenC]; code = testCode; constraints = [testConstraint] }
let testRule = { name = "test_rule"; cases = [testRuleCase] }
let testPreaCode = PreaCode("some test preamble code\nover two lines")
let testPreaItems = [ {name = "token"; value = "ID"}; {name = "type"; value = "<CoolType> start"} ]
let testPreamble = { preaCode = testPreaCode; preaItems = testPreaItems; }
let testFSY = { preamble = testPreamble; rules = [testRule]; }

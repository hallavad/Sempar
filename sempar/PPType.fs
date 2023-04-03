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
            $"//! {constr}\n"
        static member ListToString (cs : Constraint list) : string =
            cs |> mapToString |> concatNewlines

type Code = 
    | Code of string
        override this.ToString(): string =
            let (Code code) = this 
            code

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
        $"{Constraint.ListToString this.constraints}| {Token.ListToString this.tokens} {{{this.code.ToString()}}}"
    
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
        $"{this.name}:\n{this.cases |> mapToString |> concatNewlines}\n"

type Rules = Rule list

type PreaItem = 
    {
        name: string;
        value: string list;
    }
    override this.ToString(): string = 
        $"%%{this.name} {this.value |> mapToString |> concatSpaces}"

type PreaCode = 
    | PreaCode of string
    override this.ToString(): string =
        let (PreaCode code) = this
        $"%%{{\n{code}\n%%}}"

type Preamble = 
    {
        preaCode: PreaCode;
        preaItems: PreaItem list;
    }
    override this.ToString(): string =
        $"{this.preaCode.ToString()}\n\n{concatNewlines (mapToString this.preaItems)}"

    member this.usedTokens: string list = 
        this.preaItems |> List.filter (fun i -> i.name = "token") |> List.map (fun i -> List.last i.value)

type FSY = 
    {
        preamble: Preamble;
        rules: Rule list;
    }
    override this.ToString(): string =
        $"{this.preamble.ToString()}\n\n%%%%\n\n{concatNewlines (mapToString this.rules)}"
    

let testCode = Code("test code that uses $3, $7 and $11")
let testConstraint = Constr("test constraint that uses $2, $11 and $5")
let testTokenA = Token("tokenA")
let testTokenB = Token("OtherIDToken")
let testTokenC = Token("IDToken")
let testRuleCase = { tokens = [testTokenA; testTokenB; testTokenC]; code = testCode; constraints = [testConstraint] }
let testRule = { name = "test_rule"; cases = [testRuleCase] }
let testPreaCode = PreaCode("some test preamble code\nover two lines")
let testPreaItems = [ {name = "token"; value = ["IDToken"]}; { name = "token"; value = ["OtherIDToken"]}; {name = "type"; value = ["<CoolType>"; "start"]} ]
let testPreamble = { preaCode = testPreaCode; preaItems = testPreaItems; }
let testFSY = { preamble = testPreamble; rules = [testRule]; }

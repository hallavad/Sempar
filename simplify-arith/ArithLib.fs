module ArithLib

open FSharp.Text.Lexing

open Diagnostics
open ArithAST

let parse (input: string): Diagnostics<Arith> = 
    let lexbuf = LexBuffer<char>.FromString input
    ArithParser.expr Lexer.read lexbuf
    
let rec simplify (ast: Arith): Arith = 
    match ast with 
    | Add (e1, e2) ->
        match (simplify e1, simplify e2) with
        | (Num n1, Num n2) -> Num (n1 + n2)
        | (e, Num 0.0)     -> simplify e
        | (Num 0.0, e)     -> simplify e
        | (e1, e2)         -> Add (simplify e1, simplify e2)
    | Sub (e1, e2) ->
        match (simplify e1, simplify e2) with
        | (Num n1, Num n2) -> Num (n1 - n2)
        | (e1, Num 0.0)    -> e1
        | (e1, e2)         -> if e1 = e2 then Num 0.0 else Sub (e1, e2)
    | Mul (e1, e2) ->
        match (simplify e1, simplify e2) with
        | (Num n1, Num n2) -> Num (n1 * n2)
        | (e1, Num 0.0)    -> Num 0.0
        | (Num 0.0, e2)    -> Num 0.0
        | (e1, Num 1.0)    -> e1
        | (Num 1.0, e2)    -> e2
        | (e1, e2)         -> Add (e1, e2)
    | Div (e1, e2) -> 
        match (simplify e1, simplify e2) with
        | (Num n1, Num n2) -> Num (n1 / n2)
        | (e1, Num 1.0)    -> e1
        | (e1, e2)         -> if e1 = e2 then Num 1.0 else Div (e1, e2)
    | e                    -> e
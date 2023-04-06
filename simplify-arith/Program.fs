module Program

open System.IO
open FSharp.Text.Lexing

open Diagnostics
open ArithAST

let parse (input: string): DataModel.Rules = 
    let lexbuf = LexBuffer<char>.FromString input
    let res = Parser.start Lexer.read lexbuf
    printfn "Wrapped result: %A" res
    match res with 
    | OK r -> r
    | Warnings (r, ws) -> printfn "%A" ws; r
    | Errors es -> printfn "%A" es; exit 1

let simplify (ast: Arith): Arith = 
    match ast with 
    | Add (Num n1, Num n2) -> Num (n1 + n2)
    | Sub (Num n1, Num n2) -> Num (n1 - n2)
    | Mul (Num n1, Num n2) -> Num (n1 * n2)
    | Div (Num n1, Num n2) -> Num (n1 / n2)
    | Add (e, Num 0) -> e
    | Add (Num 0, e) -> e
    | Sub (e, Num 0) -> e
    | Sub (e1, e2)   -> if e1 = e2 then Num 0 else Sub (e1, e2)
    | Mul (e, Num 0) -> Num 0
    | Mul (Num 0, e) -> Num 0
    | Mul (e, Num 1) -> e
    | Mul (Num 1, e) -> e
    | Div (e, Num 1) -> e
    | Div (e1, e2)   -> if e1 = e2 then Num 1 else Div (e1, e2)

[<EntryPoint>]
let main argv =
    let contents = File.ReadAllLines "example/input.txt" |> String.concat "\n" 
    printfn "File: %A" contents
    let parseResult = contents |> parse 
    printfn "Result: %A" parseResult

    0
module Program

open System.IO
open FSharp.Text.Lexing

open Diagnostics

let parse (input: string): DataModel.Rules = 
    let lexbuf = LexBuffer<char>.FromString input
    let res = Parser.start Lexer.read lexbuf
    printfn "Wrapped result: %A" res
    match res with 
    | OK r -> r
    | Warnings (r, ws) -> printfn "%A" ws; r
    | Errors es -> printfn "%A" es; exit 1


[<EntryPoint>]
let main argv =
    let contents = File.ReadAllLines "example/input.txt" |> String.concat "\n" 
    printfn "File: %A" contents
    let parseResult = contents |> parse 
    printfn "Result: %A" parseResult

    0
module Program

open System.IO
open FSharp.Text.Lexing

let parse (input: string): DataModel.Rules = 
    let lexbuf = LexBuffer<char>.FromString input
    let res = Parser.start Lexer.read lexbuf
    res

[<EntryPoint>]
let main argv =
    let contents = File.ReadAllLines "example/input.txt" |> String.concat "\n" 
    let parseResult = contents |> parse 
    printfn "%A" parseResult

    0
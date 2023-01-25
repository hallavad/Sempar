module Program

open System.IO

open FSharp.Text.Lexing

open HelperFunctions

[<EntryPoint>]
let main argv =
    let contents = File.ReadAllLines(argv[1]) 

    let parse json = 
        let lexbuf = LexBuffer<char>.FromString json
        let res = ""// Parser.start Lexer.read lexbuf
        json

    let (parseResult) = simpleExample |> parse
    printfn "%A" (parseResult)
    0 


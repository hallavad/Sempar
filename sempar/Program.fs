module Program

open System.IO

open FSharp.Text.Lexing

open HelperFunctions

let parse processed = 
    // let lexbuf = LexBuffer<char>.FromString json
    let res = (* Parser.start Lexer.read lexbuf //*) processed
    res

[<EntryPoint>]
let main argv =
    match argv with 
    | argv when argv.Length <> 1 -> printf "Must provide exactly one file";  1
    | [| path |] ->
        let contents = File.ReadAllLines path
        let processed = PreProcessing.preprocess contents
        let result = parse processed
        printfn "%A" result
        0 



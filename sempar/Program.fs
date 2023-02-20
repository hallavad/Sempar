module Program

open System.IO

open FSharp.Text.Lexing

open Fli

open HelperFunctions
open ParserType
open PPType

[<EntryPoint>]
let main argv =
    match argv with 
    | argv when argv.Length <> 1 -> 
        printf "Must provide exactly one file"
        1
    | [| path |] ->
        let parse (input: string): FSY = 
            let splits = input.Split ("%%", 2)
            let lexbuf = LexBuffer<char>.FromString splits[1]
            let rules = PreProcessingParser.rules PreProcessingLexer.read lexbuf
            { preamble = splits[0]; rules = rules } : FSY


        let contents = File.ReadAllLines path |> String.concat "\n" 
        let parseResult = contents |> parse |> (fun x -> x.ToString())
        File.WriteAllText(path + ".ppfsy", parseResult)
        printfn "%s" parseResult

        0

        

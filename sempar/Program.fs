module Program

open System.IO

open FSharp.Text.Lexing

open Fli

open HelperFunctions
open ParserType

// let parse (processed: PPType.Processed) : ParserType<'a> = 
//     // let lexbuf = LexBuffer<char>.FromString json
//     let res = (* Parser.start Lexer.read lexbuf //*) processed
//     OK "cool parser type"

[<EntryPoint>]
let main argv =
    match argv with 
    | argv when argv.Length <> 1 -> 
        printf "Must provide exactly one file"
        1
    | [| path |] ->
        let parse (input: string) = 
            let splits = input.Split ("%%", 2)
            let lexbuf = LexBuffer<char>.FromString splits[1]
            let rules = PreProcessingParser.rules PreProcessingLexer.read lexbuf
            { preamble = splits[0] ;rules = rules} : PPType.FSY


        let contents = File.ReadAllLines path |> String.concat "\n" 

        let (parseResult) = contents |> parse
        printfn "%A" (parseResult)
        0

        // call fslex/yacc on generated files to build Parser.fs
        
        // cli {
        //     Shell PS
        //     Command ("cat " + path)
        // }
        // |> Command.execute
        // |> Output.printText

        // 0
        
        // This should be moved to the first rule of the parser:
        (*
        match parsed with
        | Errors es -> 
            printfn "errors, no file produced: %A" es
            2
        | Warnings (pt, ws) -> 
            printfn "warnings: %A\n%A" ws pt
            0
        | OK pt -> 
            printfn "All OK: %A" pt
            0
        *)


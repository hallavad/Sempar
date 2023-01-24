module Program
open FSharp.Text.Lexing

[<EntryPoint>]
let main argv =
    let parse json = 
        let lexbuf = LexBuffer<char>.FromString json
        // let res = Parser.start Lexer.read lexbuf
        lexbuf
    let simpleJson = @"rule Testing for real: 
      project types: 
      ""Test test test""  
    "
    let (parseResult) = simpleJson |> parse
    printfn "%A" parseResult
    0
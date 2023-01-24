module Program
open FSharp.Text.Lexing

[<EntryPoint>]
let main argv =
    let parse json = 
        let lexbuf = LexBuffer<char>.FromString json
        let res = Parser.start Lexer.read lexbuf
        res
    let simpleJson = @"precision 2
      rule satt automatiskt produktionsstart:
        project types: 
          ""Göteborg - 1 Små projekt under 100 tkr""
          ""Göteborg - 2 Mindre projekt mellan 100tkr -625tkr""
          ""Göteborg - 3 Standard projekt över 625 tkr""
        sources:
          task 
            when Referens = genomför 
            get StartDate
        transformation:
          none
        destination:
          project property Produktionsstart 
    "
    let (parseResult) = simpleJson |> parse
    printfn "%A" (parseResult)
    0 
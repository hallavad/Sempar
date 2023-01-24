module Program
open FSharp.Text.Lexing
let simpleJson = @"precision 2
      rule satt automatiskt produktionsstart:
        project types: 
          ""Goteborg - 1 Sma projekt under 100 tkr""
          ""Goteborg - 2 Mindre projekt mellan 100tkr -625tkr""
          ""Goteborg - 3 Standard projekt over 625 tkr""
      sources:
          task 
            when referens = genomfor 
            get startdate
        transformation:
          none
        destination:
          project property produktionsstart 
    "
[<EntryPoint>]
let main argv =
    let parse json = 
        let lexbuf = LexBuffer<char>.FromString json
        let res = Parser.start Lexer.read lexbuf
        res
    let simpleExample = @"precision 2
      rule satt automatiskt produktionsstart:
        project types: 

      sources:
          task 
            when referens = genomfor 
            get startdate"
    let x = @"    
      transformation:
          none
        destination:
          project property produktionsstart 
    "
    let (parseResult) = simpleExample |> parse
    printfn "%A" (parseResult)
    0 


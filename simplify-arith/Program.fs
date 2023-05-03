module Program

open System.IO

open Diagnostics
open ArithLib

[<EntryPoint>]
let main argv =
    let contents = File.ReadAllLines "example/input.txt" |> String.concat "\n" 
    let parseResult = contents |> parse 
    let parseResult = match parseResult with 
                      | OK r -> 
                          r
                      | Warnings (r, ws) -> 
                          printfn "Warnings: %A" ws
                          r
                      | Errors es -> 
                          printfn "Error: %A" es; exit 1
    printfn "Unsimplifed: %A" parseResult
    let simplified = parseResult |> simplify
    printfn "Result: %A" simplified

    0
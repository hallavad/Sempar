module Program

open System.IO

[<EntryPoint>]
let main argv =
    match argv with 
    | argv when argv.Length <> 1 -> 
        printf "Must provide exactly one file"
        1
    | [| path |] ->
        let contents = File.ReadAllLines path |> String.concat "\n" 
        let parseResult = contents |> Parser.parse |> Parser.insertConstraints |> (fun x -> x.ToString())
        File.WriteAllText(path + ".ppfsy", parseResult)
        //printfn "%s" parseResult

        0

        

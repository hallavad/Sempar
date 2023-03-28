module Program

open System
open System.IO
open Argu

type CliArguments = 
    | [<MainCommand; ExactlyOnce; Last>] InputFile of path:string
    | OutputFile of path:string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | InputFile _ -> "The input .sfsy file"
            | OutputFile _ -> "Specify an output file (default: append .ppfsy to inputfilename)"
        

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(colorizer = function ErrorCode.HelpText -> None | _ -> Some ConsoleColor.Red)
    let argParser = ArgumentParser.Create<CliArguments>(errorHandler = errorHandler)
    let arguments = argParser.Parse argv

    if not (arguments.Contains InputFile) then 0 else

    let inputfilePath = arguments.GetResult InputFile

    let outputFilePath = match arguments.Contains OutputFile with
                                | false -> inputfilePath + ".ppfsy"
                                | true -> arguments.GetResult OutputFile

    let contents = File.ReadAllLines inputfilePath |> String.concat "\n" 
    let parseResult = contents |> Parser.parse |> Parser.preprocess |> (fun x -> printfn "%A" x; x.ToString())
    File.WriteAllText(outputFilePath, parseResult)

    0

        

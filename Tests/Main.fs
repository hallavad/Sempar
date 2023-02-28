module Main

open Expecto


let config = { FsCheckConfig.defaultConfig with arbitrary = [typeof<Generate.FSYWithWSGenerator>]}

[<Tests>]
let properties =
    testList "Check FSY definition" [
        testPropertyWithConfig config "Stringify of FSY returns the same FSY when parsed" <| 
            fun (fsy, _) -> 
                Parser.parse (fsy.ToString()) |> Expect.equal <| fsy
        testPropertyWithConfig config "Arbitrary Whitespaceing" <| 
            fun (fsy, fsyWS )->
                Expect.equal (Parser.parse (fsyWS.ToString())) fsy
    ]

[<EntryPoint>]
let main argv =

    Tests.runTests defaultConfig properties


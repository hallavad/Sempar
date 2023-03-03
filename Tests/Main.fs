module Main

open Expecto


let config = { FsCheckConfig.defaultConfig with arbitrary = [typeof<Generate.FSYGenerator>]}

[<Tests>]
let properties =
    testList "Check FSY definition" [
        testPropertyWithConfig config "Stringify of FSY returns the same FSY when parsed" <| 
            fun fsy ->
                Expect.equal fsy (Parser.parse (fsy.ToString()))
        // testPropertyWithConfig config "Arbitrary Whitespaceing" <| 
        //     fun (fsy, fsyWS )->
        //         Expect.equal (Parser.parse (fsyWS.ToString())) fsy
    ]

[<EntryPoint>]
let main argv =

    Tests.runTests defaultConfig properties


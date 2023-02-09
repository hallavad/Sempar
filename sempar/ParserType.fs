module ParserType

open HelperFunctions

type Error = string
type Warning = W

type ParserType<'a> =
    | Errors of Error list
    | Warnings of ('a * Warning list)
    | OK of 'a

let (<|>) (pt1: ParserType<'a>) (pt2: ParserType<'b>) : ParserType<'b> = 
    match pt1 with
    | OK a -> 
        match pt2 with
        | OK b -> OK b
        | Warnings (b, ws) -> Warnings (b, ws)
        | Errors es -> Errors es
    | Warnings (a, ws) -> 
        match pt2 with
        | OK b -> Warnings (b, ws)
        | Warnings (b, ws2) -> Warnings (b, List.append ws ws2)
        | Errors es -> Errors es
    | Errors es -> 
        match pt2 with 
        | OK b -> Errors es
        | Warnings (b, ws) -> Errors es
        | Errors es2 -> Errors (List.append es es2)

type ParserTypeBuilder() =
    member this.Delay(f: unit -> ParserType<'a>): ParserType<'a> =
        f()

    member this.Combine(x: ParserType<'a>, y: ParserType<'b>): ParserType<'b> =
        x <|> y

    member this.Return(x) =
        OK x

let parserType = new ParserTypeBuilder()

let assertWith (e: Error) (c: bool): ParserType<unit> = if c then OK () else Errors [e]

let x: obj = parserType {
    assertWith "sadasd" false
    return "as"
}



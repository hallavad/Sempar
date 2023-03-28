module ParserType

open HelperFunctions

type Error = string
type Warning = string

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
        | Warnings (b, ws') -> Warnings (b, List.append ws ws')
        | Errors es -> Errors es
    | Errors es -> 
        match pt2 with 
        | OK b -> Errors es
        | Warnings (b, ws) -> Errors es
        | Errors es2 -> Errors (List.append es es2) // Questionable if we should append here, not the same behaviour as Bind 

type ParserTypeBuilder() =
    member this.Delay(f: unit -> ParserType<'a>): ParserType<'a> =
        f()

    member this.Bind(x: ParserType<'a>, f: 'a -> ParserType<'b>): ParserType<'b> =
        match x with
        | OK a -> f a
        | Warnings (a, ws) -> 
            match f a with
            | OK b -> Warnings (b, ws)
            | Warnings (b, ws') -> Warnings (b, List.append ws ws')
            | Errors es -> Errors es
        | Errors es -> Errors es

    member this.Combine(x: ParserType<'a>, y: ParserType<'b>): ParserType<'b> =
        x <|> y

    member this.Return(x) =
        OK x

    member this.MergeSources((x,y): (ParserType<'a> * ParserType<'b>)): ParserType<'a * 'b> =
        match x with
        | OK a -> 
            match y with
            | OK b -> OK (a, b)
            | Warnings (b, ws) -> Warnings ((a, b), ws)
            | Errors es -> Errors es
        | Warnings (a, ws) -> 
            match y with 
            | OK b -> Warnings ((a, b), ws)
            | Warnings (b, ws') -> Warnings ((a, b), List.append ws ws')
            | Errors es -> Errors es
        | Errors es -> 
            match y with
            | OK b -> Errors es
            | Warnings (b, ws) -> Errors es
            | Errors es' -> Errors (List.append es es')

let parserType = new ParserTypeBuilder()

let warn (w: Warning): ParserType<unit> = Warnings ((), [w])
let error (e: Error): ParserType<unit> = Errors [e]
let warnWhen (w: Warning) (c: bool): ParserType<unit> = if c then OK () else warn w
let warnUnless (w: Warning) (c: bool): ParserType<unit> = if not c then OK () else warn w
let errorWhen (e: Error) (c: bool): ParserType<unit> = if c then OK () else error e
let errorUnless (e: Error) (c: bool): ParserType<unit> = if not c then OK () else error e

let x = parserType {
    let! x = OK 1
    errorWhen "sadasd" false
    return x
}



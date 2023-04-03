module Diagnostics

open HelperFunctions

type Error = string
type Warning = string

type Diagnostics<'a> =
    | Errors of Error list
    | Warnings of ('a * Warning list)
    | OK of 'a

let (<|>) (pt1: Diagnostics<'a>) (pt2: Diagnostics<'b>) : Diagnostics<'b> = 
    match pt1 with
    | OK a -> 
        match pt2 with
        | OK b -> OK b
        | Warnings (b, ws) -> Warnings (b, ws)
        | Errors es -> Errors es
    | Warnings (a, ws) -> 
        match pt2 with
        | OK b -> Warnings (b, ws)
        | Warnings (b, ws') -> Warnings (b, ws @ ws')
        | Errors es -> Errors es
    | Errors es -> 
        match pt2 with 
        | OK b -> Errors es
        | Warnings (b, ws) -> Errors es
        | Errors es2 -> Errors (es @ es2) // Questionable if we should append here, not the same behaviour as Bind 

type DiagnosticsBuilder() =
    member this.Delay(f: unit -> Diagnostics<'a>): Diagnostics<'a> =
        f()

    member this.Bind(x: Diagnostics<'a>, f: 'a -> Diagnostics<'b>): Diagnostics<'b> =
        match x with
        | OK a -> f a
        | Warnings (a, ws) -> 
            match f a with
            | OK b -> Warnings (b, ws)
            | Warnings (b, ws') -> Warnings (b, ws @ ws')
            | Errors es -> Errors es
        | Errors es -> Errors es

    member this.Combine(x: Diagnostics<'a>, y: Diagnostics<'b>): Diagnostics<'b> =
        x <|> y

    member this.Return(x) =
        OK x

    member this.MergeSources((x,y): (Diagnostics<'a> * Diagnostics<'b>)): Diagnostics<'a * 'b> =
        match x with
        | OK a -> 
            match y with
            | OK b -> OK (a, b)
            | Warnings (b, ws) -> Warnings ((a, b), ws)
            | Errors es -> Errors es
        | Warnings (a, ws) -> 
            match y with 
            | OK b -> Warnings ((a, b), ws)
            | Warnings (b, ws') -> Warnings ((a, b), ws @ ws')
            | Errors es -> Errors es
        | Errors es -> 
            match y with
            | OK b -> Errors es
            | Warnings (b, ws) -> Errors es
            | Errors es' -> Errors (es @ es')
    
    member this.ReturnFrom(x: Diagnostics<'a>): Diagnostics<'a> =
        x

    member this.Zero(() : unit): Diagnostics<'a> = 
        Errors ["Zero called, shouldn't be!"]

let sempar = new DiagnosticsBuilder()

let warn (w: Warning): Diagnostics<unit> = Warnings ((), [w])
let error (e: Error): Diagnostics<unit> = Errors [e]
let warnWhen (w: Warning) (c: bool): Diagnostics<unit> = if c then warn w else OK ()
let warnUnless (w: Warning) (c: bool): Diagnostics<unit> = if not c then warn w else OK ()
let errorWhen (e: Error) (c: bool): Diagnostics<unit> = if c then error e else OK ()
let errorUnless (e: Error) (c: bool): Diagnostics<unit> = if not c then error e else OK ()

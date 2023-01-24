module Program2

open FParsec
open Microsoft.FSharp.Core.Option

let undefined<'T> : 'T = failwith "Not implemented yet"
let _pendedBy s = charsTillString s System.Int32.MaxValue

let _pprecision = skipString "precision" >>. spaces >>. puint64

let _pruleheader = skipString "rule" >>. _pendedBy ":" 

let _prule = _pruleheader .>>. undefined

let _pfile = opt _pprecision .>>. many _prule
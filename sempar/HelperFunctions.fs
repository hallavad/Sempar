module HelperFunctions

let undefined<'T> : 'T = failwith "Not implemented yet"

let charsToString (chars: char list): string =  new string(List.toArray chars)
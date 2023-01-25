module ParserType

type Error = E
type Warning = W

type ParserType<'a> =
    | Errors of Error list
    | Warnings of ('a * Warning list)
    | OK of 'a
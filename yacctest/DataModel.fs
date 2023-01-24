module DataModel

type Transformation =
  | NoTransform
  | Arith of string

type Task = (string * string * string)
type Destination =
  | Property of string

type Type = string

type Rule = 
  {
    name: string option;
    types: Type list;
    sources: Task list;
    transformation: Transformation option;
    destination: Destination;
  }

type Rules = 
  {
    precision: uint option;
    rules: Rule list;
  }
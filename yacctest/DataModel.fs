module DataModel

type Transformation =
  | All 
  | Some of string

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
    destination: Destination option;
  }

type Rules = 
  {
    precision: uint option;
    rules: Rule list;
  }
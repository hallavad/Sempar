module DataModel

type Transformation =
  | All 
  | Some of string

type Task = (string * string * string)
type Destination =
  | Property of string

type Rule = 
  {
    name: string option;
    types: string list;
    sources: Task list;
    transformation: Transformation option;
    destination: Destination option;
  }

type Rules = 
  {
    precision: uint option;
    rules: Rule list;
  }
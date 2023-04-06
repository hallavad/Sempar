module DataModel

type Transformation = string

type Destination =
  | Property of string

type Type = string

type Source = 
  Task  of (string * string * string)
  | DecisionPoint of (int * string) 
  | NumericProjectProperty of (string * string)

type Rule = 
  {
    name: string option;
    types: Type list;
    sources: Source list;
    transformation: Transformation option;
    destination: Destination;
  }

type Rules = 
  {
    precision: int option;
    rules: Rule list;
  }
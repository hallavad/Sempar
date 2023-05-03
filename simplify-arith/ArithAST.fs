module ArithAST

type Arith = 
    | Add of (Arith * Arith)
    | Sub of (Arith * Arith)
    | Mul of (Arith * Arith)
    | Div of (Arith * Arith)
    | Num of float
    | Var of string

let rec arithToString (arith: Arith): string = 
    match arith with 
    | Add (e1, e2) -> $"({arithToString e1} + {arithToString e2})"
    | Sub (e1, e2) -> $"({arithToString e1} - {arithToString e2})"
    | Mul (e1, e2) -> $"({arithToString e1} * {arithToString e2})"
    | Div (e1, e2) -> $"({arithToString e1} / {arithToString e2})"
    | Num n        -> string n
    | Var v        -> v
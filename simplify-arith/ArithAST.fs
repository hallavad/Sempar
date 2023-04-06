module ArithAST

type Arith = 
    | Add of (Arith * Arith)
    | Sub of (Arith * Arith)
    | Mul of (Arith * Arith)
    | Div of (Arith * Arith)
    | Num of float
    | Var of string

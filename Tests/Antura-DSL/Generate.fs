module Generate

open FsCheck


let validChars = Gen.elements ['A';'B';'C';'D';'E';'F';'G';'H';'I';'J';'K';'L';'M';'N';'O';'P';'Q';'R';'S';'T';'U';'V';'W';'X';'Y';'Z';'Å';'Ä';'Ö';
                                     'a';'b';'c';'d';'e';'f';'g';'h';'i';'j';'k';'l';'m';'n';'o';'p';'q';'r';'s';'t';'u';'v';'w';'x';'y';'z';'å';'ä';'ö';
                                     '-';'_'
                                     ]

let genID = validChars |> Gen.nonEmptyListOf |> Gen.map (fun s -> s |> List.map string |> List.reduce (+))

let genConstraints = genID |> Gen.map PPType.Constr
let genToken = genID |> Gen.map PPType.Token


let genCase = 
    gen {   let! tokens = genToken |> Gen.listOf
            let code = PPType.Code "FSHARP CODE GOES HERE. NOT PART OF TESTING"
            let! constraints = genConstraints |> Gen.listOf
            return ({tokens = tokens; code = code; constraints = constraints}: PPType.RuleCase) }

let genRule: Gen<PPType.Rule> = 
    gen {   let! name = genID 
            let! cases = genCase |> Gen.nonEmptyListOf 
            return {name = name; cases = cases}}

let genFSY: Gen<PPType.FSY> = 
    gen {   let preamble = "PREAMBLE WIHTOUT REAL CONTENT %%"
            let! rules = genRule |> Gen.nonEmptyListOf
            return {preamble = preamble; rules = rules} }
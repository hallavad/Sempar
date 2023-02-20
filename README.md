# Sempar

Sempar is a tool developed to aid in creating external DSLs. It's core functionality is based on FSLexYacc and the syntax used by it to generate parsers. Sempar then aims to put an extra layer on top of said functionality allowing the language creator to give warnings or errors from "any" F# code.

## Repository structure

The repository has a couple of folders at top level
* examples - A folder containing some examples of input and it's expected output.
* parsing - A .NET project testing the functionality of FParsec.
* yacctest - A .NET project testing the functionality of FSLexYacc.
* sempar - Main .NET project containing the project's actual code.

## Usage

Install .NET 7 (can be installed from [here](https://dotnet.microsoft.com/en-us/download))

Clone this repo
`git clone git@github.com/hallavad/exjobb.git`

Move into the sempar directory
`cd exjobb/sempar`

Build & Run the program
`dotnet run -- [file]`
where file is the file .sfsy containing the parser specification for your DSL, including constraints. An example can be found in `exjobb/sempar/examples/antura-DSL`.

The program generates an .ppfsy file which can then be used with FSLexYacc as a parser for your language. An example project that uses the generated file can be found in the folder `antura-dsl-parser`.s

*MORE INFORMATION ABOUT HOW THE RESULTING .fs CAN BE USED. WHAT DATATYPE DOES IT RETURN AND HOW CAN YOU HANDLE THE POTENTIAL ERRORS AND WARNINGS.*


## How it works


## Future

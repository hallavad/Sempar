# Sempar

Sempar is a tool developed to aid in creating external DSLs. It's core functionality is based on FSLexYacc and the syntax used by it to generate parsers. Sempar then aims to put an extra layer on top of said functionality allowing the language creator to give warnings or errors from "any" F# code.

## Repository structure

The repository has a couple of folders at top level
* sempar - Main .NET project containing the Sempar tool.
* simplify-arith - An implementation of Arithmetic expression using the Sempar tool 
* antura-dsl - An implementation of Anturas DSL using the Sempar tool 

## Usage

### MSBuild functionality
The recommended way to use the tool is to depend on it using MSBuild. 
To use it, import `Sempar.targets` from your .fsproj file:
`<Import Project="../sempar/Sempar.targets"/>`. 
Then, include the Sempar target, pointing to your .sfsy file:
`<Sempar Include="inputs/Parser.sfsy" />`. 
When building your project, a file will be created with the .ppsfsy file ending.
This file can then be used by fslexyacc to generate the files to be included in your project. See the [documentation for fslexyacc](https://fsprojects.github.io/FsLexYacc/fsyacc.html) for usage.

An example of this functionality can be seen in the `.fsproj` file of both the `antura-dsl` and the `simplify-arith` implementations.


### Running the executable
Install .NET 7 (can be installed from [here](https://dotnet.microsoft.com/en-us/download))
or from your [favorite package manager](https://learn.microsoft.com/en-us/dotnet/core/install/linux). 

Clone this repo
`git clone git@github.com/hallavad/exjobb.git`

Move into the sempar directory
`cd exjobb/sempar`

Build & Run the program
`dotnet run [file]`
where file is the .sempar file containing the parser specification for your language, including constraints. An example can be found in `antura-dsl/inputs/AnturaParser.sempar`.

The program generates an .fsy file which can then be used with FSLexYacc as a parser for your language. By default the file generated will append .fsy after .sempar. This can be changed by adding the `--outputfile [path]` argument when running sempar, i.e. running `dotnet run --outputfile [output path] [input path]`. An example project that uses the generated file can be found in the folder `antura-dsl` or `simplify-arith`.

### Running the example implementations
In order to run either the `antura-dsl` or the `simplify-arith` implementations you can do the following.

Make sure .NET is installed as described in the section above.

move in to the relevant directory for the implementation and run `dotnet build` followed by `dotnet run`. 

*Side note: the run command will not reliably rebuild the .sempar.fsy file, hence the need to run build beforehand. If you're only making changes to files not needed to send into FsLex & FsYacc, usually files that the .sempar.fsy file doesn't depend on, the build command is unnecessary.*


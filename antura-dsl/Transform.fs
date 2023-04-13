module Transform

open DataModel
open Chiron

let concatNewlines (ss : string list) : string = 
    String.concat "\n" ss 

let tab = "    "
let rec tabs (n) = 
    match n with 
        | 0 -> "" 
        | _ -> tabs (n-1) + tab 

let prefixTabs (n) (str)= (tabs n) + str

type TransformedDestination = 
    {
        kind: string;
        property: string;
    } 
    static member ToJson (x:TransformedDestination) = json {
        do! Json.write "kind" x.kind
        do! Json.write "property" x.property
    }


type TransformedTransformationNone = 
    {
        kind: string; 
    }
    static member ToJson (x:TransformedTransformationNone) = json {
        do! Json.write "kind" x.kind
    }

type TransformedTransformationArith = 
    {
        kind: string;
        value: string;
    }
    static member ToJson (x:TransformedTransformationArith) = json {
        do! Json.write "kind" x.kind
        do! Json.write "value" x.value
    }

type TransformedTransformation = 
    NoTrans of TransformedTransformationNone 
    | Arith of TransformedTransformationArith
    static member ToJson (x: TransformedTransformation) = json {
        do! match x with
            | NoTrans nt -> TransformedTransformationNone.ToJson nt 
            | Arith ar -> TransformedTransformationArith.ToJson ar
    }

type TransformedSourceTaskFilter = 
    {
        kind: string;
        taskPropertyName: string;
        taskPropertyValue: string; 
    }
    static member ToJson (x:TransformedSourceTaskFilter) = json {
        do! Json.write "kind" x.kind
        do! Json.write "taskPropertyName" x.taskPropertyName
        do! Json.write "taskPropertyValue" x.taskPropertyValue
    }

type TransformedSourceTask =
    {
        kind: string;
        findTaskBy: TransformedSourceTaskFilter; 
        taskSourceValue: string;
    }
    static member ToJson (x:TransformedSourceTask) = json {
        do! Json.write "kind" x.kind
        do! Json.write "findTaskBy" x.findTaskBy
        do! Json.write "taskSourceValue" x.taskSourceValue
    }

type TransformedSourceDecisionPointFilter = 
    {
        kind: string;
        oneBasedIndex: int;
    }
    static member ToJson (x:TransformedSourceDecisionPointFilter) = json {
        do! Json.write "kind" x.kind
        do! Json.write "oneBasedIndex" x.oneBasedIndex
    }

type TransformedSourceDecisionPoint =
    {
        kind: string;
        findDecisionPointBy: TransformedSourceDecisionPointFilter; 
        decisionPointSourceValue: string;
    }
    static member ToJson (x:TransformedSourceDecisionPoint) = json {
        do! Json.write "kind" x.kind
        do! Json.write "findDecisionPointBy" x.findDecisionPointBy
        do! Json.write "decisionPointSourceValue" x.decisionPointSourceValue
    }

type TransformedSourceProjectPropertyFilter =
    {
        kind: string;
        name: string;
    }
    static member ToJson (x:TransformedSourceProjectPropertyFilter) = json {
        do! Json.write "kind" x.kind
        do! Json.write "name" x.name
    }

type TransformedSourceNumericProjectProperty =
    {
        kind: string;
        findProjectPropertyBy: TransformedSourceProjectPropertyFilter; 
        projectPropertySourceValue: string;
    }
    static member ToJson (x:TransformedSourceNumericProjectProperty) = json {
        do! Json.write "kind" x.kind
        do! Json.write "findProjectPropertyBy" x.findProjectPropertyBy
        do! Json.write "projectPropertySourceValue" x.projectPropertySourceValue
    }


type TransformedSource = 
    Task of TransformedSourceTask 
    | DecisionPoint of TransformedSourceDecisionPoint 
    | NumericProjectProperty of TransformedSourceNumericProjectProperty
    static member ToJson (x:TransformedSource) = json {
        do! match x with
            | Task t -> TransformedSourceTask.ToJson t 
            | DecisionPoint dp -> TransformedSourceDecisionPoint.ToJson dp 
            | NumericProjectProperty np -> TransformedSourceNumericProjectProperty.ToJson np
    }

type TransformedTypesMultiple = 
    {
        kind: string;
        multipleProjectTypes: string list;
    }
    static member ToJson (x: TransformedTypesMultiple) = json {
        do! Json.write "kind" x.kind
        do! Json.write "multipleProjectTypes" x.multipleProjectTypes
    }


type TransformedTypesSingle = 
    {
        kind: string;
        projectType: string; 
    }
    static member ToJson (x: TransformedTypesSingle) = json {
        do! Json.write "kind" x.kind
        do! Json.write "projectTypes" x.projectType
    }


type TransformedTypes = 
    Multiple of TransformedTypesMultiple 
    | Single of TransformedTypesSingle
    static member ToJson (x: TransformedTypes) = json {
        do! match x with
            | Multiple ts -> TransformedTypesMultiple.ToJson ts 
            | Single t -> TransformedTypesSingle.ToJson t 
    }

type TransformedRule = 
    {
        name: string option
        validFor: TransformedTypes; 
        valueSource: TransformedSource list;
        transformation: TransformedTransformation;
        destination: TransformedDestination
    }
    static member ToJson (x: TransformedRule) = json {
        match x.name with
        | Some n -> do! Json.write "_name" x.name
        | None -> failwith "Not Implemented"

        do! Json.write "validFor" x.validFor
        do! Json.write "valueSource" x.valueSource
        do! Json.write "transformation" x.transformation
        do! Json.write "destination" x.destination
    }
    
type TransformedRules = 
    {
        schemaVersion: int;
        decimalPrecision: int;
        rules: TransformedRule list;
    }
    static member ToJson (x: TransformedRules) = json {
        do! Json.write "schemaVersion" x.schemaVersion
        do! Json.write "decimalPrecision" x.decimalPrecision
        do! Json.write "rules" x.rules
    }

let transformTransformation (trans: DataModel.Transformation option): TransformedTransformation =
    match trans with
    | None -> 
        NoTrans { 
            kind = "NoTransformation" 
        }
    | Some(trans) -> 
        Arith { 
            kind = "ArithmeticOperation";
            value = trans
        }

    
let transformSource (source: DataModel.Source): TransformedSource=
    match source with 
        | DataModel.Task (name, value, sourceValue) -> 
            let findTaskBy = {
                kind = "PropertyNameValue";
                taskPropertyName =  name;
                taskPropertyValue = value
            }
            Task  {
                kind = "Task";
                findTaskBy = findTaskBy;
                taskSourceValue = sourceValue
            }
        | DataModel.DecisionPoint (index, sourceValue) ->
            let findDecisionPointBy = {
                kind = "OneBasedIndex";
                oneBasedIndex = index
            }
            DecisionPoint {
                kind = "DecisionPoint";
                findDecisionPointBy = findDecisionPointBy;
                decisionPointSourceValue = sourceValue
            }
        | DataModel.NumericProjectProperty (name, sourceValue) -> 
            let findProjectPropertyBy = {
                kind = "Name";
                name = name
            }
            NumericProjectProperty {
                kind = "NumericProjectProperty";
                findProjectPropertyBy = findProjectPropertyBy;
                projectPropertySourceValue = sourceValue
            }


let transformDestination (DataModel.Property destination: DataModel.Destination): TransformedDestination =
    {
        kind = "ProjectProperty";
        property = destination
    }

let transformRule (rule: DataModel.Rule): TransformedRule =
    let TransformedType = 
        match rule.types.Length with
        | 1 -> 
            Single { 
                kind = "ProjectType";
                projectType = rule.types[0]
            }
        | _ -> 
            Multiple { 
                kind = "MultipleProjectTypes";
                multipleProjectTypes =  rule.types
            }
    
    {
        name = rule.name
        validFor = TransformedType;
        valueSource =  List.map transformSource rule.sources
        transformation = transformTransformation rule.transformation
        destination = transformDestination rule.destination
    }

let transformRules (rules: DataModel.Rules): TransformedRules = 
    let precision = 
        match rules.precision with
        | Some(n) -> n
        | None -> 2

    { 
        schemaVersion = 1; 
        decimalPrecision = precision;
        rules = List.map transformRule rules.rules
    }

let transform (rules: DataModel.Rules): string =
    rules 
    |> transformRules 
    |> Json.serialize
    |> Json.formatWith JsonFormattingOptions.Pretty
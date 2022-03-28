open System
open System.Collections.Generic
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

// Question: Fastest way to lookup a value for two int32

// let pair = 1, 2
// let structPair = struct (1, 2)

type PairRecord =
    {
        X : int
        Y : int
    }

[<Struct>]
type StructPairRecord =
    {
        X : int
        Y : int
    }



type Benchmarks () =

    let defaultTupleDict = Dictionary<(int * int), float>()
    let structTupleDict = Dictionary<struct (int * int), float>()
    let defaultRecordDict = Dictionary<PairRecord, float>()
    let structRecordDict = Dictionary<StructPairRecord, float>()

    let rng = Random 123
    let maxKeyValue = 100
    let maxValueValue = 100.0
    let numberOfPairs = 100
    let numberOfLookups = 1_000


    let randomKeysAndValues =
        [ for _ in 1 .. numberOfPairs ->
            let x = rng.Next maxKeyValue
            let y = rng.Next maxKeyValue
            let value = (rng.NextDouble ()) * maxValueValue
            (x, y), value
        ] |> List.distinctBy fst

    do for ((x, y), value) in randomKeysAndValues do
        
        defaultTupleDict[(x, y)] <- value

        structTupleDict[struct (x, y)] <- value
        
        let defaultRecordPair : PairRecord =
            { X = x; Y = y}
        defaultRecordDict[defaultRecordPair] <- value

        let structRecordPair : StructPairRecord =
            { X = x; Y = y }
        structRecordDict[structRecordPair] <- value

    let randomTupleKeys =
        let keys = 
            randomKeysAndValues
            |> List.map fst
            |> Array.ofList
        [| for _ in 1 .. numberOfLookups do
            keys[rng.Next keys.Length]
        |]

    let randomStructTupleKeys =
        randomTupleKeys
        |> Array.map (fun (x, y) -> struct (x, y))

    let randomPairRecordKeys =
        randomTupleKeys
        |> Array.map (fun (x, y) -> { PairRecord.X = x; Y = y })

    let randomStructPairRecordKeys =
        randomTupleKeys
        |> Array.map (fun (x, y) -> { StructPairRecord.X = x; Y = y })


    [<Benchmark>]
    member _.DefaultTuple () =
        let mutable acc = 0.0

        for i = 0 to randomTupleKeys.Length - 1 do
            let nextKey = randomTupleKeys[i]
            acc <- acc + defaultTupleDict[nextKey]

        acc

    [<Benchmark>]
    member _.StructTuple () =
        let mutable acc = 0.0

        for i = 0 to randomStructTupleKeys.Length - 1 do
            let nextKey = randomStructTupleKeys[i]
            acc <- acc + structTupleDict[nextKey]

        acc

    [<Benchmark>]
    member _.PairRecord () =
        let mutable acc = 0.0

        for i = 0 to randomPairRecordKeys.Length - 1 do
            let nextKey = randomPairRecordKeys[i]
            acc <- acc + defaultRecordDict[nextKey]
        acc

    [<Benchmark>]
    member _.StructRecord () =
        let mutable acc = 0.0

        for i = 0 to randomStructPairRecordKeys.Length - 1 do
            let nextKey = randomStructPairRecordKeys[i]
            acc <- acc + structRecordDict[nextKey]

        acc


[<EntryPoint>]
let main _ =

    // I don't care about what Run returns so I'm ignoring it
    let _ = BenchmarkRunner.Run<Benchmarks>()
    0

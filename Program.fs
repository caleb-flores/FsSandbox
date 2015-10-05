// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open FsSandbox
open System.IO
open ProtoBuf
open System

module Serialization =
    let serialize person =
        use ms = new System.IO.MemoryStream()
        Serializer.SerializeWithLengthPrefix(ms, person, PrefixStyle.Fixed32)
        ms.ToArray()
    let deserialize<'T> data =
        use ms = new System.IO.MemoryStream(buffer = data)
        Serializer.DeserializeWithLengthPrefix<'T>(ms, PrefixStyle.Fixed32)

[<EntryPoint>]
let main argv = 
    let person:Person = { 
        Name = "FirstName"
        LastName  = "LastName"
        Age = 10 } 
    

    let p = person |> Serialization.serialize |> Serialization.deserialize<PersonV2>

    let pe = p |> Serialization.serialize |> Serialization.deserialize<Person>

    printfn "%s %s %d" pe.Name pe.LastName pe.Age 

    0 // return an integer exit code


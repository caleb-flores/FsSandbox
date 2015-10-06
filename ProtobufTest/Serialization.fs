

namespace ProtobufTest
open System
open ProtoBuf
open ProtoBuf.Meta
open System.Reflection
open Microsoft.FSharp.Reflection

[<RequireQualifiedAccessAttribute>]
module Serialization = 
    let serialize person =
        use ms = new System.IO.MemoryStream()
        Serializer.SerializeWithLengthPrefix(ms, person, PrefixStyle.Fixed32)
        ms.ToArray()

    let deserialize<'T> data =
        use ms = new System.IO.MemoryStream(buffer = data)
        Serializer.DeserializeWithLengthPrefix<'T>(ms, PrefixStyle.Fixed32)


    let registerSerializableDuInModel<'TMessage> (model:RuntimeTypeModel) =
        let baseType = model.[typeof<'TMessage>]
        for case in typeof<'TMessage> |> FSharpType.GetUnionCases do
            let caseType = case.Name |> case.DeclaringType.GetNestedType 
            baseType.AddSubType(1000 + case.Tag, caseType) |> ignore
            let caseTypeModel = model.[caseType]
            caseTypeModel.Add("item").UseConstructor <- false
        baseType.CompileInPlace()

    let registerSerializableDu<'TMessage> () = registerSerializableDuInModel<'TMessage> RuntimeTypeModel.Default


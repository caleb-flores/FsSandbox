

namespace ProtobufTest
open System
open ProtoBuf

[<RequireQualifiedAccessAttribute>]
module Serialization = 
    let serialize person =
        use ms = new System.IO.MemoryStream()
        Serializer.SerializeWithLengthPrefix(ms, person, PrefixStyle.Fixed32)
        ms.ToArray()

    let deserialize<'T> data =
        use ms = new System.IO.MemoryStream(buffer = data)
        Serializer.DeserializeWithLengthPrefix<'T>(ms, PrefixStyle.Fixed32)

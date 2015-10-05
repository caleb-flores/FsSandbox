namespace ProtobufTest
open System
open NUnit.Framework
open ProtoBuf
open System.IO

[<ProtoContract>]
type PersonV1' () = 
    let mutable firstName = ""
    let mutable lastName = ""
    [<ProtoMember(1)>]
    member __.FirstName 
        with  get() = firstName
        and set(v) = firstName <- v 

    [<ProtoMember(2)>]
    member __.LastName
        with get() = lastName
        and set(v) = lastName <- v

[<ProtoContract>]
type PersonV2' () =
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable age = 0
    [<ProtoMember(1)>]
    member __.FirstName 
        with  get() = firstName
        and set(v) = firstName <- v 

    [<ProtoMember(2)>]
    member __.LastName
        with get() = lastName
        and set(v) = lastName <- v

    [<ProtoMember(3)>]
    member __.Age
        with get() = age
        and set(v) = age <- v

[<TestFixture>]
type ``ProtoBuf test``() = 

    [<Test>]
    member x.``Backward compatibility``() =
        let fileName = "/tmp/person_v1.bin"
        let person = PersonV1'()
        person.FirstName <- "FirstName"
        person.LastName <- "LastName"
        using  (File.Create fileName) (fun stream -> Serializer.Serialize<PersonV1'>(stream, person))
        use st = File.OpenRead fileName
        let personB = Serializer.Deserialize<PersonV2'> st

        Assert.AreEqual ("FirstName",personB.FirstName)
        Assert.AreEqual ("LastName",personB.LastName)
        Assert.AreEqual (0,personB.Age)

    [<Test>]
    member x.``Forward compatibility``() =
        let fileName = "/tmp/person_v2.bin"
        let person = PersonV2'()
        person.FirstName <- "FirstName"
        person.LastName <- "LastName"
        person.Age <- 19
        using  (File.Create fileName) (fun stream -> Serializer.Serialize<PersonV2'>(stream, person))
        use st = File.OpenRead fileName
        let personB = Serializer.Deserialize<PersonV1'> st

        Assert.AreEqual ("FirstName",personB.FirstName)
        Assert.AreEqual ("LastName",personB.LastName)


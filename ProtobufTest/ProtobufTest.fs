namespace ProtobufTest.WithAttributes
open System
open NUnit.Framework
open ProtoBuf
open ProtobufTest

[<ProtoContract>]
[<CLIMutable>]
type Person = {
    [<ProtoMember(1)>]FirstName   : string
    [<ProtoMember(2)>]LastName    :string
}
[<ProtoContract>]
[<CLIMutable>]
type PersonV2 = {
    [<ProtoMember(1)>]FirstName   : string
    [<ProtoMember(2)>]LastName    : string
    [<ProtoMember(3)>]Age         : int
}
[<ProtoContract>]
[<CLIMutable>]
type PersonV3  = {
    [<ProtoMember(1)>]FirstName   : string
    [<ProtoMember(2)>]LastName    : string
    [<ProtoMember(4)>]Address     : string
}

[<TestFixture>]
type ``Basic use of Protobuf`` () = 

    [<Test>]
    member __.``Basic Serialization and Deserialization``() =
        let p:Person= {
            FirstName   = "FirstName"
            LastName    = "LastName"
        }
        let pCopy  = p |> Serialization.serialize |> Serialization.deserialize<Person>

        Assert.AreEqual(p,pCopy)       

    [<Test>]
    member __.``Backward compatibility``() =
        let p:Person= {
            FirstName   = "FirstName"
            LastName    = "LastName"
        }

        let pv2  = p |> Serialization.serialize |> Serialization.deserialize<PersonV2>
        Assert.AreEqual (p.FirstName, pv2.FirstName)
        Assert.AreEqual (p.LastName, pv2.LastName)
        Assert.AreEqual (0, pv2.Age)

    [<Test>]
    member __.``Forward compatibility``() =
        let p:PersonV2= {
            FirstName   = "FirstName"
            LastName    = "LastName"
            Age         = 17
        }

        let pv2  = p |> Serialization.serialize |> Serialization.deserialize<Person>
        Assert.AreEqual (p.FirstName, pv2.FirstName)
        Assert.AreEqual (p.LastName, pv2.LastName)

    [<Test>]
    member __.``Backward compatibility removing properties``() =
        let p:PersonV2= {
            FirstName   = "FirstName"
            LastName    = "LastName"
            Age         = 17
        }

        let pv3 = p |> Serialization.serialize |> Serialization.deserialize<PersonV3>
        Assert.AreEqual (p.FirstName, pv3.FirstName)
        Assert.AreEqual (p.LastName, pv3.LastName)
        Assert.AreEqual (None, pv3.Address)


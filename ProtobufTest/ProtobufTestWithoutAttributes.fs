namespace ProtobufTest
open System
open NUnit.Framework
open ProtoBuf
type Person = {
    FirstName   : string
    LastName    :string
}

type PersonV2 = {
    FirstName   : string
    LastName    : string
    Age         : int
}

type PersonV3  = {
    FirstName   : string
    LastName    : string
    Address     : string
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


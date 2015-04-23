module CommandLineParser.Tests

open NUnit.Framework

[<Test>]
let ``When I parse a command line with one argument then I should get a list with one token``() =
    let argv = [ "test" ]
    let expect = [ Argument "test" ]
    let actual = parseCommandLine argv
    Assert.AreEqual(expect, actual)

[<Test>]
let ``When I parse a command line with two arguments then I should get a list with two tokens``() =
    let argv = [ "test"; "this" ]
    let expect = [ Argument "test"; Argument "this" ]
    let actual = parseCommandLine argv 
    Assert.AreEqual(expect, actual)

[<Test>]
let ``When I parse a command line with a flag then I should get a list with one token``() =
    for (argv, expect) in [ (["-d"], [BoolFlag "d"]); (["/d"], [BoolFlag "d"])] do
        let actual = parseCommandLine argv
        Assert.AreEqual(expect, actual)

[<Test>]
let ``When I parse a command line with a flag and an argument then I should get a list with two tokens``() =
    let argv = ["test"; "-d"]
    let expect = [Argument "test"; BoolFlag "d"]
    let actual = parseCommandLine argv
    Assert.AreEqual(expect, actual)

[<Test>]
let ``When I parse a command line with a value flag then I should get a list with one token``() =
    for (argv, expect) in 
        [ (["-o"; "test.txt"], [ValueFlag ("o", "test.txt")]);
          (["-o=test.txt"], [ValueFlag ("o", "test.txt")]); 
          (["/o:test.txt"], [ValueFlag ("o", "test.txt")])] do 
        let actual = parseCommandLine argv
        Assert.AreEqual(expect, actual)

[<Test>]
let ``When I have a command line with one argument, and an options record with one command then the command is initialized``() =
    let argv = [ "query" ]

    let executeQueryCommand tokens =
        true
    let query = { name="query"; cmd = executeQueryCommand}

    let expect = { commands = [ query ]; options = []; arguments = [] }
    let actual = parse [ query ] [] argv

    Assert.AreEqual(expect, actual)

[<Test>]
let ``When I have a commandline specification with one command and one bool flag, and a command line with one argument then the command should be parsed``() =
    let argv = [ "query"; ]

    let executeQueryCommand tokens = 
        true

    let query = { name="query"; cmd = executeQueryCommand}
    
    let expect = { commands = [ query ]; options = [ ]; arguments = [] }
    let actual = parse [ query ] [ BoolFlag "v" ] argv 
    Assert.AreEqual(expect, actual)

[<Test>]
let ``When I have a commandline specificaiton with one command and no flags, and a command line with one command and a bool flag then the command should be parsed``() =
    let argv = [ "query"; "-v" ]

    let executeQueryCommand tokens =
        true 

    let query = { name="query"; cmd = executeQueryCommand}

    let expect = { commands = [ query ]; options = [ ]; arguments = [] }
    let actual = parse [ query ] [ ] argv 
    Assert.AreEqual(expect, actual)
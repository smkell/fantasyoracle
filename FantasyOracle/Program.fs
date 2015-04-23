open CommandLineParser 
open FantasyOracle

let executeQueryCommand commandline =
    if (List.length commandline.arguments) < 1 then
        printfn "ERROR: Not enough arguments for command 'query'"
        false
    else 
        let players = commandline.arguments |> List.map Baseball.queryPlayersByName |> List.concat
                
        Baseball.printPlayerTableHeader
        for player in players do
            Baseball.printPlayerTableRow player
        Baseball.printPlayerTableFooter players

        true

let query = { name = "query"; cmd = executeQueryCommand }

let commands = [ query ]
let options = [ ]

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    let commandline = parse commands options (Array.toList argv)
    
    for cmd in commandline.commands do 
        printfn "%b" (cmd.cmd commandline)
    0 // return an integer exit code

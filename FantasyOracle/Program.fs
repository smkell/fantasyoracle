/// Commands 
/// query [player name] - get statistics for the given player
/// team create [team name] [player name]* [categories]* - create a team with the given name, and players, outputs a file with [team name].json 
/// team list [team name] - list current season statistics for the given team
/// team query [team name] [player] - query a player's statistics in the context of the team (what has the player contributed)
/// lineup set [team name] - set the lineup 

open FSharp.Data

[<EntryPoint>]
let main argv = 
    let team = 
        [
            ("Yadier","Molina");
            ("Anthony","Rizzo");
            ("Ian","Kinsler");
            ("Nolan","Arenado");
            ("Starlin","Castro");
            ("Daniel","Murphy");
            ("Evan","Longoria");
            ("Yasiel","Puig");
            ("Corey","Dickerson");
            ("Matt","Holliday");
            ("Andrew","McCutchen");
            ("Kyle","Seager");
            ("Joey","Votto");
            ("Adam","LaRoche");
            ("Evan","Gattis");
            ("Eric","Hosmer");
        ]

    team
    |> List.map Baseball.fetchPlayerByName
    |> Common.caculateContributions ["R";"HR";"XBH";"TB";"RBI";"SB";"AVG";"OBP"]
    |> Baseball.printPlayersContribTable
    |> ignore

    0
    

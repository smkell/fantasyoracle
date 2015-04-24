module Baseball

open Common
open FSharp.Data

let constructMlbStatsUrl pageSize pageNum = 
    let baseUrl = "http://mlb.mlb.com/pubajax/wf/flow/stats.splayer?season=2015&sort_order=%27desc%27&sort_column=%27avg%27&stat_type=hitting&page_type=SortablePlayer&game_type=%27R%27&player_pool=QUALIFIER&season_type=ANY&sport_code=%27mlb%27&results=1000"

    baseUrl + "&recSP=" + (string pageNum) + "&recPP=" + (string pageSize)

type MlbStats = JsonProvider<"..\DataSamples\mlbstats.json">

let playerFromJson (json:MlbStats.Row) =
    let stats = 
        [
            ("R", float json.R);
            ("HR", float json.Hr);
            ("XBH", float json.Xbh);
            ("TB", float json.Tb);
            ("RBI", float json.Rbi);
            ("SB", float json.Sb);
            ("AVG", float json.Avg);
            ("OBP", float json.Obp);
        ]
    {
        FirstName = json.NameFirst;
        LastName = json.NameLast;
        Stats = stats;
    }

let fetchAllPlayerStats =
    let page = constructMlbStatsUrl 50 1
                |> MlbStats.Load
    
    let numPages = page.StatsSortablePlayer.QueryResults.TotalP

    // Output the copyright notice
    printfn "%s" page.StatsSortablePlayer.CopyRight

    seq {
        yield! page.StatsSortablePlayer.QueryResults.Row
                |> Seq.map playerFromJson

        for pageNum in 2..numPages do 
            let page = (constructMlbStatsUrl 50 pageNum) |> MlbStats.Load
            yield! page.StatsSortablePlayer.QueryResults.Row
                    |> Seq.map playerFromJson

    } |> Seq.cache

let fetchPlayerByName (firstname:string, lastname:string) =
    fetchAllPlayerStats 
    |> Seq.filter (fun p -> p.LastName.ToLower() = lastname.ToLower() && p.FirstName.ToLower() = firstname.ToLower()) 
    |> Seq.exactlyOne

let fetchPlayerByLastName (lastname:string) =
    fetchAllPlayerStats |> Seq.filter (fun p -> p.LastName.ToLower() = lastname.ToLower()) |> Seq.exactlyOne

let printHeaderRow() =
    printfn "%15s|%15s|%3s|%4s|%5s|%4s|%5s|%4s|%5s|%5s|" 
        "Last Name" 
        "First Name" 
        " R " 
        " HR " 
        " XBH " 
        " TB " 
        " RBI " 
        " SB " 
        " AVG " 
        " OBP "

    let dashes n = 
        String.concat "" (seq { for i in 1..n do yield "-" })

    printfn "%15s|%15s|%3s|%4s|%5s|%4s|%5s|%4s|%5s|%5s|" (dashes 15) (dashes 15) (dashes 3) (dashes 4) (dashes 5) (dashes 4) (dashes 5) (dashes 4) (dashes 5) (dashes 5)

let printPlayerRow (player:Player) =
    printfn "%15s|%15s|%3d|%4d|%5d|%4d|%5d|%4d|%5.3f|%5.3f|" 
        player.LastName 
        player.FirstName 
        (getStatIntValue "R" player)
        (getStatIntValue "HR" player)
        (getStatIntValue "XBH" player)
        (getStatIntValue "TB" player)
        (getStatIntValue "RBI" player)
        (getStatIntValue "SB" player)
        (getStatValue "AVG" player)
        (getStatValue "OBP" player)

let printPlayerContribHeader =
    printfn "%15s|%15s|%3s|%4s|%4s|%4s|%5s|%4s|"
        "Last Name" 
        "First Name" 
        " R " 
        " HR " 
        " XBH " 
        " TB " 
        " RBI " 
        " SB " 

    let dashes n = 
        String.concat "" (seq { for i in 1..n do yield "-" })

    printfn "%15s|%15s|%3s|%4s|%5s|%4s|%5s|%4s|" 
        (dashes 15) (dashes 15) (dashes 3) (dashes 4) (dashes 5) (dashes 4) (dashes 5) (dashes 4)

let printPlayerContribRow player =
    printfn "%15s|%15s|%3.1f%%|%4.1f|%5.1f|%4.1f|%5.1f|%4.1f|"
        player.LastName
        player.FirstName
        (getStatValue "R" player)
        (getStatValue "HR" player)
        (getStatValue "XBH" player)
        (getStatValue "TB" player)
        (getStatValue "RBI" player)
        (getStatValue "SB" player)

let printPlayersTable players =
    printHeaderRow |> ignore
    for player in players do 
        printPlayerRow player

let printPlayersContribTable players =
    printPlayerContribHeader |> ignore
    for player in players do 
        printPlayerContribRow player
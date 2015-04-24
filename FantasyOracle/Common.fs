module Common

type Stat = string * float 

let statName (name,_) = name
let statValue (_, value) = value

type Player = { FirstName:string; LastName:string; Stats: Stat list }

let getStat name player =
    List.find (fun s -> statName s = name) player.Stats

let getStatValue name player = (getStat name player) |> statValue
let getStatIntValue name player = (getStatValue name player) |> int

let totalCategory players name  =
    let total = players
                |> List.map (getStatValue name)
                |> List.sum

    (name, total)

let playerContribution totals player  =
    let stats = totals
                |> List.map (fun (name, value) -> (name, ((getStatValue name player) / value) * 100.))
    { player with Stats = stats }

let caculateContributions (categories:string list) players =
    // Sum up totals for each category 
    let totals = 
        categories
        |> List.map (totalCategory players)

    // Now determine the player's stat as a percentage of the total
    players |> Seq.map (playerContribution totals)



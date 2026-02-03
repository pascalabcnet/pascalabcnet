// 3.11 - AdjacentGroupBy
type
  Player = auto class
    Name: string; Score: integer;
  end;  

function GetPlayers := [
  new Player('Alice', 95),
  new Player('Bob', 95),
  new Player('Charlie', 110),
  new Player('Diana', 110),
  new Player('Karina', 120)
];

begin
  var leaderboard := GetPlayers;
  foreach var group in leaderboard.AdjacentGroupBy(p -> p.Score) do
  begin
    Println(group.Key);
    group.Println;
  end;
end.
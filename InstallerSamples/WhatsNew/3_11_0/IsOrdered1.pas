// 3.11 - IsOrdered
type
  Player = auto class
    Name: string; Points: integer;
  end;  

function GetPlayers := [
  new Player('Alice', 120),
  new Player('Bob', 95),
  new Player('Charlie', 130),
  new Player('Diana', 110)
];

begin
  var leaderboard := GetPlayers;
  Println(leaderboard);
  if not leaderboard.IsOrderedByDescending(p -> p.Points) then
    Println('Рейтинг составлен неверно!');

  leaderboard := leaderboard.OrderByDescending(p -> p.Points).ToArray;
  Println(leaderboard);
  if leaderboard.IsOrderedByDescending(p -> p.Points) then
    Println('Теперь рейтинг составлен верно!');
end.
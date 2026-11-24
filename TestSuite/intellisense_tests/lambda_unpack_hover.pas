begin
  var participants := [('Ivanov', Arr(100, 85, 90)), ('Petrov', Arr(75, 90, 80))];
  var winners := participants
    .OrderByDescending(\(name, points) -> points{@parameter points: array of integer;@}.Sum)
    .Take(3);
  var names := participants.Select(\(name, points) -> name{@parameter name: string;@}.Length);
  var nested := Seq(('Ivanov', (42, Arr(100, 90))));
  var totals := nested.Select(\(name, \(age, points)) -> points{@parameter points: array of integer;@}.Length);
end.

var a: array of integer := (1,3,5,7);

begin
  a.Skip(2).Concat(a.Take(2)).Print;
end.
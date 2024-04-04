begin
  // Можно распаковывать также тип System.ValueTuple
  var (n,s) := System.ValueTuple.Create(2,'ab');
  Print(n,s);
end. 
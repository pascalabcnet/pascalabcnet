//ä
begin
  var l := Seq(2,1,4);
  l.OfType&<integer>().ForEach{@(расширение) procedure ForEach<T>(Self: sequence of T; action: T->());@}(x->Print(x));
end.
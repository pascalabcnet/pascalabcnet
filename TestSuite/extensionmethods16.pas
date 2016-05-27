type TBase = class
end;
TDer = class(TBase)
end;
begin
  var l := Lst&<TBase>(new TBase, new TDer, new TDer);
  var num := 0;
  l.OfType&<TDer>.Foreach(x->Inc(num));
  assert(num = 2);
end.
begin
  var arr:=|12,23,34,45,56,67,78,89|;
  arr.Numerate.Nwise(3).where(\(\(a,b),\(c,d),\(e,f)) → a<b).print
end.
begin
  var l: string -> integer := t->t.Where(c->c='0').Count;
  Assert(l('abc0')=1);
end.
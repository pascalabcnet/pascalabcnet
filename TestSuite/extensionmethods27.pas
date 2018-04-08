procedure Add<T>(self:List<T>; item: T; b: boolean); extensionmethod;
begin
  self.Add(item);
end;

begin
  var l: List<integer> := new List<integer>;
  l.Add(1, true);
  l.Add(6);
  assert(l[0] = 1);
end.
procedure System.Collections.Generic.IEnumerable<TSource>.Show;
begin
  var qq := self.ElementAt(0).ToString();
  assert(qq = '2');
  {var obj := object(self.ElementAt(0));
  assert(obj.ToString()='2');
  assert(object(self.ElementAt(0)).ToString()='2');}
  var t: TSource;
  assert(t.ToString() = '0');
end;
begin
  var a := Arr(2,3,4);
  a.Show;
end.
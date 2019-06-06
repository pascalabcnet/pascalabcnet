
procedure Deconstruct<T>(self: List<T>; var f: T); extensionmethod; 
begin 
  if self.Count > 0 then f := self[0];
end;

procedure Deconstruct<T>(self: Dictionary<integer,List<T>>; var f: T); extensionmethod;
begin
  var fst := self.First;
  var lst := fst.Value;
  if not (lst = nil) and (lst.Count > 0) then
    f := lst[0];
end;

procedure Deconstruct<K,V>(self: Dictionary<K,V>; var kk: Dictionary<List<K>, V>; var vv: V); extensionmethod; 
begin 
  kk := Dict((self.Keys.ToList, self.Values.First));
end;

begin
  var pair := ('asd', Arr(1,2,3).ToList);
  var lst := Arr(4,5,6).ToList as object;
  var d := Dict(Arr(pair)) as object;
  var d2 := Dict((7, 8.0)) as object;
  var collection := Arr(lst, d, d2);
  foreach var x in collection do
    match x with
      Dictionary<integer,real>(s; k): Assert((s is Dictionary<List<integer>, real>) and (k is real));
      Dictionary<string,List<integer>>(first): Assert(first['asd'][0] = 1);
      List<real>(l: real): Assert(false);
      List<integer>(first: integer): Assert(first = 4);
      else Assert(false)
    end;
end.
procedure p1<T>(o: T);
where T: string;
begin
  o.Length.Println;
end;

begin
  var s := '123';
  var l := false ? s : s.Take(3);
  p1( false ? s : s.Take(3) ); 
end.

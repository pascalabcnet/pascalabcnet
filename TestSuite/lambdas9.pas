var l: array of byte;

procedure p1<T>(f: ()->T);
begin
  f();
end;

begin
  p1(()->
  begin
    Result := new byte[1];
    
    loop 1 do
    begin
      Result[0] := 1;
    end;
    l := Result;
  end);
  assert(l[0] = 1);
end.
procedure p1(p: ()->());
begin
  Assert(1=2);
end; 

procedure p1<T>(f: ()->T);
begin
  Assert(1=1);
end;  

begin
  p1(
    function->
    begin
      Result := 1;
    end
  )
end.
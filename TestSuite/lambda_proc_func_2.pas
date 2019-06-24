procedure p1(p: procedure);
begin
  Assert(1=2)
  //write('procedure')
end; 

procedure p1(f: ()->integer);
begin
  Assert(1=1)
  //write('function')
end;  

begin
  p1(
    function->
    begin
      Result := 1;
    end
  )
end.
var i: integer;

procedure Gen;
begin
  for var i := 10 downto 1 do
    writeln(i);
  for var i := 2 to 5 do
  begin
    if (i mod 2 = 0) then 
    begin
      writeln(i);
    end;
  end
end; 


begin
  Gen();
  
end.
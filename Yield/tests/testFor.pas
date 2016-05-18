var i: integer;

function Gen: sequence of integer;
begin
  for var i := 10 downto 1 do
    yield i;
  for var i := 2 to 5 do
  begin
    if (i mod 2 = 0) then 
    begin
      yield i;
    end;
  end
end; 

(* procedure Gen;
begin
  for var i := 10 downto 1 do
    writeln(i);
end; *)

begin
  //Gen();
  var q := Gen();
  foreach var e in q do
    writeln(e);
end.
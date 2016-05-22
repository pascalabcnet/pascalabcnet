var i: integer;

function Gen: sequence of integer;
begin
  for var i := 2 to 5 do
  begin
    if (i mod 2 = 0) then 
    begin
      yield i;
    end;
  end
end; 


begin
  var q := Gen();
  foreach var x in q do
    writeln(x);
end.
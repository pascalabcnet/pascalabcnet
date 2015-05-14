var i:integer;
begin
  for i:=1 to 10 do begin
    Writeln(i);
  	if i=5 then break;
  end;
  i:=0;
  while true do begin
    Writeln(i);
    i:=i+1;
  	if i=5 then break;
  end;
  i:=0;
  repeat
  	Writeln(i);
    i:=i+1;
  	if i=5 then break;
  until false;  
  readln;
end.
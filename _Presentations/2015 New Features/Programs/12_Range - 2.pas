function IsPrime(x: integer): boolean;
var i: integer;
begin
  for i:=2 to Round(sqrt(x)) do
    if x mod i = 0 then
    begin
      IsPrime := False;
      exit
    end;
  IsPrime := True;  
end;

var i: integer;

begin
  for i:=2 to 1000 do
    if IsPrime(i) then
      write(i,' ');
end.
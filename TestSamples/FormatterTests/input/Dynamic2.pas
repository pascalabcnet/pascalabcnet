uses PT4;

var tek,prev:Node;
    n:integer;
    
begin
  Task('Dynamic2');
  read(tek);
  while tek<>nil do begin
    n:=n+1;
    write(tek.Data);
    prev:=tek;
    tek:=tek.Next;
  end;
  write(n,prev);
end.


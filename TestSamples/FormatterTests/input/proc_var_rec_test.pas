type rec = record
       x:integer;
     
     end;
     

procedure Write1(var f:rec); 
begin
  writeln(string.Format('Write({0});', f));
end;

var r:rec;

begin
  //r:=new rec;
  Write1(r);
  readln;
  //readln;
end.
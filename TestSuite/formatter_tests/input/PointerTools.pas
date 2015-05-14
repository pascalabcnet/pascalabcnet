unit PointerTools;

procedure WriteMemoryToScreen(p:pointer; Count,InLineCount:integer);
var i: integer;    
    val: byte;
begin
  for i:=1 to Count do begin
    val:=PByte(p)^;
    Write(string.Format('{0} ',val));    
    p:=pointer(integer(p)+1);
    if i mod InLineCount = 0 then Writeln; 
  end;
end;

procedure WriteMemoryToScreen(p:pointer; Count:integer);
begin
  WriteMemoryToScreen(p,Count,4);
end;

end.
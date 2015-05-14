function Concat1(params strs : array of string) : string;
var i:integer;
    sb:System.Text.StringBuilder;
begin
  sb:=new System.Text.StringBuilder;
  for i:=0 to strs.length-1 do begin
    writeln(strs[i]);
    sb.Append(strs[i]);
  end;  
  concat1 := sb.ToString;
end;

begin

  writeln(concat1('1 ','2','3 '));
  readln;
end.
type
  rec = record
    i: integer;
    function ToString: string; override;
    begin
      result := 'rec!';
    end;
  end;
  
var
  r: rec;
  
begin
  writeln(r.ToString);
  readln;
end.
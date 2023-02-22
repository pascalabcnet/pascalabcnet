type
  r1 = record
    x := 5;
    function ToString: string; override := x.ToString;
  end;
  
begin
  var a := new r1;
  var s := (if true then a else new r1).ToString;
  assert(s = '5');
  s := '';
  s := (if false then a else new r1).ToString;
  assert(s = '5');
end.
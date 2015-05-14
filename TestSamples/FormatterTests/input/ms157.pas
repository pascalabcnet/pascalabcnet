type
  SpriteState = record
    Name: string;
    Beg: integer;
  end;
  
var a: array of SpriteState;

begin
  SetLength(a,1);
  a[0].Name := 'abc';
  a[0].Beg := 2;
  SetLength(a,2);
  writeln(a[0].Name,' ',a[0].Beg);
end.
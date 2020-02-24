var i: integer;
type
  r1 = record
    
    procedure p1<T>;
    begin
      i := 1;
    end;
    
  end;
  
begin
  var r: r1;
  r.p1&<byte>;
  assert(i = 1);
end.
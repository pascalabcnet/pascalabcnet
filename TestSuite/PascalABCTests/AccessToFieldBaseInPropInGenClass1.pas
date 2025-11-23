type
  A<T> = class
    fielda: integer := 1;
  end;
  
  B<T> = class(A<T>)
    property PropA: integer read fielda;
  end;

begin
  var b1 := B&<integer>.Create;
  Assert(b1.Propa = 1);
end.
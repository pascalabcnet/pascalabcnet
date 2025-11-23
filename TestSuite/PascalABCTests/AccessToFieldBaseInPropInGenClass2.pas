type
  A<T> = class
    fielda: integer := 1;
  end;
  
  B<T> = class(A<T>)
    property PropA: integer write fielda read fielda;
  end;

begin
  var b1 := B&<integer>.Create;
  b1.Propa := 2;
  Assert(b1.Propa = 2);
end.
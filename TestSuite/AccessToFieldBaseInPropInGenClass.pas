type
  A<T> = class
    fielda: T;
  end;
  
  B<T> = class(A<T>)
    property PropA: T read fielda;
  end;

begin
  var b1 := new B<integer>;
  Assert(b1.PropA = 0);
end.
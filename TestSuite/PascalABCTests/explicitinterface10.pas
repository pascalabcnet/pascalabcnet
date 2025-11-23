type
  I1<T> = interface
    property X: integer read;
  end;
  
  T1<T> = record(I1<T>)
    property X: integer read 1;
    property I1<T>.X: integer read 2;
  end;

begin
  var r: T1<integer>;
  assert(r.X = 1);
  var r1: I1<integer> := r;
  assert(r1.X = 2);
end.
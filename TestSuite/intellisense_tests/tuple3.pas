type
  r1<T1, T2>=record
    a: T1;
    b: T2;
  end;

function f1<T1, T2>(t: (T1, T2)): r1<T1, T2>;
begin
  (Result.a, Result.b) := t;
end;

begin
  var a{@var a: r1<real,string>;@} := f1((0.0, ''));
  var s{@var s: string;@} := a.b;
end.
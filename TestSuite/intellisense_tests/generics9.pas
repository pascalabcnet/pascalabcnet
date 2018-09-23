type
  t1 = class end;
  t2 = class(t1) end;

begin
  var a1: sequence of t2;
  var a2{@var a2: sequence of t1;@} := a1.Cast&<t1>;
  var a3{@var a3: sequence of List<t1>;@} := a1.Cast&<List<t1>>;
end.
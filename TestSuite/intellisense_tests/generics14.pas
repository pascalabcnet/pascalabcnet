procedure F() := exit;

begin
  var a{var a: (procedure,procedure);} := Rec(procedure() -> exit, procedure() -> exit); // (T1, T2)
  var b{var b: (procedure,procedure);} := Rec(F, F);
end.
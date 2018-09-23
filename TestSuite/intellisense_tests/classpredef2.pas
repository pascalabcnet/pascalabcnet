type
  t1 = class;
  t1 = abstract class end;
  
  t2 = class;
  t2 = sealed class end;

begin
  var a: t1{@abstract class t1@};
  var b: t2{@sealed class t2@};
end.
uses u_abstract2;

type
  t2 = abstract class(t1<t2>) end;
  t3 = class(t2)
  end;
  
begin
  new t3;
end.
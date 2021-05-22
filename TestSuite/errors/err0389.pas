type
  T1 = class public procedure p := Writeln('333'); end;
  
  T2 = class public procedure p := Writeln('444'); end;
  
  T = partial class(T1) end;
  
  T = partial class(T2) end;

begin
  T.Create.p;
end.
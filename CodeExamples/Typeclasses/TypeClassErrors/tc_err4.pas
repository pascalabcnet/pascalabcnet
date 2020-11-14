type 
  tc1[T] = typeclass end;
  
  I1=interface end;
  tc1[I1] = instance end;//не должно компилироваться

begin
end.

function Gen<T>: sequence of T;
begin
  yield default(T);
end;


begin
  Gen&<real>.Println;
end.
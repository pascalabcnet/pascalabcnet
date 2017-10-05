function f<T>(x: sequence of T): T;
begin
end;

begin
  f&<real>(Arr(1,2,3)).Round{@(расширение) function real.Round(): integer;@}();
end.
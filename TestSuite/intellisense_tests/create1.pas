type A = class
  f: sequence of integer;
end;

begin
  A.Create{@constructor A();@}.f{@var A.f: sequence of integer;@}.Println;
  A.Create().f{@var A.f: sequence of integer;@}.Println;
  StringBuilder.Create{@constructor StringBuilder();@}.ToString{@function StringBuilder.ToString(): string; virtual;@};
end.
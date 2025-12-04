type
  t3 = class end;
  t2 = class
    class function operator implicit(a: t2): t3 := new t3;
  end;
  t1 = class
    class function operator/(a: t1; b: t3): t1 := new t1;
  end;

begin
  var a := new t1;
  var b := new t2;
  var c{@var c: t1;@} := a / b;//тут у b неявно приводится тип
end.
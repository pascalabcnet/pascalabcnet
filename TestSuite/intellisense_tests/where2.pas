type
  I1 = interface
    procedure p1;
  end;
  I2 = interface
    procedure p2;
  end;


procedure p1<T>(a:T); where T: I1, I2;
begin
  a.p1{@procedure I1.p1();@};
  a.p2{@procedure I2.p2();@};
end;

function f1<T>(a:T); where T: I1, I2;
begin
  a.p1{@procedure I1.p1();@};
  a.p2{@procedure I2.p2();@};
end;

begin end.
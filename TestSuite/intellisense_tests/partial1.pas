type
  t1 = partial class
    procedure p1 := exit;
  end;
  
  t1 = partial class
    procedure p2 := exit;
  end;
  
begin
  var a := new t1;
  a.p1{@procedure t1.p1();@};
  a.p2{@procedure t1.p2();@};
end.
type
  t1 = class
    event ev1: ()->();
  end;

begin 
  var o := new t1;
  o.ev1();
end.
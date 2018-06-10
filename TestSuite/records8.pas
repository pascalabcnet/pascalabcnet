type
  
  t1=record
    public x,y:integer;
    function p1 := (x, y);
  end;
  
  t2=class
    _p1:t1;
    property p1:t1 read _p1;
    function GetP1 := _p1;//не обязательно короткая функция
  end;

begin
  
  var a:=new t2;
  a._p1 := new t1;
  a._p1.x := 3;
  a._p1.y := 5;
  
  var t := a.p1.p1;
  assert(t.Item1 = 3);
  assert(t.Item2 = 5);
  
end.
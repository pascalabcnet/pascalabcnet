type BallInfo = class
  x: integer;
end;    

begin
  var a : array of BallInfo;
  a := new BallInfo[2](new BallInfo, new BallInfo);
  var s := 0;
  a[0].x := 1;
  a[1].x := 2;
  a.ForEach(x->begin s += x.x end);
  assert(s = 3);
end.
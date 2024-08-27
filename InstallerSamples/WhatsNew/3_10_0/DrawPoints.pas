uses Turtle;

begin
  Window.Title := 'DrawPoints';
  var n := 1000;
  var d := 10;
  var a := ArrRandomReal(n,-d,d);
  DrawPoints(a[::2],a[1::2]);
  a := ArrRandomReal(n,-d,d);
  DrawPoints(a[::2],a[1::2]);
end.
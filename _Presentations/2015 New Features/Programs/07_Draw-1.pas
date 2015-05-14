uses GraphABC;

begin
  Window.Title := 'Графики функций';
  Draw(sin);

  var r := Rect(0,0,200,100);

  r.Offset(120,10);
  Draw(sqr, r);

  r.Offset(0,110);
  Draw(abs,-5,5,r);

  r.Offset(200,200);
  Draw(exp,0,5,r);
end.
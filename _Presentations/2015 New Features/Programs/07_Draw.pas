uses GraphABC;

begin
  Window.Title := 'Графики функций';
  Draw(sin);
  Draw(-5,5,320,250,520,350,abs);
  Draw(0,5,320,370,520,470,exp);
  
  var r := Rect(120,10,320,110);
  Draw(r, sqr);
  
  r.Y := r.Y + 120;
  Draw(r, x -> x*cos(3*x));
end.
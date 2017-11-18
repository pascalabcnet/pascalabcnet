uses Graph3D;

begin
  Window.Title := 'a+b+c - последовательное выполнение, a*b - параллельное выполнение';
  var s := Box(0,0,0,3,1,2,Colors.Blue);
  var p1 := P3D(5,0,0);
  var p2 := P3D(5,5,0);
  var OrtZ := V3D(0,0,1);
  var an := s.MoveToAnim(p1,1.Sec)*s.ScaleAnim(1.5,1.Sec) + 
            s.MoveToAnim(p2,1.Sec)*s.ScaleAnim(1,1.Sec) + 
            s.RotateAnim(OrtZ,90,1.Sec);
  an.Begin;
end.
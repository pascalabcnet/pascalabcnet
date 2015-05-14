uses GraphABC;

var pp: Picture;

begin
  CenterWindow;
  Brush.Color := clLightBlue;
  Rectangle(10,10,WindowWidth - 10,WindowHeight - 10);
  pp := new Picture('1.bmp');
  pp.Draw(40,20);
  pp.Transparent := True;
  pp.Draw(190,20);
  pp.TransparentColor := clWhite;
  pp.Draw(340,20);
  pp.TransparentColor := pp.GetPixel(70,70);
  pp.Draw(490,20);
  pp.Transparent := False;
  pp.Draw(40,250);
  pp.TransparentColor := clBlack;
  pp.Draw(190,250);
  pp.Transparent := True;
  pp.Draw(340,250);
end.
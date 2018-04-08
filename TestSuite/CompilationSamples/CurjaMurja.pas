uses GraphWPF;

begin
  Window.Title := 'Всякая Курья Мурья';
  Pen.Width := 0.5;
  Brush.Color := RGB(128,200,100);
  Ellipse(100,100,30,20);
  Brush.Color := RandomColor;
  Circle(170,100,20);
  Brush.Color := RandomColor;
  Rectangle(220,80,70,50);
  Line(220,80,220+70,80+50);
  //DrawImage(200,140,'cofe.jpg');
  Brush.Color := RGB(200,200,255);
  Polygon(Arr(Pnt(20,20),Pnt(20,120),Pnt(120,20)));
  Brush.Color := Colors.Black;
  for var i:=0 to 400 do
    Rectangle(1+2*i,2,0,0);
  Font.Size := 30;
  Font.Color := Colors.Red;
  TextOut(0,0,'Hello');  
  Font.Size := 40;
  Font.Color := Colors.Blue;
  Font.Name := 'Times New Roman';
  Font.Style := FontStyle.BoldItalic;
  TextOut(200,0,'Привет'); 
  Sleep(1000);
  Window.Save('1.png');
  Window.Title := 'Сохранили';
  Sleep(1000);
  Window.Clear;
  Window.Title := 'Очистили';
  Sleep(1000);
  Window.Load('1.png');
  Window.Title := 'Загрузили';
end.
// Графика. Сохраниение изображения графического окна в файл и загрузка из файла 
uses GraphABC;

const 
  delay = 500;
  filename = 'GraphWindow.bmp';

begin
  Window.Title := 'Window.Save и Window.Load';
  SetWindowSize(320,240);
  
  for var x:=0 to Window.Width - 1 do
  for var y:=0 to Window.Height - 1 do
    SetPixel(x,y,RGB(x-2*y,y+x,y-2*x));
  
  Window.Save(filename);
  for var i := 1 to 5 do
  begin
    Window.Clear;
    Sleep(delay);
    Window.Load(filename);
    Sleep(delay);
  end;
end.
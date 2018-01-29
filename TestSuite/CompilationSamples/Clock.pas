uses GraphWPF;

begin
  Window.Title := 'Цифровые часы';
  Font.Size := 180;
  while True do
  begin
    DrawText(Window.ClientRect,System.DateTime.Now.ToLongTimeString,Colors.Red);
    Sleep(1000);
    Window.Clear;
  end;
end.
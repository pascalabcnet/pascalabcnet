uses GraphWPF;

begin
  Window.Title := 'Использование FontOptions';
  TextOut(10,10,'Обычный');
  TextOut(10,40,'Жирный',Font.WithStyle(FontStyle.Bold));
  TextOut(10,70,'20 пунктов',Font.WithSize(20));
  TextOut(10,110,'Наклонный синий',Font.WithColor(Colors.Blue).WithStyle((FontStyle.Italic)));
end.
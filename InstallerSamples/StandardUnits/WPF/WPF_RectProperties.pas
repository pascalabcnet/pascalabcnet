uses WPF;

begin
  MainWindow.Title := 'Rectangle свойства';
  MainWindow.FontSize := 14;
  MainWindow.WindowStartupLocation := WindowStartupLocation.CenterScreen;
  var mainpanel := CreateDockPanel(Margin := 5).AsMainContent;
  var leftpanel := CreateStackPanel(Margin := 0, Width := 150)
    .With(Background := Brushes.Bisque).AddTo(mainpanel,Dock.Left);
  var rect := CreateRectangle(Width := 300, Height := 200).With(HA := HA.Center, VA := VA.Center).AddTo(mainpanel);
  rect.Fill := Brushes.Blue;
  var widthslider := CreateSlider(50,500,10,50);
  var tbwidth := CreateTextBlock();
  var heightslider := CreateSlider(50,500,10,50);
  var tbHeight := CreateTextBlock();
  var radiusslider := CreateSlider(0,50,10,50);
  var tbRadius := CreateTextBlock();
  
  leftpanel.AddElements(widthslider, tbwidth, heightslider, tbHeight, radiusslider, tbRadius);

  widthslider.ValueChanged += procedure(o,e) -> tbwidth.Text := 'Ширина: ' + widthslider.Value.ToString(0);
  heightslider.ValueChanged += procedure(o,e) -> tbheight.Text := 'Высота: ' + heightslider.Value.ToString(0);
  radiusslider.ValueChanged += procedure(o,e) -> begin
    tbRadius.Text := 'Радиус скругления: ' + radiusslider.Value.ToString(0);
    rect.RadiusX := radiusslider.Value;
    rect.RadiusY := radiusslider.Value;
  end; 
  radiusslider.Value := 10;

  widthslider.SetBinding(Slider.ValueProperty, rect, 'Width');
  heightslider.SetBinding(Slider.ValueProperty, rect, 'Height');
end.
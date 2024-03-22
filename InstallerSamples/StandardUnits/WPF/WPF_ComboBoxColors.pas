uses WPF;

begin
  var colorNames := typeof(Colors).GetProperties
    .ToDictionary(pi -> pi.Name, pi -> GBrush(Color(pi.GetValue(nil,nil))));
  MainWindow.Title := 'Standard Colors';
  MainWindow.WindowStartupLocation := WindowStartupLocation.CenterScreen;
  var mainpanel := CreateStackPanel(Margin := 10, Horizontal := True).AsMainContent;
  var panel1 := CreateStackPanel(Width := 250).AddTo(mainpanel);
  var cb := CreateComboBox.AddTo(panel1);
  cb.ItemsSource := colorNames.Keys;
  var rect := CreateRectangle(Height := 200, Margin := |0,10|).AddTo(panel1);
  rect.Fill := Brushes.Blue;
  
  cb.SelectionChanged += procedure(o,e) -> (rect.Fill := colorNames[cb.SelectedValue as string]);
  
  MainWindow.SizeToContent := SizeToContent.WidthAndHeight;
end.
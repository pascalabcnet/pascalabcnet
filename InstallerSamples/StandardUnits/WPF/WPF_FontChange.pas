uses WPF;

begin
  MainWindow.Title := 'Свойства шрифта';
  var dpanel := CreateDockPanel(Margin := 10).AsMainContent;
  var toppanel := Panels.StackPanel(Horizontal := True, Margin := 0);
  var TextLabel := CreateLabel('PascalABC.NET').With(HA := HA.Center, VA := VA.Center);
  TextLabel.FontSize := 100;
  dpanel.Add(toppanel,Dock.Top);
  dpanel.Add(TextLabel);
  
  CreateLabel('Размер шрифта', Margin := 0).AddTo(toppanel);
  var fontsizeslider := CreateSlider(20,100,10,10,Width := 100, Value := 100).AddTo(toppanel);
  fontsizeslider.ValueChanged += procedure(o,e) -> TextLabel.FontSize := fontsizeslider.Value;
  
  CreateLabel('Имя шрифта',Margin := |20,0|).AddTo(toppanel);
  var cb := CreateComboBox(|'Segoe UI','Arial','Times New Roman','Courier New'|).AddTo(toppanel);
  cb.SelectionChanged += procedure(o,e) -> TextLabel.FontFamily := cb.SelectedValue.ToString;

  var chb := CreateCheckBox('Наклонный',Margin := |20,5,0,0|).AddTo(toppanel);
  chb.Click += procedure(o,e) -> (TextLabel.FontStyle := 
    chb.IsChecked.Value ? FontStyles.Italic : FontStyles.Normal);
  var chb2 := CreateCheckBox('Жирный',Margin := |20,5,0,0|).AddTo(toppanel);
  chb2.Click += procedure(o,e) -> (TextLabel.FontWeight := 
    chb2.IsChecked.Value ? FontWeights.Bold : FontWeights.Normal);
end.
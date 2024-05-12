uses WPF;

begin
  MainWindow.Title := 'Калькулятор';
  var mainpanel := Panels.StackPanel.AsMainContent;
  var panel1 := Panels.StackPanel(Margin := 10, Horizontal := True)
    .With(Background := Brushes.LightBlue)
    .AddTo(mainpanel);
  
  var tb := CreateTextBox('0', Width := 120);
  var lb := CreateLabel('+',Width := 35);
  var tb1 := CreateTextBox('0', Width := 120);
  var lb1 := CreateLabel('=',Width := 55);
  panel1.AddElements(tb,lb,tb1,lb1);
  
  var panel2 := Panels.StackPanel(Margin := |10,0,10,10|, Horizontal := True)
    .With(Background := Brushes.LightBlue)
    .AddTo(mainpanel);
  panel2.HorizontalAlignment := HA.Right;

  var Handler: EventHandler := (o,e) -> begin
    var b := o as Button;
    var Left := tb.Text.ToReal;
    var Right := tb1.Text.ToReal;
    case b.Text of
      $'+': lb1.Content := '= ' + (Left + Right);
      $'-': lb1.Content := '= ' + (Left - Right);
      $'*': lb1.Content := '= ' + (Left * Right);
      $'/': lb1.Content := '= ' + (Left / Right);
    end;
  end;
  
  var bb := CreateButtons(|$'+',$'-',$'*',$'/'|); 
  panel2.AddButtons(bb, Width := 35, Margin := |10,0|, Padding := 5);

  for var i:=0 to bb.Length-1 do
    bb[i].Click += Handler;
  
  MainWindow.SizeToContent := SizeToContent.WidthAndHeight;
end.
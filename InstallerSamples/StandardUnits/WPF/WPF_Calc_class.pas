uses WPF;

type Main = class
  //-- Controls
  mainpanel,panel1,panel2: StackPanel;
  tb,tb1: TextBox;
  lb,lb1: TLabel;
  bb: array of Button;
  //-- Event Handlers
  procedure ButtonClick(o: Object; e: RoutedEventArgs);
  begin
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
  //-- InitControls
  procedure InitControls;
  begin
    MainWindow.Title := 'Калькулятор';
    mainpanel := Panels.StackPanel.AsMainContent;
    panel1 := Panels.StackPanel(Margin := 10, Horizontal := True)
      .With(Background := Brushes.LightBlue)
      .AddTo(mainpanel);
    
    tb := CreateTextBox('0', Width := 120);
    lb := CreateLabel('+',Width := 35);
    tb1 := CreateTextBox('0', Width := 120);
    lb1 := CreateLabel('=',Width := 55);
    panel1.AddElements(tb,lb,tb1,lb1);
    
    panel2 := Panels.StackPanel(Margin := |10,0,10,10|, Horizontal := True)
      .With(Background := Brushes.LightBlue)
      .AddTo(mainpanel);
    panel2.HorizontalAlignment := HA.Right;
    
    bb := CreateButtons(|$'+',$'-',$'*',$'/'|); 
    panel2.AddButtons(bb, Width := 35, Margin := |10,0|, Padding := 5);

    for var i:=0 to bb.Length-1 do
      bb[i].Click += ButtonClick;
    
    MainWindow.SizeToContent := SizeToContent.WidthAndHeight;
  end;
end;

begin
  Main.Create.InitControls; 
end.
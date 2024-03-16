uses WPF;

function MixColor(r,g,b: boolean): Color;
begin
  var ri := integer(r);
  var gi := integer(g);
  var bi := integer(b);
  Result := RGB(ri*255,gi*255,bi*255);
end;

type Main = class
  mainpanel: StackPanel;
  cbr,cbg,cbb: CheckBox;
  constructor;
  begin
    mainpanel := Panels.StackPanel(Margin := 10, Horizontal := True)
      .With(Background := Brushes.LightBlue).AsMainContent;
    cbr := CreateCheckBox('Красный', Margin := 5);
    cbg := CreateCheckBox('Зеленый', Margin := 5, IsChecked := True);
    cbb := CreateCheckBox('Синий', Margin := 5);
    mainpanel.AddElements(cbr,cbg,cbb);
    cbr.Click += Handler;
    cbg.Click += Handler;
    cbb.Click += Handler;
    Handler(nil,nil);
  end;
  procedure Handler(o: object; e: RoutedEventArgs);
  begin
    var (r,g,b) := (cbr.IsChecked,cbg.IsChecked,cbb.IsChecked);
    mainpanel.Background := GBrush(MixColor(r.Value,g.Value,b.Value));
  end;
end;

begin
  Main.Create;
end.
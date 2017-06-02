uses FormsABC;

type Fun = function (x: real): real;
  
var funs: array of Fun := (sin,cos,sqr);

function CalcIntegral(a,b: real; N: integer; f: Fun): real;
begin
  Result := 0;
  var x := a;
  var h := (b-a)/N;
  for var i:=0 to N-1 do
  begin
    Result += f(x);
    x += h;
  end;
  Result *= h;
end;

var 
  a := new RealField('a:');
  f1 := new FlowBreak;
  b := new RealField('b:');
  f2 := new FlowBreak;
  N := new IntegerField('N:');
  f3 := new FlowBreak;
  tl := new TextLabel('Функция: ');
  f4 := new FlowBreak;
  cb := new ComboBox;
  f5 := new FlowBreak(50);
  s1 := new Space(20);
  ok := new Button('Вычислить');
  tb: TextBox;

procedure MyClick;
begin
  var f := funs[cb.SelectedIndex];
  var res := CalcIntegral(a.Value,b.Value,N.Value,f);
  tb.AddLine(Format('Интеграл({0},{1},{2},{3}) = {4}',a.Value,b.Value,N.Value,cb.SelectedValue,res.ToString));
end;

procedure InitControls;
begin
  MainForm.Title := 'Вычисление определенного интеграла';
  MainForm.SetSize(500,350);
  MainForm.CenterOnScreen;
  b.Value := 1;
  N.Value := 10;
  cb.Items.Add('sin');
  cb.Items.Add('cos');
  cb.Items.Add('x^2');
  cb.SelectedIndex := 0;
  ok.Click += MyClick;
  mainPanel.Dock := DockStyle.Left;
  mainPanel.Width := 150;

  ParentControl := MainForm;
  tb := new TextBox;
  tb.Dock := DockStyle.Fill;
end;

begin
  InitControls;
end.
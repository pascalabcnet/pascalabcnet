uses 
  GraphWPF,
  WPFObjects,
  System.Reflection;

const lim = 127 + 32;

var 
  CurrentBackColor := Colors.White;
  InfoString: RectangleWPF;

procedure ChangeInfoStringText(colorName: string);
begin
  var c := Colors.LightGray;//Color.FromName(colorName);
  InfoString.Text := $'Colors.{colorName}: (R: {c.R}  G: {c.G}  B: {c.B})';
  InfoString.Color := c;
  if c.R*c.R+c.G*c.G+c.B*c.B <= 3*lim*lim then 
    InfoString.FontColor := Colors.White
  else InfoString.FontColor := Colors.Black;
  if c.A = 0 then 
    InfoString.FontColor := Colors.Gray;
end;

procedure CreateStandardColors(backColor: Color);
begin
  Window.Clear(backColor);
  var t := typeof(Colors);
  var mi := t.GetProperties();
  var y := 10.0;
  var x := 10.0;
  var h := Window.Width / 5 - 11;
  foreach m: PropertyInfo in mi do
  begin
    if m.GetGetMethod(true).IsStatic then
    begin
      var c := Color(m.GetValue(nil,nil)); 
      var r := new RectangleWPF(x,y,h,22,c);
      r.Text := m.Name;
      r.FontSize := 16;//.TextScale := 0.9;
      //r.Bordered := False;
      if c.R*c.R+c.G*c.G+c.B*c.B <= 3*lim*lim then 
        r.FontColor := Colors.White
      else r.FontColor := Colors.Black;
      if c.A = 0 then 
        r.FontColor := Colors.Gray;
      y += 25;
      if y > Window.Height-40 then
      begin
        y := 10;
        x += h + 10;
      end;  
    end;
  end;
end;

var 
  ob: ObjectWPF;
  dx,dy: real;

procedure MouseDown(x,y: real; mb: integer);
begin
  ob := ObjectUnderPoint(x,y);
  if ob=InfoString then
    ob := nil;
  if ob<>nil then
  begin
    ob.ToFront;
    dx := x - ob.Left;
    dy := y - ob.Top;
    var c := ob.Color;
    if (mb=2) then 
      ob.ScaleFactor := 2
    else ob.Color := Color.FromArgb(128,c.R,c.G,c.B);
  end;  
end;

procedure MouseUp(x,y: real; mb: integer);
begin
  if ob<>nil then
  begin
    if (mb=2) then 
      ob.Scale(0.5)
    else
    begin
      var c := ob.Color;
      if ob.Text = 'Transparent' then
        ob.Color := Color.FromArgb(0,c.R,c.G,c.B)
      else ob.Color := Color.FromArgb(255,c.R,c.G,c.B);
    end;  
    ob:= nil;
  end;
end;

procedure MouseMove(x,y: real; mb: integer);
begin
  if (ob<>nil) and (mb=1) then
  begin
    ob.ToFront;
    ob.Left := x - dx;
    ob.Top := y - dy;
  end;
  if ob=nil then
  begin
    InfoString.Text := '';
    InfoString.Color := CurrentBackColor;
  end;
  var ob1 := ObjectUnderPoint(x,y);
  if ob1=InfoString then
    ob1 := nil;
  if (ob1<>nil) and (mb=0) then
    ChangeInfoStringText(ob1.Text);
end;

procedure KeyDown(k: Key);
begin
  if k=Key.Space then
  begin
    if CurrentBackColor=Colors.White then 
    begin
      CurrentBackColor := Colors.Black;
      InfoString.BorderColor := Colors.White;
    end  
    else 
    begin
      CurrentBackColor := Colors.White;
      InfoString.BorderColor := Colors.Black;
    end;  
    Window.Clear(CurrentBackColor);
    //RedrawObjects;
  end;  
end;

begin
  Window.SetSize(1024,768);
  //Window.IsFixedSize := True;
  Window.CenterOnScreen;
  Window.Title := 'Стандартные цвета (нажмите Пробел для изменения фонового цвета)';
  CreateStandardColors(CurrentBackColor);
  InfoString := new RectangleWPF(10,Window.Height-30,Window.Width-20,25,Colors.LightGray);
  OnMouseMove := MouseMove;
  OnMouseDown := MouseDown;
  OnMouseUp := MouseUp;
  OnKeyDown := KeyDown;
end.

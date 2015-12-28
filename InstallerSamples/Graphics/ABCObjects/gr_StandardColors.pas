uses 
  GraphABC,
  ABCObjects,
  System.Reflection;

const lim = 127 + 32;

var 
  CurrentBackColor := clWhite;
  InfoString: RectangleABC;

procedure ChangeInfoStringText(colorName: string);
begin
  var c := Color.FromName(colorName);
  InfoString.Text := string.Format('Color.{0}: (R: {1}  G: {2}  B: {3})   (H: {4:f1}  S: {5:f1}  B: {6:f1})',colorName,c.R,c.G,c.B,c.GetHue,c.GetSaturation,c.GetBrightness);
  InfoString.Color := c;
  if c.R*c.R+c.G*c.G+c.B*c.B <= 3*lim*lim then 
    InfoString.FontColor := clWhite
  else InfoString.FontColor := clBlack;
  if c.A = 0 then 
   InfoString.FontColor := clGray;
end;

procedure CreateStandardColors(backColor: Color);
begin
  ClearWindow(backColor);
  var t := typeof(Color);
  var mi := t.GetProperties();
  var y := 10;
  var x := 10;
  var h := Window.Width div 5 - 11;
  foreach m: PropertyInfo in mi do
  begin
    if m.GetGetMethod(true).IsStatic then
    begin
      var c := Color(m.GetValue(nil,nil)); 
      var r := new RectangleABC(x,y,h,22,c);
      r.Text := m.Name;
      r.TextScale := 0.9;
      r.Bordered := False;
      if c.R*c.R+c.G*c.G+c.B*c.B <= 3*lim*lim then 
        r.FontColor := clWhite
      else r.FontColor := clBlack;
      if c.A = 0 then 
        r.FontColor := clGray;
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
  ob: ObjectABC;
  dx,dy: integer;

procedure MouseDown(x,y,mb: integer);
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
      ob.Scale(2)
    else ob.Color := Color.FromArgb(128,c.R,c.G,c.B);
  end;  
end;

procedure MouseUp(x,y,mb: integer);
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

procedure MouseMove(x,y,mb: integer);
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

procedure KeyDown(Key: integer);
begin
  if Key=vk_Space then
  begin
    if CurrentBackColor=clWhite then 
    begin
      CurrentBackColor := clBlack;
      InfoString.BorderColor := clWhite;
    end  
    else 
    begin
      CurrentBackColor := clWhite;
      InfoString.BorderColor := clBlack;
    end;  
    ClearWindow(CurrentBackColor);
    RedrawObjects;
  end;  
end;

begin
  SetWindowSize(1024,768);
  Window.IsFixedSize := True;
  Window.CenterOnScreen;
  Window.Title := 'Стандартные цвета (нажмите Пробел для изменения фонового цвета)';
  CreateStandardColors(CurrentBackColor);
  InfoString := new RectangleABC(10,Window.Height-30,Window.Width-20,25);
  OnMouseMove := MouseMove;
  OnMouseDown := MouseDown;
  OnMouseUp := MouseUp;
  OnKeyDown := KeyDown;
end.

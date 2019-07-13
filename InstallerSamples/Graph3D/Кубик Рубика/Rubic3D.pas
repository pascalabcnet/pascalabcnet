uses Graph3D;

var InnerColor := RGB(70,70,70);
var sec := 0.5;

var g := new Object3D[3,3,3];

procedure CreateCube;
begin
  var a := 1.03;

  for var x := 0 to 2 do
  for var y := 0 to 2 do
  for var z := 0 to 2 do
    if (x,y,z) <> (1,1,1) then
    begin
      var b := Box(0+x*a-a,0+y*a-a,-0.5+z*a-a,1,1,0.01,z=0 ? Colors.White : InnerColor);
      var b3 := Box(0+x*a-a,0+y*a-a,0.5+z*a-a,1,1,0.01,z=2 ? Colors.Yellow : InnerColor);
      
      var b1 := Box(0.5+x*a-a,0+y*a-a,0+z*a-a,0.01,1,1,x=2 ? Colors.Red : InnerColor);
      var b4 := Box(-0.5+x*a-a,0+y*a-a,0+z*a-a,0.01,1,1,x=0 ? Colors.Orange : InnerColor);
    
      var b2 := Box(0+x*a-a,0.5+y*a-a,0+z*a-a,1,0.01,1,y=2 ? Colors.Green : InnerColor);
      var b5 := Box(0+x*a-a,-0.5+y*a-a,0+z*a-a,1,0.01,1,y=0 ? Colors.Blue : InnerColor);
    
      g[x,y,z] := Group(b,b1,b2,b3,b4,b5);
    end;  
end;

var IsAnimated := False;
var CountCompleted := 0;  

procedure ShiftLeft<T>(var a,b,c,d: T);
begin
  var v := a;
  a := b;
  b := c;
  c := d;
  d := v;
end;

procedure ShiftRight<T>(var a,b,c,d: T);
begin
  var v := d;
  d := c;
  c := b;
  b := a;
  a := v;
end;

procedure EndAnim;
begin
  CountCompleted += 1;
  if CountCompleted = 9 then
  begin  
    IsAnimated := False;
    CountCompleted := 0;
  end;
end;

procedure RightRotate(n: integer := 1);
begin
  IsAnimated := True;
  var y := 2;
  if n = 0 then exit;
  for var x := 0 to 2 do
  for var z := 0 to 2 do
    g[x,y,z].AnimRotateAtAbsolute(OrtY,-90*n,Origin,sec*Abs(n),EndAnim).Begin;
  
  if n>0 then
    loop Abs(n) do
    begin  
      ShiftLeft(g[1,y,0],g[0,y,1],g[1,y,2],g[2,y,1]);
      ShiftLeft(g[2,y,0],g[0,y,0],g[0,y,2],g[2,y,2]);
    end
  else   
    loop Abs(n) do
    begin  
      ShiftRight(g[1,y,0],g[0,y,1],g[1,y,2],g[2,y,1]);
      ShiftRight(g[2,y,0],g[0,y,0],g[0,y,2],g[2,y,2]);
    end
end;

procedure LeftRotate(n: integer := 1);
begin
  IsAnimated := True;
  var y := 0;
  if n = 0 then exit;
  for var x := 0 to 2 do
  for var z := 0 to 2 do
    g[x,y,z].AnimRotateAtAbsolute(OrtY,90*n,Origin,sec*Abs(n),EndAnim).Begin;

  if n<0 then
    loop Abs(n) do
    begin  
      ShiftLeft(g[1,y,0],g[0,y,1],g[1,y,2],g[2,y,1]);
      ShiftLeft(g[2,y,0],g[0,y,0],g[0,y,2],g[2,y,2]);
    end
  else   
    loop Abs(n) do
    begin  
      ShiftRight(g[1,y,0],g[0,y,1],g[1,y,2],g[2,y,1]);
      ShiftRight(g[2,y,0],g[0,y,0],g[0,y,2],g[2,y,2]);
    end
end;

procedure UpRotate(n: integer := 1);
begin
  IsAnimated := True;
  var z := 2;
  if n = 0 then exit;
  for var x := 0 to 2 do
  for var y := 0 to 2 do
    g[x,y,z].AnimRotateAtAbsolute(OrtZ,-90*n,Origin,sec*Abs(n),EndAnim).Begin;

  if n<0 then
    loop Abs(n) do
    begin  
      ShiftLeft(g[1,0,z],g[0,1,z],g[1,2,z],g[2,1,z]);
      ShiftLeft(g[2,0,z],g[0,0,z],g[0,2,z],g[2,2,z]);
    end
  else   
    loop Abs(n) do
    begin  
      ShiftRight(g[1,0,z],g[0,1,z],g[1,2,z],g[2,1,z]);
      ShiftRight(g[2,0,z],g[0,0,z],g[0,2,z],g[2,2,z]);
    end
end;

procedure DownRotate(n: integer := 1);
begin
  IsAnimated := True;
  var z := 0;
  if n = 0 then exit;
  for var x := 0 to 2 do
  for var y := 0 to 2 do
    g[x,y,z].AnimRotateAtAbsolute(OrtZ,90*n,Origin,sec*Abs(n),EndAnim).Begin;

  if n>0 then
    loop Abs(n) do
    begin  
      ShiftLeft(g[1,0,z],g[0,1,z],g[1,2,z],g[2,1,z]);
      ShiftLeft(g[2,0,z],g[0,0,z],g[0,2,z],g[2,2,z]);
    end
  else   
    loop Abs(n) do
    begin  
      ShiftRight(g[1,0,z],g[0,1,z],g[1,2,z],g[2,1,z]);
      ShiftRight(g[2,0,z],g[0,0,z],g[0,2,z],g[2,2,z]);
    end
end;

procedure FrontRotate(n: integer := 1);
begin
  IsAnimated := True;
  var x := 2;
  if n = 0 then exit;
  for var y := 0 to 2 do
  for var z := 0 to 2 do
    g[x,y,z].AnimRotateAtAbsolute(OrtX,-90*n,Origin,sec*Abs(n),EndAnim).Begin;

  if n<0 then
    loop Abs(n) do
    begin  
      ShiftLeft(g[x,1,0],g[x,0,1],g[x,1,2],g[x,2,1]);
      ShiftLeft(g[x,2,0],g[x,0,0],g[x,0,2],g[x,2,2]);
    end
  else   
    loop Abs(n) do
    begin  
      ShiftRight(g[x,1,0],g[x,0,1],g[x,1,2],g[x,2,1]);
      ShiftRight(g[x,2,0],g[x,0,0],g[x,0,2],g[x,2,2]);
    end
end;

procedure BackRotate(n: integer := 1);
begin
  IsAnimated := True;
  var x := 0;
  if n = 0 then exit;
  for var y := 0 to 2 do
  for var z := 0 to 2 do
    g[x,y,z].AnimRotateAtAbsolute(OrtX,90*n,Origin,sec*Abs(n),EndAnim).Begin;

  if n>0 then
    loop Abs(n) do
    begin  
      ShiftLeft(g[x,1,0],g[x,0,1],g[x,1,2],g[x,2,1]);
      ShiftLeft(g[x,2,0],g[x,0,0],g[x,0,2],g[x,2,2]);
    end
  else   
    loop Abs(n) do
    begin  
      ShiftRight(g[x,1,0],g[x,0,1],g[x,1,2],g[x,2,1]);
      ShiftRight(g[x,2,0],g[x,0,0],g[x,0,2],g[x,2,2]);
    end
end;



procedure InitScene;
begin
  View3D.ShowGridLines := False;
  Camera.Position := P3D(10,6,6);
  Camera.LookDirection := V3D(-10,-6,-6);
  Window.Title := 'Кубик Рубика';
  View3D.Title := 'Вращение граней:';
  View3D.SubTitle := 'F,B - передняя-задняя'#10'U,D - верхняя-нижняя'#10'L,R - левая-правая';
  Lights.AddDirectionalLight(Colors.DarkGray,V3D(-2.0,0,0));
  Lights.AddDirectionalLight(Colors.Gray,V3D(2.0,0,0));
  Lights.AddDirectionalLight(RGB(150,150,150),V3D(0,0,2));
  Lights.AddDirectionalLight(RGB(50,50,50),V3D(0,0,-2));
  Lights.AddDirectionalLight(Colors.Gray,V3D(0,2,0));
end;

procedure Mix;
begin
  sec := 0.5;
  loop 20 do
  begin
    case Random(6) of
      0: RightRotate;
      1: LeftRotate;
      2: UpRotate;
      3: DownRotate;
      4: FrontRotate;
      5: BackRotate;
    end;
    //Sleep(Round((sec+0.03)*1000));
  end;  
  sec := 0.5;  
end;

begin
  InitScene;
  CreateCube;
  OnKeyDown := k -> begin
    if IsAnimated then 
      exit;
    case k of
      Key.R: RightRotate;
      Key.L: LeftRotate;
      Key.U: UpRotate;
      Key.D: DownRotate;
      Key.F: FrontRotate;
      Key.B: BackRotate;
      Key.M: Mix;
    end;
  end;
end.
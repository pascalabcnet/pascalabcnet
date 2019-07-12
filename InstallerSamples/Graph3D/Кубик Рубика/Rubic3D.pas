uses Graph3D;

var g := new Object3D[3,3,3];

procedure CreateCube;
begin
  var b := Box(0,0,-0.5,1,1,0.01,Colors.White);
  var b3 := Box(0,0,0.5,1,1,0.01,Colors.Yellow);
  
  var b1 := Box(0.5,0,0,0.01,1,1,Colors.Red);
  var b4 := Box(-0.5,0,0,0.01,1,1,Colors.Orange);

  var b2 := Box(0,0.5,0,1,0.01,1,Colors.Green);
  var b5 := Box(0,-0.5,0,1,0.01,1,Colors.Blue);

  var g000 := Group(b,b1,b2,b3,b4,b5);
  g[1,1,1] := g000;

  var a := 1.02;

  for var x := 0 to 2 do
  for var y := 0 to 2 do
  for var z := 0 to 2 do
    if (x,y,z) <> (1,1,1) then
      g[x,y,z] := g000.Clone.MoveOn(a*x-a,a*y-a,a*z-a);   
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
    g[x,y,z].AnimRotateAtAbsolute(OrtY,-90*n,Origin,1*Abs(n),EndAnim).Begin;
  
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
    g[x,y,z].AnimRotateAtAbsolute(OrtY,90*n,Origin,1*Abs(n),EndAnim).Begin;

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
    g[x,y,z].AnimRotateAtAbsolute(OrtZ,-90*n,Origin,1*Abs(n),EndAnim).Begin;

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
    g[x,y,z].AnimRotateAtAbsolute(OrtZ,90*n,Origin,1*Abs(n),EndAnim).Begin;

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
    g[x,y,z].AnimRotateAtAbsolute(OrtX,-90*n,Origin,1*Abs(n),EndAnim).Begin;

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
    g[x,y,z].AnimRotateAtAbsolute(OrtX,90*n,Origin,1*Abs(n),EndAnim).Begin;

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
    end;
  end;
end.
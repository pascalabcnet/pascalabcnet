uses GraphABC,ABCObjects;

var 
  r,r1,r2: RectangleABC;
  s: SquareABC;
  c: CircleABC;
  rr: RoundSquareABC;
  a: array of Point;
  t: TextABC;
  p: RegularPolygonABC;
  z: StarABC;
  pic: PictureABC;
  
  pp,pp1: Picture;
  m: MultiPictureABC;
  
  cc: ContainerABC;
  b: ObjectBoardABC;

begin
  CenterWindow;
  SetBrushColor(clLightBlue);
//  Rectangle(10,10,500,500);
//  SetSmoothingOff;
//  LockDrawingObjects;
{  Font.Name := 'Arial';
  SetFontColor(clGreen);
  Font.Style := fsItalic;
  TextOut(10,10,'fgjhfdj');
  Brush.Color := clRandom;
  CenterWindow;
  Ellipse(120,100,240,200);
  r1 := new RectangleABC(10,40,180,60,clLightGray);
  r2 := new RectangleABC(120,60,100,110,clYellow);
  rr := new RoundSquareABC(140,200,200,30,clLightBlue);
  rr.Radius := 55;
  rr.FontName := 'Courier New';
  rr.Text := 'Hello';
  rr.TextScale := 0.9;
  rr.FontStyle := fsBold;

  r2 := r2.Clone;
  r2.MoveOn(10,10);

  s := new SquareABC(300,40,60,clLightGreen);
  s.Height := 180;

//  Sleep(500);

  t := new TextABC(101,300,26,clGreen,'Текст gWЙ');
  t.TransparentBackground := False;
  t.BackgroundColor := clGray;
  t.FontSize := 11;
  t.Text := 'dsfKJH';
  
  c := new CircleABC(400,300,50,clGreen);
  c.Radius := c.Radius + 1;
  p := new RegularPolygonABC(400,300,50,3,clLightSalmon);
  p.Radius := p.Radius + 1;
  p.Angle := 20;
  
  z := new StarABC(200,300,70,40,7,clMoccasin);}

//  LockDrawingObjects;  
{  pic := new PictureABC(100,100,'1.bmp');
  pic.Transparent := True;
  pic.TransparentColor := clWhite;
  
  m := new MultiPictureABC(250,200,'a1.bmp');
  m.Add('a2.bmp');
  m.Transparent := True;
  m.CurrentPicture := 1;}
  
{  r := new RectangleABC(0,0,100,100,clGreen);
  cc := new ContainerABC(100,100);
  cc.Add(r);
  r := r.Clone;
  r.MoveOn(150,0);
  cc.Add(r);
  Sleep(1000);
  cc.MoveOn(100,0);}
  b := new ObjectBoardABC(10,10,4,5,70,40,clWhite);
  b.BorderColor := clGreen;
  b[2,3] := new RectangleABC(0,0,30,30,clBlue);
  b.SwapObjects(2,3,2,4);
  b.DestroyObject(2,3);
end.
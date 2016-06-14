// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit GraphABCHelper;

//{$apptype windows}
{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}

interface

uses System.Drawing;

type Color = System.Drawing.Color;

// Primitives
procedure Line(x1,y1,x2,y2: integer; gr: Graphics);
procedure Line(x1,y1,x2,y2: integer; c: Color; gr: Graphics);

procedure FillEllipse(x1,y1,x2,y2: integer; gr: Graphics);
procedure DrawEllipse(x1,y1,x2,y2: integer; gr: Graphics);
procedure FillRectangle(x1,y1,x2,y2: integer; gr: Graphics);
procedure FillRect(x1,y1,x2,y2: integer; gr: Graphics);
procedure DrawRectangle(x1,y1,x2,y2: integer; gr: Graphics);
procedure FillCircle(x,y,r: integer; gr: Graphics);
procedure DrawCircle(x,y,r: integer; gr: Graphics);
procedure DrawRoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);
procedure FillRoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);

procedure Ellipse(x1,y1,x2,y2: integer; gr: Graphics);
procedure Rectangle(x1,y1,x2,y2: integer; gr: Graphics);
procedure Circle(x,y,r: integer; gr: Graphics);
procedure RoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);

procedure Arc(x,y,r,a1,a2: integer; gr: Graphics);
procedure Pie(x,y,r,a1,a2: integer; gr: Graphics);
procedure FillPie(x,y,r,a1,a2: integer; gr: Graphics);
procedure DrawPie(x,y,r,a1,a2: integer; gr: Graphics);

procedure DrawPolygon(a: array of Point; gr: Graphics);
procedure FillPolygon(a: array of Point; gr: Graphics);
procedure Polygon(a: array of Point; gr: Graphics);
procedure Polyline(a: array of Point; gr: Graphics);
procedure Curve(a: array of Point; gr: Graphics);
procedure DrawClosedCurve(a: array of Point; gr: Graphics);
procedure FillClosedCurve(a: array of Point; gr: Graphics);
procedure ClosedCurve(a: array of Point; gr: Graphics);

procedure TextOut(x,y: integer; s: string; gr: Graphics); 
procedure DrawTextCentered(x,y,x1,y1: integer; s: string; gr: Graphics); 
procedure DrawTextCentered(x,y: integer; s: string; gr: Graphics); 

function GetView(b: Bitmap; r: System.Drawing.Rectangle): Bitmap;
function ImageIntersect(b1,b2: Bitmap): boolean;

var 
  _CurrentTextBrush: SolidBrush; 
  _ColorLinePen: System.Drawing.Pen;

///--
procedure __InitModule__;

implementation

uses System, System.Drawing, System.Drawing.Drawing2D, GraphABC; 
     
var
  sf: StringFormat;
     
function ImageIntersect(b1,b2: Bitmap): boolean;
// Предполагается, что b1 и b2 - одного размера
type 
  RGB = record
    r,g,b: byte;
  end;
  PRGB = ^uint32;
label 1;
var
  ptr1,ptr2: pointer;
  iptr1,iptr2: integer;
  p1,p2: PRGB;
  rect: System.Drawing.Rectangle;
  bmpData1,bmpData2: System.Drawing.Imaging.BitmapData;
begin
  rect := new System.Drawing.Rectangle(0,0,b1.Width,b1.Height);
  bmpData1 := b1.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b1.PixelFormat);
  ptr1 := bmpData1.Scan0.ToPointer;
  iptr1 := integer(ptr1);

  bmpData2 := b2.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b2.PixelFormat);
  ptr2 := bmpData2.Scan0.ToPointer;
  iptr2 := integer(ptr2);

  var pixelFormatSize := Image.GetPixelFormatSize(b1.PixelFormat) div 8;
  var Offset := bmpData1.stride - b1.Width * pixelFormatSize; 
  
  Result := False;
  for var y:=0 to b1.Height-1 do
  begin
    for var x:=0 to b1.Width-1 do
    begin
      p1 := PRGB(pointer(iptr1));
      p2 := PRGB(pointer(iptr2));
      //writeln(p1^.R,' ',p1^.G,' ',p1^.B,'  ',p2^.R,' ',p2^.G,' ',p2^.B);
//      if ((p1^.R and p1^.G and p1^.B)<>$FF) and ((p2^.R and p2^.G and p2^.B)<>$FF) then
      if (p1^<>$FFFFFFFF) and (p2^<>$FFFFFFFF) then
      begin
        Result := True;
        goto 1;
      end;
      iptr1 += pixelFormatSize;
      iptr2 += pixelFormatSize;
    end;
    iptr1 += Offset;
    iptr2 += Offset;
  end;
1:  
  b2.UnlockBits(bmpData1);
  b1.UnlockBits(bmpData1);
end;     
     
function GetView(b: Bitmap; r: System.Drawing.Rectangle): Bitmap;
var 
  w,h,stride,padding,pixelFormatSize: integer;
  ptr,start: System.IntPtr;
  rect: System.Drawing.Rectangle;
  bmpData: System.Drawing.Imaging.BitmapData;
begin
  w := b.Width;
  h := b.Height;
  rect := new System.Drawing.Rectangle(0,0,w,h);
  bmpData := b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b.PixelFormat);
          
  ptr := bmpData.Scan0;
  b.UnlockBits(bmpData);

  pixelFormatSize := Image.GetPixelFormatSize(b.PixelFormat) div 8;
  
  r := System.Drawing.Rectangle.Intersect(r,rect);
  start := (System.IntPtr)(integer(ptr) + w * pixelFormatSize * r.Y + r.X * pixelFormatSize);

  stride := w * pixelFormatSize;
  padding := stride mod 4;
  if padding<>0 then 
    stride := stride + 4 - padding; //pad out to multiple of 4

  Result := new Bitmap(r.Width,r.Height,stride,b.PixelFormat,start);
end;
     
procedure Swap(var x1,x2: integer);
var t: integer;
begin
  t := x1;
  x1 := x2;
  x2 := t;
end; 

// Graphics Primitives
procedure Line(x1,y1,x2,y2: integer; c: Color; gr: Graphics);
begin
  _ColorLinePen.Color := c;
  gr.DrawLine(_ColorLinePen,x1,y1,x2,y2);
end;

procedure Line(x1,y1,x2,y2: integer; gr: Graphics);
begin
  gr.DrawLine(Pen.NETPen,x1,y1,x2,y2);
end;

procedure FillRectangle(x1,y1,x2,y2: integer; gr: Graphics);
var sm: SmoothingMode;
begin
  if x1>x2 then
    Swap(x1,x2);
  if y1>y2 then
    Swap(y1,y2);
  sm := gr.SmoothingMode;
  gr.SmoothingMode := SmoothingMode.None;  
  if Brush.NETBrush<>nil then 
    gr.FillRectangle(Brush.NETBrush,x1,y1,x2-x1,y2-y1);
  gr.SmoothingMode := sm;
end;

procedure FillRect(x1,y1,x2,y2: integer; gr: Graphics);
begin
  FillRectangle(x1,y1,x2,y2,gr);
end;

procedure DrawRectangle(x1,y1,x2,y2: integer; gr: Graphics);
begin
  if x1>x2 then
    Swap(x1,x2);
  if y1>y2 then
    Swap(y1,y2);
  gr.DrawRectangle(Pen.NETPen,x1,y1,x2-x1-1,y2-y1-1);
end;

procedure Rectangle(x1,y1,x2,y2: integer; gr: Graphics);
begin
  if Brush.NETBrush <> nil then 
    FillRectangle(x1,y1,x2-1,y2-1,gr);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawRectangle(x1,y1,x2,y2,gr);
end;

procedure DrawEllipse(x1,y1,x2,y2: integer; gr: Graphics);
begin
  if x1>x2 then
    Swap(x1,x2);
  if y1>y2 then
    Swap(y1,y2);
  gr.DrawEllipse(Pen.NETPen,x1,y1,x2-x1-1,y2-y1-1);
end;

procedure FillEllipse(x1,y1,x2,y2: integer; gr: Graphics);
begin
  if x1>x2 then
    Swap(x1,x2);
  if y1>y2 then
    Swap(y1,y2);
  gr.FillEllipse(Brush.NETBrush,x1,y1,x2-x1-1,y2-y1-1);
end;

procedure Ellipse(x1,y1,x2,y2: integer; gr: Graphics);
begin
  if Brush.NETBrush <> nil then 
    FillEllipse(x1,y1,x2,y2,gr);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawEllipse(x1,y1,x2,y2,gr);
end;

procedure DrawCircle(x,y,r: integer; gr: Graphics);
begin
  gr.DrawEllipse(Pen.NETPen,x-r,y-r,x+r,y+r);
end;

procedure FillCircle(x,y,r: integer; gr: Graphics);
begin
  gr.FillEllipse(Brush.NETBrush,x-r,y-r,x+r,y+r);
end;

procedure Circle(x,y,r: integer; gr: Graphics);
begin
  FillCircle(x,y,r,gr);
  DrawCircle(x,y,r,gr);
end;

function CreatePathForDrawRoundRect(x1,y1,x2,y2,w,h: integer): GraphicsPath;
var xx,yy: integer;
begin
  if x1>x2 then
    Swap(x1,x2);
  if y1>y2 then
    Swap(y1,y2);
  xx := w div 2;
  yy := h div 2;
  Result := new GraphicsPath();
  Result.AddLine(x1 + xx, y1, x2 - xx, y1);
  Result.AddArc(x2 - w - 1, y1, w, h, 270, 90);
  Result.AddLine(x2 - 1, y1 + yy, x2 - 1, y2 - yy);
  Result.AddArc(x2 - w - 1, y2 - h - 1, w, h, 0, 90);
  Result.AddLine(x2 - xx, y2 - 1, x1 + xx, y2 - 1);
  Result.AddArc(x1, y2 - h - 1, w, h, 90, 90);
  Result.AddLine(x1, y2 - yy, x1, y1 + yy);
  Result.AddArc(x1, y1, w, h, 180, 90);
  Result.CloseFigure();
end;

procedure DrawRoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);
var path: GraphicsPath;
begin
  path := CreatePathForDrawRoundRect(x1,y1,x2,y2,w,h);
  gr.DrawPath(Pen.NETPen, path);
  path.Dispose;
end;

procedure FillRoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);
var path: GraphicsPath;
begin
  path := CreatePathForDrawRoundRect(x1,y1,x2,y2,w,h);
  gr.FillPath(Brush.NETBrush, path);
  path.Dispose;
end;

procedure RoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);
begin
  if Brush.NETBrush <> nil then 
    FillRoundRect(x1,y1,x2,y2,w,h,gr);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawRoundRect(x1,y1,x2,y2,w,h,gr);
end;

procedure Arc(x,y,r,a1,a2: integer; gr: Graphics);
begin
  a1:=360-a1;
  a2:=360-a2;
  if a2<a1 then
    Swap(a1,a2);
  gr.DrawArc(Pen.NETPen,x-r,y-r,2*r,2*r,a1,a2-a1);
end;

procedure DrawPie(x,y,r,a1,a2: integer; gr: Graphics);
begin
  a1:=360-a1;
  a2:=360-a2;
  if a2<a1 then
    Swap(a1,a2);
  gr.DrawPie(Pen.NETPen,x-r,y-r,2*r,2*r,a1,a2-a1);
end;

procedure FillPie(x,y,r,a1,a2: integer; gr: Graphics);
begin
  a1:=360-a1;
  a2:=360-a2;
  if a2<a1 then
    Swap(a1,a2);
  gr.FillPie(Brush.NETBrush,x-r,y-r,2*r,2*r,a1,a2-a1);
end;

procedure Pie(x,y,r,a1,a2: integer; gr: Graphics);
begin
  if Brush.NETBrush <> nil then 
    FillPie(x,y,r,a1,a2,gr);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawPie(x,y,r,a1,a2,gr);
end;

procedure DrawPolygon(a: array of Point; gr: Graphics);
begin
  gr.DrawPolygon(Pen.NETPen,a);
end;

procedure FillPolygon(a: array of Point; gr: Graphics);
begin
  gr.FillPolygon(Brush.NETBrush,a);
end;

procedure Polygon(a: array of Point; gr: Graphics);
begin
  if Brush.NETBrush <> nil then 
    FillPolygon(a,gr);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawPolygon(a,gr);
end;

procedure Polyline(a: array of Point; gr: Graphics);
begin
  gr.DrawLines(Pen.NETPen,a);
end;

procedure Curve(a: array of Point; gr: Graphics);
begin
  gr.DrawCurve(Pen.NETPen,a);
end;

procedure DrawClosedCurve(a: array of Point; gr: Graphics);
begin
  gr.DrawClosedCurve(Pen.NETPen,a);
end;

procedure FillClosedCurve(a: array of Point; gr: Graphics);
begin
  gr.FillClosedCurve(Brush.NETBrush,a);
end;

procedure ClosedCurve(a: array of Point; gr: Graphics);
begin
  if Brush.NETBrush <> nil then 
    FillClosedCurve(a,gr);
  if Pen.NETPen.DashStyle <> DashStyle.Custom then
    DrawClosedCurve(a,gr);
end;

procedure TextOut(x,y: integer; s: string; gr: Graphics); 
begin
  var sz := gr.MeasureString(s,Font.NETFont,0,sf);
  if Brush.NETBrush <> nil then 
    FillRectangle(x,y,x+round(sz.Width),y+round(sz.Height)+1,gr);
  gr.DrawString(s,Font.NETFont,_CurrentTextBrush,x,y,sf);
end;

procedure DrawTextCentered(x,y,x1,y1: integer; s: string; gr: Graphics); 
begin
  var sf := new StringFormat();
  sf.Alignment := StringAlignment.Center;
  sf.LineAlignment := StringAlignment.Center;
   
  gr.DrawString(s,Font.NETFont,_CurrentTextBrush,new System.Drawing.RectangleF(x,y,x1-x,y1-y),sf);
end;

procedure DrawTextCentered(x,y: integer; s: string; gr: Graphics); 
begin
  var sf := new StringFormat();
  sf.Alignment := StringAlignment.Center;
  sf.LineAlignment := StringAlignment.Center;
   
  gr.DrawString(s,Font.NETFont,_CurrentTextBrush,x,y,sf);
end;

var __initialized := false;

procedure __InitModule;
begin
  sf := new StringFormat(StringFormat.GenericTypographic);
  sf.FormatFlags := StringFormatFlags.MeasureTrailingSpaces;
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitModule;
  end;
end;

initialization
  __InitModule;
end.
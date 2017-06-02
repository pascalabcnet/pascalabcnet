uses
  System.Drawing, 
  System.Windows.Forms,
  System.Threading,
  FormsABC; 

procedure DrawMandelbrot(g: Graphics; w,h: integer; scale: real; dx,dy: integer);
const max = 10;
begin
  for var ix:=0 to w-1 do
  for var iy:=0 to h-1 do
  begin
    var x := 0.0;
    var y := 0.0;
    var cx := scale * (ix - dx);
    var cy := scale * (iy - dy);
    var i := 1;
    while i<255 do
    begin
      var x1 := x*x-y*y+cx;
      var y1 := 2*x*y+cy;
      x := x1;
      y := y1;
      if (abs(x)>max) and (abs(y)>max) then break;
      i += 1;
    end;
    if i>=255 then 
      g.FillRectangle(Brushes.Red,ix,iy,1,1)
    else 
      g.FillRectangle(new SolidBrush(Color.FromArgb(255,255-i,255-i)),ix,iy,1,1)
  end;
end;

var 
  Scale := new RealField('Масштаб: ');
  l1 := new FlowBreak;
  dx := new IntegerField('dx: ');
  l2 := new FlowBreak;
  dy := new IntegerField('dy: ');
  l3 := new FlowBreak(20);
  b := new Button(' Нарисовать ');
  p: PaintBox;


procedure Draw;
begin
  var g := p.Graphics;
  DrawMandelbrot(g,p.Width,p.Height,Scale.Value,dx.Value,dy.Value);
  p.Invalidate;
end;

procedure My(o: Object);
begin
  Draw;
end;

procedure Click;
begin
  ThreadPool.QueueUserWorkItem(My);
end;

begin
  MainForm.Title := 'Множество Мандельброта';
  MainForm.SetSize(700, 600);
  MainPanel.Dock := Dockstyle.Left;
  MainPanel.Width := 120;
  Scale.Value := 0.0035;
  dx.Value := 430;
  dy.Value := 280;
  b.Click += Click;

  ParentControl := MainForm;
  p := new PaintBox;
  p.Dock := DockStyle.Fill;
  ThreadPool.QueueUserWorkItem(My);
end.
uses vcl,System,System.Collections,System.Windows.Forms,System.Drawing;


type TMyForm = class (Form)
		    btn1 : Button;
		    txtBox1 : TextBox;
		    bmp : Bitmap;
		    g : Graphics;
			gbmp : Graphics;

		   constructor Create;
		   begin
		    //Height := 320;
		    Height := $140;
		    Width := 420;
		    Text := 'Mandelbrot';
		    BackColor := Color.FromArgb(255,255,255,255);
		    bmp := Bitmap.Create(1600,1200);
		    g := Graphics.FromHwnd(Handle);
		    gbmp := Graphics.FromImage(bmp);
		   end;
		   
procedure TMyForm_Click(sender: System.Object; e: EventArgs);
begin
end;

procedure TMyForm_Paint(sender: System.Object; e: PaintEventArgs);
var gp : Graphics;
begin
 gp := e.Graphics;
 gp.DrawImage(bmp,0,0);
end;
		   

procedure DrawMandelbrot;
var
  x,y,x1,y1,cx,cy: real;
  i,ix,iy, n, max,count : integer;
  p : Pen;
begin
  count := 0;
  n := 255; max := 10;
  ix := 0; iy := 0;
  p := Pen.Create(Color.FromArgb(255,255,0,0));
  p.Color := Color.FromArgb(255,255,0,0);
  while ix <= 400 do
  begin
   iy := 0;
  while iy <= 300 do
  begin
    x:=0;
    y:=0;
    cx:=0.002*(ix-720);
    cy:=0.002*(iy-150);
    i := 1;
    
    while i <= n do
    begin
      x1:=x*x-y*y+cx;
      y1:=x*y*2+cy;
      if (x1>max) or (y1>max) then break;
      x:=x1;
      y:=y1;
      i := i + 1;
    end;
    if i>=n then
    begin 
	p.Color := Color.FromArgb(255,255,0,0);
	g.DrawLine(p,ix,iy,ix+1,iy+1);
	gbmp.DrawLine(p,ix,iy,ix+1,iy+1);

    end
    else
    begin 
	p.Color := Color.FromArgb(255,255,255-i,255-i);
	g.DrawLine(p,ix,iy,ix+1,iy+1);
	gbmp.DrawLine(p,ix,iy,ix+1,iy+1);
    end;
    iy := iy+1; 
  end;
   ix := ix+1;
  end;
end;

end;

procedure my_proc(obj : System.Object);
begin
end;

var
f : System.Windows.Forms.Form;
ff : TMyForm;
b : boolean;
g : Graphics;


begin

f := System.Windows.Forms.Form.Create;
//f.ShowDialog;
ff := TMyForm.Create;
ff.Show;

//g := Graphics.FromHwnd(ff.Handle);
//g.DrawEllipse(Pen.Create(Color.FromArgb(255,100,200,100)),10,10,200,200);
ff.DrawMandelbrot;
Application.Run(ff);
//MessageBox.Show('Application exit');
end.

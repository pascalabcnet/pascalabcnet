uses vcl,System,System.Collections,System.Windows.Forms,System.Drawing;

type TMyForm = class (Form)
		    btn1 : Button;
		    txtBox1 : TextBox;

		   constructor Create;
		   begin
		    set_Height(600);
		    set_Width(800);
		    set_Text('Guseniza');
		    set_BackColor(Color.FromArgb(255,255,255,255));

		   end;
		   
		   end;


var xx, yy, rr, rr1, rr0, x1, y1, x2, y2, j, r, g, b : integer;
	i, s, t, x, y, z, p, q, pi : real;
	ff : TMyForm;
	gr : Graphics;
	pen1 : Pen; 

begin
 ff := TMyForm.Create;
 ff.Show;
 pen1 := Pen.Create(Color.FromArgb(255,0,0,0));
 gr := Graphics.FromHwnd(ff.Handle);

 pi := 3.1415926;
 xx := 430;
 yy := 260;
 //SetWindowSize (800, 600);
 rr := 240;
 rr1 := 30;
 i := 0;
 while i <= 360 do
  begin
  j := 0;
  while j <= 360 do
   begin
    t := i*pi/180;
    s := j*pi/180;
    q := Math.Abs(Math.Sin(16*t));
    p := Math.Abs(Math.Sin(8*s));
    if (q < 0.00000001) or (p < 0.00000001) then rr0 := Convert.ToInt32(Math.Round(rr1+60*Math.Sin(2*t)*Math.Sin(2*t)))
    else
     rr0 :=  Convert.ToInt32(Math.Round(rr1 + 60*Math.Sin(2*t)*Math.Sin(2*t)+60*Math.Exp(24*Math.Log(p*q))));
    x := rr*Math.Cos(t)+rr0*Math.Cos(s);
    y := rr0*Math.Sin(s);
    z := rr*Math.Sin(2*t)+rr0*Math.Cos(s);
    x1 :=  Convert.ToInt32(Math.Round(x - z*Math.Cos(pi/3)*0.5));
    y1 :=  Convert.ToInt32(Math.Round(y - z*Math.Sin(pi/3)*0.5));
    r := 200 * rr0 div (rr1+60);
    if r > 255 then r := r mod 255;
    g := 100 * ( Convert.ToInt32(Math.Round(i+j)) mod 255);
    if g > 255 then g := g mod 255;
    b :=  Convert.ToInt32(Math.Round(1.5*i*(j mod 255)));
    if b > 255 then b := b mod 255;
    pen1.set_Color(Color.FromArgb(255,r,g,b));
    gr.DrawLine (pen1,x1+xx, y1+yy, x2+xx, y2+yy);
    x2 := x1; y2 := y1;
    j := j+2;
  end;
 i := i+0.5;
end;
Application.Run(ff);  	
end.

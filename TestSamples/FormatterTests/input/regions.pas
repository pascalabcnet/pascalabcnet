#reference 'System.Windows.Forms.dll'
#reference 'System.Drawing.dll'

uses System,
     System.Collections,
     System.Windows.Forms,
     System.Drawing,
     System.Drawing.Drawing2D;

type TMyForm = class (Form)
		    btn1 : Button;
		    txtBox1 : TextBox;

		   constructor Create;
		   begin
		    set_Height(300);
		    set_Width(400);
		    //set_Text('Mandelbrot');
		    set_BackColor(Color.FromArgb(255,255,0,0));
		    SuspendLayout;
		   end;
		   
		   end;


var f : TMyForm;
    g : Graphics;
    gp : GraphicsPath;
    txt : string;
    fs : FontStyle;
    ff : FontFamily;
    dim : integer;
    sl : Point;
sl2 : Point;
    sf : StringFormat;

begin

 f := TMyForm.Create;
 f.Show;

 g := Graphics.FromHwnd(f.Handle);
 //g.DrawEllipse(Pen.Create(Color.FromArgb(255,100,200,100)),10,10,200,200);

 gp := GraphicsPath.Create;
 txt := 'Hello, World!';
 ff := FontFamily.Create('Comic Sans MS');
 dim := 40;
 sl.X := 0; sl.Y := 0;
 sl2 := sl;
 //gp.AddRectangle(new System.Drawing.Rectangle(f.Location.X,f.Location.Y,100,100));
 gp.AddString(txt,ff,0,Convert.ToSingle(40.0),sl,sf);
 //gp.StartFigure;
 //gp.AddPie(Rectangle.Create(0,0,f.ClientSize.Width,f.ClientSize.Height),0,90);
 //gp.AddPie(Rectangle.Create(0,0,f.ClientSize.Width,f.ClientSize.Height),180,90);
 //gp.AddEllipse(30,30,f.ClientSize.Width,f.ClientSize.Height);
 //gp.CloseFigure;
 f.Region := Region.Create(gp);
 Application.Run(f);
end.

{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}
{$apptype windows}

uses System, System.Drawing, System.Windows.Forms, OpenGL;

type Form1 = class(Form)

_hdc : HDC;

constructor Create;
begin
  _hdc := GetDC(self.Handle.ToInt32());
  OpenGLInit(self.Handle);
end;

protected procedure OnPaint(e: System.Windows.Forms.PaintEventArgs); override;
begin
  glClearColor(single(0.0), single(0.0), single(0.0), single(0.0));
  glClear(GL_COLOR_BUFFER_BIT or GL_DEPTH_BUFFER_BIT);
  glColor3f(single(0.0), single(1.0), single(0.0));
  glOrtho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);
  glBegin(GL_POLYGON);
    glVertex2f(single(-0.5), single(-0.5));
    glVertex2f(single(-0.5), single(0.5));
    glVertex2f(single(0.5), single(0.5));
    glVertex2f(single(0.5), single(-0.5));
  glEnd();
  glFlush();
  SwapBuffers(_hdc);
end;

procedure Form_Closed(sender : object; e : EventArgs);
begin
  OpenGLUninit(self.Handle);
end;

procedure Form_Resize(sender: object; e : EventArgs);
begin
  glViewport(0, 0, Width, Height);
end;
end;

var f : Form1;
begin
  f := new Form1();
  //f.Load += f.Form_Load;
  f.Resize += f.Form_Resize;
  f.Closed += f.Form_Closed;
  //f.Paint += f.InternalPaint;
  Application.Run(f);
end.
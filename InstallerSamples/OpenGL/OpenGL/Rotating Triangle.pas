{$reference System.Windows.Forms.dll}
{$reference System.Drawing.dll}
uses System.Windows.Forms;
uses OpenGL;
uses System;

type
  PIXELFORMATDESCRIPTOR = record
    nSize: Word;
    nVersion: Word;
    dwFlags: longword;
    iPixelType: Byte;
    cColorBits: Byte;
    cRedBits: Byte;
    cRedShift: Byte;
    cGreenBits: Byte;
    cGreenShift: Byte;
    cBlueBits: Byte;
    cBlueShift: Byte;
    cAlphaBits: Byte;
    cAlphaShift: Byte;
    cAccumBits: Byte;
    cAccumRedBits: Byte;
    cAccumGreenBits: Byte;
    cAccumBlueBits: Byte;
    cAccumAlphaBits: Byte;
    cDepthBits: Byte;
    cStencilBits: Byte;
    cAuxBuffers: Byte;
    iLayerType: Byte;
    bReserved: Byte;
    dwLayerMask: longword;
    dwVisibleMask: longword;
    dwDamageMask: longword;
  end;

function GetDC(hwnd: IntPtr): IntPtr;
external 'user32.dll';

function SetPixelFormat(hdc: IntPtr; iPixelFormat: integer; ppfd: ^PIXELFORMATDESCRIPTOR): boolean;
external 'gdi32.dll';

function ChoosePixelFormat(_hdc: IntPtr; ppfd: ^PIXELFORMATDESCRIPTOR): integer;
external 'gdi32.dll';

function wglCreateContext(_hdc: IntPtr): HGLRC;
external 'opengl32.dll';

function wglMakeCurrent(_hdc: IntPtr; _hglrc: HGLRC): boolean;
external 'opengl32.dll';

function SwapBuffers(_hdc: IntPtr): boolean;
external 'gdi32.dll';

function InitOpenGL(hwnd: IntPtr): IntPtr;
begin
  Result := GetDC(hwnd);
  
  
  
  var pfd: PIXELFORMATDESCRIPTOR;
  pfd.nSize := sizeof( PIXELFORMATDESCRIPTOR );
  pfd.nVersion := 1;
  pfd.dwFlags := $1 or $4 or $20;
  pfd.cColorBits := 24;
  pfd.cDepthBits := 16;
  
  if not SetPixelFormat(
    Result,
    ChoosePixelFormat(Result, @pfd),
    @pfd
  ) then raise new InvalidOperationException;
  
  var context := wglCreateContext(Result);
  if not wglMakeCurrent(Result, context) then raise new InvalidOperationException;
  
  
  
  gl_Deprecated.LoadIdentity;
  gl.ClearColor(0.0, 0.0, 0.0, 1.0);
  
  
  
end;

begin
  var f := new Form;
  
  f.StartPosition := FormStartPosition.CenterScreen;
  f.ClientSize := new System.Drawing.Size(500,500);
  f.FormBorderStyle := FormBorderStyle.Fixed3D;
  
  f.Closing += (o,e)->Halt();
  
  f.Shown += (o,e)->
  begin
    var hdc := InitOpenGL(f.Handle);
    
    var dy := -Sin(Pi/6) / 2;
//    var pts := real(0.0).Step(Pi*2/3).Take(3).Select(rot->(Sin(rot), Cos(rot)+dy)).ToArray; //ToDo #2042
    var pts := Range(0,2).Select(i->i* Pi*2/3 ).Select(rot->(Sin(rot), Cos(rot)+dy)).ToArray;
    var frame_rot := 0.0;
    
    System.Threading.Thread.Create(()->
    while true do
    begin
      
      f.Invoke(()->
      begin
        gl.Clear(BufferTypeFlags.COLOR_BUFFER_BIT);
        var rot_k := Cos(frame_rot);
        
        gl_Deprecated.Begin(PrimitiveType.TRIANGLES);
        gl_Deprecated.Color4f(1,0,0,1); gl_Deprecated.Vertex2f( pts[0][0]*rot_k, pts[0][1] );
        gl_Deprecated.Color4f(0,1,0,1); gl_Deprecated.Vertex2f( pts[1][0]*rot_k, pts[1][1] );
        gl_Deprecated.Color4f(0,0,1,1); gl_Deprecated.Vertex2f( pts[2][0]*rot_k, pts[2][1] );
        gl_Deprecated._End;
        
        frame_rot += 0.03;
        gl.Finish;
        SwapBuffers(hdc);
      end);
      
      Sleep(1);
    end).Start;
    
  end;
  
  Application.Run(f);
end.
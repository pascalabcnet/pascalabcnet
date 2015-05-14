#reference 'Microsoft.DirectX.dll'
#reference 'Microsoft.DirectX.DirectDraw.dll'
#reference 'System.Windows.Forms.dll'
#reference 'System.Drawing.dll'

uses System, System.Windows.Forms, System.Drawing, Microsoft.DirectX, Microsoft.DirectX.DirectDraw;

type MyForm = class(Form)
display : Device;
front, back, title, text : Surface;
clip : Clipper;
titlescreen := Application.StartupPath + '\demo.bmp';
procedure InitializeGraphics;
begin
  var description := new SurfaceDescription();
  display := new Device;
  display.SetCooperativeLevel(self, CooperativeLevelFlags.Normal);
  
  description.SurfaceCaps.PrimarySurface := true;
  front := new Surface(description, display);
  description.Clear;
  
  description.Width := front.SurfaceDescription.Width;
  description.Height := front.SurfaceDescription.Height;
  description.SurfaceCaps.OffScreenPlain := true;
  back := new Surface(description, display);
  
  clip := new Clipper(display);

  clip.Window := self;
    // Set the clipper for the front Surface

  front.Clipper := clip;

    // Reset the description

  description.Clear();
    // Create the title screen

  title := new Surface(titlescreen, description, display);

  description.Clear();
    // Set the height and width of the text.

    description.Width := 600;
    description.Height := 16;
    // OffScreenPlain means that this Surface 

    //is not a front, back, alpha Surface.

    description.SurfaceCaps.OffScreenPlain := true;

    // Create the text Surface

    text := new Surface(description, display);
    // Set the backgroup color

    text.ColorFill(Color.Violet);
    // Set the fore color of the text

    text.ForeColor := Color.White;
    // Draw the Text to the Surface to coords (0,0)

    text.DrawText(0, 0, 
        'Managned DirectX Tutorial 1 - Press Enter or Escape to exit', 
        true);

end;

procedure RestoreSurfaces;
begin
  var description := new SurfaceDescription();

    // Restore al the surface associed with the device
    display.RestoreAllSurfaces();
    // Redraw the text

    text.ColorFill(Color.Black);
    text.DrawText(0, 0, 
        'Managned DirectX Tutorial 1 - Press Enter or Escape to exit', 
        true);

    // For the title screen, we need to 

    //dispose it first and then re-create it

    title.Dispose();
    title := nil;
    title := new Surface(titlescreen,  description, display);
  
end;

protected procedure OnPaint(e: System.Windows.Forms.PaintEventArgs); override;
begin
  Draw;
end;

procedure Draw;
begin
  if front = nil then exit;
  try
    back.DrawFast(0, 0, title, DrawFastFlags.Wait);

        // Draw the text also to the back buffer using source copy blit

    back.DrawFast(10, 10, text, DrawFastFlags.Wait);
    front.Draw(back, DrawFlags.Wait);
    writeln('ok');
  except on SurfaceLostException do
  RestoreSurfaces();
  end;
end;

protected procedure OnKeyPress(e: System.Windows.Forms.KeyPressEventArgs); override;
begin
  inherited;
  if integer(e.KeyChar) = integer(Keys.Escape) then
  Close;
end;
end;

var frm : MyForm;
begin
  frm := new MyForm();
  frm.InitializeGraphics;
  Application.Run(frm);
  
end.
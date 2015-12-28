// Рисование мышью на форме. Иллюстрация Windows.Forms, событий
{$apptype windows}
{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}

uses 
  System, 
  System.Drawing, 
  System.Windows.Forms;

type
  MyForm = class(Form)
  private
    g: Graphics;
    // Предыдущие координаты мыши
    x,y: integer;
  public  
    constructor;
    begin
      Size := new System.Drawing.Size(640,480);
      StartPosition := FormStartPosition.CenterScreen;
      Text := 'Рисование мышью на форме';
      // Привязка обработчиков к событиям
      MouseDown	+= OnMouseDown;
      MouseMove	+= OnMouseMove;
      Resize	+= OnResize;
      g := Graphics.FromHwnd(Handle);
    end;
    procedure OnMouseDown(sender: object; e: MouseEventArgs);
    begin
      x := e.x;
      y := e.y;
    end; 
    procedure OnMouseMove(sender: object; e: MouseEventArgs);
    begin
      if e.Button = System.Windows.Forms.MouseButtons.Left then 
      begin
        g.DrawLine(new Pen(Color.FromARGB(PABCSystem.Random(255),PABCSystem.Random(255),PABCSystem.Random(255))),x,y,e.x,e.y);
        x := e.x;
        y := e.y;
        writeln(e.x,' ',e.y);
      end;
    end; 
    procedure OnResize(sender: object; e: EventArgs);
    begin
      g := Graphics.FromHwnd(Handle);
    end;
  end;
    
begin
  Application.Run(new MyForm);
end.
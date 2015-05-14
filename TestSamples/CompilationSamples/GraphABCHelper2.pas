unit GraphABCHelper2;

//#apptype windows
{$reference 'System.Windows.Forms.dll'}
{$reference 'System.Drawing.dll'}

interface

uses System.Drawing;

type Color = System.Drawing.Color;

procedure FillRoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);
procedure FillRoundRect(x1,y1,x2,y2,w,h: integer; c: Color; gr: Graphics);
implementation

procedure FillRoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);
begin

end;

procedure FillRoundRect(x1,y1,x2,y2,w,h: integer; c: Color; gr: Graphics);
begin

end;

procedure RoundRect(x1,y1,x2,y2,w,h: integer; gr: Graphics);
begin
    FillRoundRect(x1,y1,x2,y2,w,h,gr);
end;


initialization
end.
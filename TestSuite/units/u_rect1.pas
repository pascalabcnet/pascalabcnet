unit u_rect1;
{$reference 'System.Drawing.dll'}

uses
  System.Drawing;
  
begin
  assert((new Rectangle(0, 0, 8, 8)).Contains(4, 4));
end.
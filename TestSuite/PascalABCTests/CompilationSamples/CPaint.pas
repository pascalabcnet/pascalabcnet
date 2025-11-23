// Рисование курсором в консольном окне
// Иллюстрация GotoXY, TextBackGround
// Для запуска программы используйте Shift+F9 !!!
uses Crt;

var
  draw: boolean; 			
  color: integer;
  
function IsCoordCorrect(x,y: integer): boolean;  
begin
  Result := (x in [1..WindowWidth]) and (y in [1..WindowHeight]);
end;
  
procedure MyGotoXY(x,y: integer);
begin
  if not IsCoordCorrect(x,y) then  
    exit;
  GotoXY(x,y);  
end;

procedure DrawSymbol(x,y: integer; c: char);
begin
  if not IsCoordCorrect(x,y) then  
    exit;
  GotoXY(x,y);  
  write(c);
  GotoXY(x,y);  
end;
  
begin
  draw := True;
  color := Green;
  ClrScr;
  SetWindowTitle('Рисование курсором (Esc-выход, Num 5 - изменение цвета)');
  TextBackGround(color);
  GotoXY(WindowWidth div 2,WindowHeight div 2);
  var c: char;
  repeat
    c := ReadKey;
    if c=#32 then 
      draw := not draw;
    if c=#0 then 
    begin
      c := ReadKey;
      case c of                           
  // Изменение цвета по клавише Num 5
    {5} #12: begin
              color := color + 1;
              if color=16 then 
                color := 0;
              TextBackGround(color);
            end;
    {RU}#33: MyGotoXY(WhereX+1,WhereY-1);
    {RD}#34: MyGotoXY(WhereX+1,WhereY+1);
    {LD}#35: MyGotoXY(WhereX-1,WhereY+1);
    {LU}#36: MyGotoXY(WhereX-1,WhereY-1);
    {L} #37: MyGotoXY(WhereX-1,WhereY);
    {U} #38: MyGotoXY(WhereX,WhereY-1);
    {R} #39: MyGotoXY(WhereX+1,WhereY);
    {D} #40: MyGotoXY(WhereX,WhereY+1);
        #67: ClrScr;
      end;
      if draw then 
        DrawSymbol(WhereX, WhereY,' ');
    end;
  until c=#27;
  TextBackGround(Black);
  GotoXY(1,25);
end.

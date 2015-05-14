//Концепция ReadKey в селедующих версиях будет изменена.
uses crt;
var
  c: char;
  draw:boolean; 			
  color:integer;
  x,y:integer;
begin
  draw:=true;
  color:=Green;
  ClrScr;
  TextBackGround(color);
  GotoXY(WindowWidth div 2,WindowHeight div 2);
  repeat
    c:=ReadKey;
    if c=#0 then begin
    c:=ReadKey;
    case c of                           
  {5} #12: begin      
            color:=color+1;
            if color=16 then color:=0;
            TextBackGround(color);
          end;
  {_} #32: draw:=not draw;
  {RU}#33: GotoXY(WhereX+1,WhereY-1);
  {RD}#34: GotoXY(WhereX+1,WhereY+1);
  {LD}#35: GotoXY(WhereX-1,WhereY+1);
  {LU}#36: GotoXY(WhereX-1,WhereY-1);
  {L} #37: GotoXY(WhereX-1,WhereY);
  {U} #38: GotoXY(WhereX,WhereY-1);
  {R} #39: GotoXY(WhereX+1,WhereY);
  {D} #40: GotoXY(WhereX,WhereY+1);
      #67: ClrScr;
    end;
    if draw then begin
      write(' ');
      GotoXY(WhereX-1,WhereY);
    end;
    end;
  until c=#27; 
end.

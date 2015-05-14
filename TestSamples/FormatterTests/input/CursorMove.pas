//Концепция ReadKey в селедующих версиях будет изменена.
uses CRT;

var
  c: char;
begin
  TextBackGround(LightBlue);
  ClrScr;
  GotoXY(40,12);
  repeat
    c:=ReadKey;
    if c=#0 then begin
    c:=readkey;
    case c of
  {L} #37: GotoXY(WhereX-1,WhereY);
  {R} #39: GotoXY(WhereX+1,WhereY);
  {U} #38: GotoXY(WhereX,WhereY-1);
  {D} #40: GotoXY(WhereX,WhereY+1);
    end;
    end;
  until c=#27;
end.

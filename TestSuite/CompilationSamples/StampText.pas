// Класс штампа текста
uses GraphABC;

type 
  TextStamp = class
    x,y,pt: integer;
    Text: string;
    constructor (xx,yy,ppt: integer; t: string);
    begin
      x := xx; y := yy;
      pt := ppt; 
      Text := t;
    end;
    procedure Stamp;
    begin
      Font.Size := pt;
      TextOut(x,y,text);
    end;
    procedure MoveOn(dx,dy: integer);
    begin
      x += dx; y += dy;
    end;
  end;
  
begin
  var txt := new TextStamp(200,200,14,'Привет!');
  txt.Stamp;
  txt.MoveOn(0,40);
  txt.Text := 'До свидания!';
  txt.Stamp;
end. 
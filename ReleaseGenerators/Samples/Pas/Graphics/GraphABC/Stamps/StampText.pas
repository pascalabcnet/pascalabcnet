// Класс штампа текста
uses GraphABC;

type 
  TextStamp = auto class
    x,y,pt: integer;
    Text: string;
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
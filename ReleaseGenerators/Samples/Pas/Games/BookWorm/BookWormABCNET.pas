uses GraphABC,ABCObjects,ABCButtons,Events;

const 
/// Примерная частота повторяемости букв
  freqcharstr='аааааааааааааааааааааааааабббббввввввввггггдддддддееееееееееееееееееееежжззззииииииииииииииииииийккккккккккккккклллллллллллммммммнннннннннннннннннооооооооооооооооооооооооппппппппрррррррррррррррррссссссссссссстттттттттттттттууууууффххцццчччшшщыыьььььэюяяяя';

const
  MaxWordLen = 12;
  scorehits: array [1..MaxWordLen] of integer = (0,1,2,4,7,11,16,22,29,37,46,56);

type 
  MySquareABC = class(SquareABC) end;

var
/// Доска с буквами
  MainBoard: ObjectBoardABC;
/// Доска высоты 1 для размещения слова
  WordBoard: ObjectBoardABC;
/// Номер первого незанятого символа на доске WordBoard
  cur: integer;
/// Количество ходов
  moves: integer;
/// Очки
  score: integer;
/// Прямоугольник для отображения информации
  Status: RectangleABC;

/// Существует ли такое слово (все слова хранятся в файле words.txt)
function WordExists(s: string): boolean;
var
  f: text;
  str: string;
begin
  s := LowerCase(s);
  Result := False;
  assign(f,'words.txt');
  reset(f);
  while not eof(f) do
  begin
    readln(f,str);
    if s=str then
    begin
      Result := True;
      break;
    end;
  end;
  close(f);
end;

procedure MyMouseDown(x,y,mb: integer);
begin
// Нажата левая мышь
  if mb=1 then
  begin
    if cur>WordBoard.DimX then
      exit;
    var ob := ObjectUnderPoint(x,y);
    if (ob is MySquareABC) and ob.Visible then
    begin
      var ob1 := WordBoard[cur,1];
      ob1.Visible := True;
      ob1.Text := ob.Text;
      Inc(cur);
      ob.Visible := False;
      var s := '';
      for var i:=1 to cur-1 do
        s := s + WordBoard[i,1].Text;
      if WordExists(s) then
        WordBoard.Color := clYellow
      else WordBoard.Color := clSkyBlue
    end;
  end
  else
// Нажата правая мышь
  begin
    for var xx:=1 to cur-1 do
      WordBoard[xx,1].Visible := False;
    for var xx:=1 to MainBoard.DimX do
    for var yy:=1 to MainBoard.DimY do
      MainBoard[xx,yy].Visible:=True;
    cur := 1;
    WordBoard.Color := clSkyBlue
  end;
end;

/// Обработчик кнопки "Новая игра"
procedure BtNewClick;
begin
  score := 0;
  moves := 0;
  Status.Text := 'Ходов: '+IntToStr(moves)+'   Очков: '+IntToStr(score);
  MyMouseDown(1,1,2);
  for var xx:=1 to MainBoard.DimX do
  for var yy:=1 to MainBoard.DimY do
    MainBoard[xx,yy].Text := UpCase(freqcharstr[Random(255)+1]);
end;

/// Обработчик кнопки "Сказать слово"
procedure BtWordClick;
begin
  if WordBoard.Color<>clYellow then
    exit;
  Inc(score,scorehits[cur-1]);
  Inc(moves);
  for var xx:=1 to cur-1 do
    WordBoard[xx,1].Visible:=False;
  for var xx:=1 to MainBoard.DimX do
  for var yy:=1 to MainBoard.DimY do
    if not MainBoard[xx,yy].Visible then
    begin
      MainBoard[xx,yy].Visible:=True;
      MainBoard[xx,yy].Text:=UpCase(freqcharstr[Random(255)+1]);
    end;
  cur := 1;
  WordBoard.Color := clSkyBlue;
  Status.Text := 'Ходов: '+IntToStr(moves)+'   Очков: '+IntToStr(score);
end;

/// Обработчик кнопки "Подсказка"
procedure BtPleaseClick;
var
  f: text;
  str,maxstr: string;
  arr,work: array ['а'..'я'] of integer;
  maxlen: integer;

  function CanConstructWord(s: string): boolean;
  begin
    work := arr;
    Result := True;
    for var i:=1 to Length(s) do
    begin
      Dec(work[s[i]]);
      if work[s[i]]<0 then
      begin
        Result := False;
        break;
      end;
    end;
  end;

begin // BtPleaseClick
  maxlen := 0;
  maxstr := '';
  for var c:='а' to 'я' do
    arr[c]:=0;

  for var xx:=1 to MainBoard.DimX do
  for var yy:=1 to MainBoard.DimY do
    Inc(arr[LowCase(MainBoard[xx,yy].Text[1])]);

  assign(f,'words.txt');
  reset(f);
  while not eof(f) do
  begin
    readln(f,str);
    if CanConstructWord(str) and (Length(str)>maxlen) and (Length(str)<=MaxWordLen) then
    begin
      maxlen := Length(str);
      maxstr := str;
    end;
  end;
  close(f);
  
  writeln(maxstr);
end;

procedure InitWindow;
begin
  SetWindowSize(640,480);
  Window.IsFixedSize := True;
  Window.Title := 'Знай русские слова!';
  Brush.Color := clMoneyGreen;
  FillRect(0,0,WindowWidth,WindowHeight);
end;

procedure InitGameVars;
begin
  cur := 1;
  moves := 0;
  score := 0;
end;

procedure InitButtons;
begin
  var btword := new ButtonABC(70,410,180,30,'Сказать слово',clGray);
  var btnew := new ButtonABC(280,410,100,30,'Заново',clLightGray);
  var btplease := new ButtonABC(410,410,160,30,'Подсказка',clGray);
  
// Привязка обработчиков к кнопкам
  btword.OnClick := BtWordClick;
  btnew.OnClick := BtNewClick;
  btplease.OnClick := BtPleaseClick;
end;


procedure InitInterface;
begin
  Status := new RectangleABC(70,350,500,30,clSkyBlue);
  Status.Text := 'Ходов: 0   Очков: 0';
  WordBoard := new ObjectBoardABC(20,40,MaxWordLen,1,50,50,clSkyBlue);
  MainBoard := new ObjectBoardABC(220,120,4,4,50,50,clMoneyGreen);
  MainBoard.BorderColor := clGreen;
  //MainBoard.Bordered := False;

  for var x:=1 to WordBoard.DimX do
  begin
    WordBoard[x,1] := new SquareABC(0,0,WordBoard.CellSizeX-6,clWhite);
    WordBoard[x,1].Visible := False;
  end;
  for var x:=1 to MainBoard.DimX do
  for var y:=1 to MainBoard.DimY do
  begin
    MainBoard[x,y] := new MySquareABC(0,0,MainBoard.CellSizeX-6,clWhite);
    MainBoard[x,y].Text := UpCase(freqcharstr[Random(freqcharstr.Length)+1]);
  end;
end;

begin
  SetConsoleIO;

  InitWindow;
  InitGameVars;
  InitInterface;
  InitButtons;

  OnMouseDown := MyMouseDown;
end.
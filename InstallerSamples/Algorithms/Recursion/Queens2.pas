// Все решения задачи о расстановке ферзей на шахматной доске
const N = 8;

var board := new integer[N]; // board[row] = столбец ферзя в этой строке

procedure Place(row: integer);
begin
  for var col := 0 to N-1 do
  begin
    // проверяем конфликты со всеми ранее поставленными ферзями
    var conflict := False;
    for var prevRow := 0 to row-1 do
      if (board[prevRow] = col) or                    // тот же столбец
         (Abs(board[prevRow] - col) = Abs(prevRow - row)) then // та же диагональ
      begin
        conflict := True;
        break;
      end;
    
    if not conflict then
    begin
      board[row] := col; // ставим ферзя
      
      if row = N-1 then
        board.Println // выводим решение
      else Place(row+1); // продолжаем рекурсию
    end
  end;
end;

begin
  Println('Все решения задачи о 8 ферзях:');
  Place(0);
end.
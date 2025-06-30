function IsSafe(board: array of integer; row, col: integer): boolean;
begin
  Result := True; // Предполагаем, что позиция безопасна
  for var i := 0 to row - 1 do
    if (board[i] = col) or            // Проверка вертикали
       (board[i] - i = col - row) or  // Проверка диагонали /
       (board[i] + i = col + row) then // Проверка диагонали \
    begin
      Result := False; // Найден конфликт
      exit; 
    end;
end;

procedure SolveNQueens(board: array of integer; row: integer);
begin
  if row = 8 then // Все ферзи расставлены успешно
    board.Println
  else
    for var col := 0 to 7 do
      if IsSafe(board, row, col) then
      begin
        board[row] := col; // Поставить ферзя на ряд row
        SolveNQueens(board, row + 1); // Рекурсивный вызов для следующго ряда
      end;
end;

begin
  var board := [0] * 8; 
  SolveNQueens(board, 0); // Начинаем с первой строки (0)
end.
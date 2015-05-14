begin
  with System.Console do begin
    if CursorLeft >= 0 then 
      WriteLine;
    Write('Программа завершена, нажмите любую клавишу . . .');
    Readkey(true);
  end;
end.
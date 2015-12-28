// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
begin
  with System.Console do begin
    if CursorLeft >= 0 then 
      WriteLine;
    Write('Программа завершена, нажмите любую клавишу . . .');
    Readkey(true);
  end;
end.
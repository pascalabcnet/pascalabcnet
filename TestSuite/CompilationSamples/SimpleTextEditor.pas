// Простейший текстовый редактор
// Для запуска программы используйте Shift+F9 !!!
uses CRT;

begin
  SetWindowTitle('Текстовый редактор (Enter - новая строка, Esc - выход)');
  clrScr;
  repeat
    var c := ReadKey;
    case c of
  #13: writeln;
  #27: break;
  #32..#255: write(c);
  #0: c := ReadKey;
    end;
  until false;
end.

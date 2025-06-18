//!Только одномерный динамический массив может описываться с ключевым словом params
procedure pr1(params par1: array[,] of integer);
begin end;

begin
  pr1(1, 2, 3);
end.
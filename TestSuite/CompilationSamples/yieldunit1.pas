unit yieldunit1;

interface//обязательно описание t1.f1 в interface

type
  t1 = class
    function f1: sequence of byte;//f1 обязательно метод какого то класса
  end;

implementation//обязательно реализация t1.f1 в implementation

function t1.f1: sequence of byte;//Ошибка
begin
  yield 0;//обязательно yield
end;

end.
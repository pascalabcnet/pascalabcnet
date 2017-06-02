// Иллюстрация поиска имен вначале справа налево в секции uses, а затем в системном модуле PABCSystem 
uses System;

begin
  // Имя Random, определенное в пространстве имен System, перекрывает имя Random 
  // в модуле PABCSystem, который неявно подключается первым
  var r: Random := new Random();
  writeln(r.Next(10));
  // Именно поэтому перед данным Random необходимо явно указывать имя модуля, в котором он находится
  var i: integer := PABCSystem.Random(10);
  writeln(i);
end.
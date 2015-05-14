///Константа!
const ccc=0;

///<summary>
///Это великая процедура!
///</summary>
procedure p(i:integer);
begin
  writeln('Я великая прооцедура!');
end;

type 
  ///<summary>Это отличный класс.</summary>
  c=class
    ///Как ты уже догодался, это метод P.
    ///Незнаю для чего он нужен.
    procedure p;
    begin
    end;
  end;

///Это супер переменная X.
var x:real;
    ///Это супер переменная CC.
    cc:c;
begin
  p(1);
  
end.
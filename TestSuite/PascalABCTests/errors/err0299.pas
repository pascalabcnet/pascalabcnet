type
  t1=class
    static function operator=(a,b: t1): word := 5;
  end;

begin
  var a := new t1;
  var b := new t1;
  var c := a=b;//Ошибка: Несколько подпрограмм могут быть вызваны
end.
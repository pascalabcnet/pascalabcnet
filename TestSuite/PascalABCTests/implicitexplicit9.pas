var s: string;
type
  t1 = class
    static function operator implicit<T>(val: T): t1;
    begin
      s := 'rec';
    end;
    static function operator implicit<T>(val: ^T): t1;
    begin
      s := 'ptr';
    end;
  end;

  t2<T> = class end;

begin
  var i := 5;
  var a: t1 := @i;
  assert(s = 'ptr');
end. 
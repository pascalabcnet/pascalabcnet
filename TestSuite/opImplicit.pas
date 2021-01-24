type
  t1<T> = class
    o: T;
    constructor(o: T);
    begin
      self.o := o;  
    end;
    static function operator implicit(o: T): t1<T> := new t1<T>(o);
  end;
  
begin
  var a: t1<string> := 'abcde'; 
  Assert(a.o = 'abcde');
end.
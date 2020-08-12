type
  t1<T> = class
    static function operator implicit(o: T): t1<T> := new t1<T>;
  end;
  
begin
  var a: t1<string> := 'abcde'; 
  Assert(a <> nil);
end.
type
  t1<T> = abstract class
    public function Clone: t1<T>; abstract;
    public procedure p;
    begin
      
    end;
  end;
  t2<T> = {sealed} class(t1<T>) end;

begin
  var a := new t2<integer>;
end.
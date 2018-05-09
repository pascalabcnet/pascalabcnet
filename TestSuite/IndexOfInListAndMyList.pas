// Закрывает баг #815

type
  MyList<T> = class(List<T>)
  public
    function IndexOf: integer; 
    begin Print(666); end;
  end;

begin
  var l := new MyList<integer>;
  l.Add(1);
  Assert(l.IndexOf(l[0])=0);
end.
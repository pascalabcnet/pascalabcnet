type
  MyList<T> = class(List<T>)
  public
  end;

begin
  var l := new MyList<integer>;
  l.Add(3);
  l.Add(3);
  l.Add(7);
  Assert(l.IndexOf(7)=2);
end.
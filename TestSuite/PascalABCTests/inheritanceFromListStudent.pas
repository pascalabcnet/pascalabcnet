type
  Student = class
    name: string;
  end;
  MyList<T> = class(List<T>)
  public
  end;

begin
  var l := new MyList<Student>;
  l.Add(new Student);
  Assert(l.IndexOf(l[0])=0);
end.
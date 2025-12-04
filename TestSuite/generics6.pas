type
  Slc<T> = class
    l: List<T>;
    constructor (l: List<T>);
    begin
      self.l := l;
    end;
  public
    procedure p;
    begin
      assert(l[0].ToString()='666');
      var t := l[0];
      assert(t.ToString='666');
      var arr := Arr(l[0]);
      assert(arr[0].ToString()='666');
      //assert(object(l[0]).ToString()='666');
    end;
  end;
  
begin
  var l := new List<integer>();
  l.Add(666);
  var s := new Slc<integer>(l);
  s.p;
end.
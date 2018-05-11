type TClass<T> = class(System.Collections.Generic.IComparer<TClass<T>>)
  i: integer;
  public function Compare(a, b: TClass<T>): integer;
  begin
    Result := integer(a.i > b.i);
  end;
end;

begin
  var o:object := new System.Collections.Generic.List<byte>();
  var e := System.Collections.Generic.List&<byte>(o);
  assert(e.Count = 0);
  e := o as System.Collections.Generic.List<byte>;
  assert(e.Count = 0);
  var a := 0;
  if o is System.Collections.Generic.List&<byte> then
    a := 1;
  assert(a = 1);
  o := 'abc';
  var s := string(o);
  var s1 := o as string;
  assert(o as string = 'abc');
  assert(string(o) = 'abc');
  assert(s = 'abc');
  assert(s1 = 'abc');
  o := 2;
  assert(integer(o) = 2);
  o := new List<integer>;
  assert((o as List<integer>).Count = 0);
  assert(List&<integer>(o).Count = 0);
  o := new TClass<integer>;
  (o as TClass<integer>).i := 2;
  assert((o as System.Collections.Generic.IComparer<TClass<integer>>) <> nil);
  var cmp := System.Collections.Generic.IComparer&<TClass<integer>>(o);
  assert(cmp.Compare(o as TClass<integer>,o as TClass<integer>) = 0);
  cmp := o as System.Collections.Generic.IComparer<TClass<integer>>;
  assert(cmp.Compare(TClass&<integer>(o),TClass&<integer>(o)) = 0);
end.
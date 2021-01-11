begin
  var o: ICollection<integer> := Arr(1,2,3);
  var enm: IEnumerator<integer> := o.GetEnumerator;
  enm.MoveNext();
  assert(enm.Current = 1);
end.
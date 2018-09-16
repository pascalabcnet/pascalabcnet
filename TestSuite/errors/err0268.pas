type
  t1 = class end;
  
begin
  
  var ll := new LinkedList<t1>();
  
  foreach var o: LinkedListNode<t1> in ll do
    writeln(o.GetType)
  
end.
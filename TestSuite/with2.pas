begin
  with System do
  begin
    assert(Environment.CurrentDirectory = System.Environment.CurrentDirectory);
  end;
  with System.Environment do
  begin
    assert(CurrentDirectory = System.Environment.CurrentDirectory);
  end;
  with System.Runtime.InteropServices do
  begin
    var hnd: GCHandle;
    var hnd2: System.Runtime.InteropServices.GCHandle;
    var o: Expando.IExpando;
    assert(hnd.GetType = hnd2.GetType);
  end;
end.
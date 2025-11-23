begin
  with System do
  begin
    assert(Environment.CurrentDirectory = System.Environment.CurrentDirectory);
  end;
  with System.Environment do
  begin
    assert(CurrentDirectory = System.Environment.CurrentDirectory);
  end;
  with System.Runtime.InteropServices, System.Runtime.InteropServices.Expando do
  begin
    var hnd: GCHandle;
    var hnd2: System.Runtime.InteropServices.GCHandle;
    var o: IExpando;
    assert(hnd.GetType = hnd2.GetType);
  end;
end.
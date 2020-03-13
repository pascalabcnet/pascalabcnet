begin
  var cb: System.Action<System.IntPtr>;
  cb := i->exit();
  var cb1: System.Action<System.UIntPtr>;
  cb1 := i->exit();
end.
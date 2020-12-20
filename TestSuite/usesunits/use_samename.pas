uses u_samename;

function val_a := u_samename.val_a;

begin 
  assert(val_a() = System.IntPtr(0));
end.
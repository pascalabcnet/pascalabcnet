uses u_alwaysrestore; 
begin 
  assert(System.Type.GetType('u_alwaysrestore.MyClass') <> nil);
  assert(System.Type.GetType('u_alwaysrestore.u_alwaysrestore').GetMethod('myfunc') <> nil);
end.
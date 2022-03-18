//winonly
begin
  var t := System.Threading.Tasks.Task&<byte>.Run(()->byte(2));
  t.Wait();
  assert(t.Result = 2);
end.
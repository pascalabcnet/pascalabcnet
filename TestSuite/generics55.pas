begin
  var f: function: byte := ()->byte(2);
  var t := System.Threading.Tasks.Task&<byte>.Run(f);
  t.Wait();
  assert(t.Result = 2);
end.
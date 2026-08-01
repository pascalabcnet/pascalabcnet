begin
  var task := System.Threading.Tasks.Task&<integer>.Run(()->1);
  assert(task.Result = 1);
end.

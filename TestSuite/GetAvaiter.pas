uses System.Threading.Tasks;

begin
  var tt := new Task<integer>(()->1);
  var q := tt.GetAwaiter
end.
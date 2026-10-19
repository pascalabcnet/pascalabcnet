{$package Microsoft.Data.Sqlite 10.0.13}
uses Microsoft.Data.Sqlite;

begin
  var connection := new SqliteConnection('Data Source=:memory:');
  connection.Open;
  var command := connection.CreateCommand;
  command.CommandText := 'select 40 + 2';
  Println(System.Convert.ToInt32(command.ExecuteScalar));
  command.Dispose;
  connection.Dispose;
end.

begin
  Println('Process: ', System.Environment.ProcessPath);
  Println('Windows: ', System.OperatingSystem.IsWindows);
  var date := new System.DateOnly(2026, 8, 2);
  Println('DateOnly: ', date);
end.

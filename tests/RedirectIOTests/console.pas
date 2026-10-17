// TEST-REGRESSION: redirection must not load WinForms into a console program.
uses __RedirectIOMode, System;
begin
  assert(not AppDomain.CurrentDomain.GetAssemblies.Any(a -> a.GetName.Name = 'System.Windows.Forms'));
  Println('redirect-console-ok');
end.

// TEST-REGRESSION: preserve the common unhandled-exception IDE protocol.
uses __RedirectIOMode;
begin
  raise new System.Exception('redirect-unhandled-test');
end.

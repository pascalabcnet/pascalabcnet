unit u_namedisambigous1;
function fun: boolean;
begin
  Result := System.Globalization.UnicodeCategory.Format = System.Globalization.UnicodeCategory.ClosePunctuation;
end;

end.
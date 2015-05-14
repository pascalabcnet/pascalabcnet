begin
try
raise new System.Exception('Ha-ha');
finally
writeln(1);
end;
end.
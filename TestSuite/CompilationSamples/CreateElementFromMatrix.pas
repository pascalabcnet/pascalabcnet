{function GetElementFromMatrix(fname: string; i, j: integer): integer;
begin
  assert((fname <> '') and (i > 0) and (j > 0));
  try
    var f := OpenBinary&<integer>(fname);
    try
      var arr := f.Elements.ToArray;
      var size := round(sqrt(f.FileSize));
    finally
      f.Close;
    end;
  except
    writeln('File Error');
  end;
end;}

begin

end.
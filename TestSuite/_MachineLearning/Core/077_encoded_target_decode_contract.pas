uses MLABC;

procedure CheckInvalidCode(target: EncodedTarget; code: integer);
begin
  var raised := False;
  try
    var names := target.Decode([code]);
  except
    on e: System.ArgumentOutOfRangeException do raised := True;
  end;
  Assert(raised, 'Invalid class code must raise ArgumentOutOfRangeException');
end;

begin
  var df := DataFrame.FromCsvText('Breed'#10'Bulldog'#10'Spaniel');
  var target := df.EncodeTarget('Breed');
  Assert(target.Decode([1, 0, 1]).SequenceEqual(['Spaniel', 'Bulldog', 'Spaniel']));
  Assert(target.Decode(new integer[0]).Length = 0);
  CheckInvalidCode(target, -1);
  CheckInvalidCode(target, target.ClassNames.Length);
  Println('OK');
end.

uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

begin
  CheckRaises(procedure -> begin
    var pipe := UDataPipeline.Build(
      Arr($'X', $'X'),
      new KMeans(3, seed := 1)
    );
  end,
  'UDataPipeline.Build must reject duplicate features');
end.

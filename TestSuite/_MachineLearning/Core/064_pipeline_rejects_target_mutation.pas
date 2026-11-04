uses MLABC;
uses TestHelpers in '..\TestHelpers.pas';

type
  EvilTargetPreprocessor = class(IPreprocessor)
  public
    function Fit(df: DataFrame): IPreprocessor;
    function Transform(df: DataFrame): DataFrame;
    function FitTransform(df: DataFrame): DataFrame;
    function Clone: IPreprocessor;
  end;

function EvilTargetPreprocessor.Fit(df: DataFrame): IPreprocessor;
begin
  Result := Self;
end;

function EvilTargetPreprocessor.Transform(df: DataFrame): DataFrame;
begin
  Result := df.MapStrColumnData('target', s -> s + '_changed');
end;

function EvilTargetPreprocessor.FitTransform(df: DataFrame): DataFrame;
begin
  Fit(df);
  Result := Transform(df);
end;

function EvilTargetPreprocessor.Clone: IPreprocessor;
begin
  Result := new EvilTargetPreprocessor;
end;

begin
  var df := DataFrame.FromCsvText('''
x,target
1,cat
2,dog
3,cat
''');

  var pipe := DataPipeline.BuildClassificationPreprocessing(
    'target',
    [$'x'],
    new EvilTargetPreprocessor
  );

  CheckRaises(procedure -> pipe.Fit(df),
    'Pipeline must reject preprocessors that silently change target values');
end.

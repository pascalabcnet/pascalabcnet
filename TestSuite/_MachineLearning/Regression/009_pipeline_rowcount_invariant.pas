uses MLABC, PreprocessorABC;
uses TestHelpers in '..\TestHelpers.pas';

type
  BadDropper = class(IPreprocessor)
  public
    function Fit(df: DataFrame): IPreprocessor;
    function Transform(df: DataFrame): DataFrame;
    function FitTransform(df: DataFrame): DataFrame;
    function Clone: IPreprocessor;
  end;

function BadDropper.Fit(df: DataFrame): IPreprocessor;
begin
  Result := Self;
end;

function BadDropper.Transform(df: DataFrame): DataFrame;
begin
  Result := df.Head(df.RowCount - 1);
end;

function BadDropper.FitTransform(df: DataFrame): DataFrame;
begin
  Result := Transform(df);
end;

function BadDropper.Clone: IPreprocessor;
begin
  Result := new BadDropper;
end;

begin
  var ds := Datasets.Iris;
  var pipe := DataPipeline.Build(
    TaskKind.tkClassification,
    ds.Target,
    ds.Features,
    new BadDropper,
    new LogisticRegression
  );

  CheckRaises(procedure -> begin pipe.Fit(ds.Data); end,
    'Pipeline must reject DataFrame preprocessors that change RowCount');
end.

uses MLABC;

function SurvivalRate(df: DataFrame): real;
begin
  Result := df.Int('Выжил').Average;
end;

begin
  var ds := Datasets.TitanicRu;
  var df := ds.Data;

  Println('Доля выживших по полу:');
  foreach var sex in |'муж', 'жен'| do
  begin
    var sub := df.Filter(r -> r.Str('Пол') = sex);
    Println($'  {sex,-4}: {100 * SurvivalRate(sub):F1}%');
  end;

  Println;
  Println('Доля выживших по классу:');
  for var cls := 1 to 3 do
  begin
    var sub := df.Filter(r -> r.Int('Класс') = cls);
    Println($'  класс {cls}: {100 * SurvivalRate(sub):F1}%');
  end;
end.

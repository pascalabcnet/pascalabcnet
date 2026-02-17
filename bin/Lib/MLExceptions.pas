unit MLExceptions;

interface

type
  /// Базовое исключение ML-библиотеки
  MLException = class(Exception);

  /// Ошибка аргументов ML-методов
  MLArgumentException = class(System.ArgumentException);

  /// Попытка использования необученной модели/пайплайна
  MLNotFittedException = class(MLException);

  /// Несоответствие размерностей
  MLDimensionException = class(MLException);

procedure Error(msg: string; params args: array of object);
procedure ArgumentError(msg: string; params args: array of object);
procedure NotFittedError(msg: string; params args: array of object);
procedure DimensionError(msg: string; params args: array of object);

implementation

function FormatSafe(msg: string; args: array of object): string;
begin
  var text := GetTranslation(msg);
  try
    Result := Format(text, args);
  except
    Result := text; // если не совпали {0},{1} — оставляем как есть
  end;
end;

procedure Error(msg: string; params args: array of object);
begin
  raise new MLException(FormatSafe(msg, args));
end;

procedure ArgumentError(msg: string; params args: array of object);
begin
  raise new MLArgumentException(FormatSafe(msg, args));
end;

procedure NotFittedError(msg: string; params args: array of object);
begin
  raise new MLNotFittedException(FormatSafe(msg, args));
end;

procedure DimensionError(msg: string; params args: array of object);
begin
  raise new MLDimensionException(FormatSafe(msg, args));
end;

end.

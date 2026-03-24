unit MLCoreABC;

interface

uses LinearAlgebraML;

type
/// Базовый интерфейс шага конвейера машинного обучения.
/// Используется для объединения всех типов шагов
/// (DataFrame-преобразователей, матричных трансформеров и моделей)
/// в единую последовательность выполнения внутри различных Pipeline
  IPipelineStep = interface end;
  
/// Интерфейс шага конвейера, работающего в числовом (матричном) пространстве.
/// Реализуется трансформерами и моделями, которые принимают на вход Matrix и Vector
/// и используются в Matrix-уровне Pipeline
  IMatrixStep = interface(IPipelineStep) 
  end;
  
/// Интерфейс шага конвейера, работающего на уровне DataFrame.
/// Реализуется препроцессорами, которые выполняют преобразования табличных данных
///   до перехода в числовое (Matrix) представление
  IDataStep = interface(IPipelineStep) 
  end;

  /// Базовый интерфейс модели машинного обучения
  IModel = interface(IMatrixStep)
    function Predict(X: Matrix): Vector;
    function Clone: IModel;
    function Name: string;
    property IsFitted: boolean read;
  end;
  
  /// Интерфейс модели с учителем (Supervised Model).
  /// Наследуется от базового интерфейса IModel.
  /// Предназначен для алгоритмов, обучающихся по признакам X
  /// с использованием целевых значений (y)
  ISupervisedModel = interface(IModel)
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
  end;
  
  /// Интерфейс модели без учителя (Unsupervised Model).
  /// Наследуется от базового интерфейса IModel.
  /// Предназначен для алгоритмов, обучающихся только по признакам X
  /// без использования целевых значений (y)
  IUnsupervisedModel = interface(IModel)
    function Fit(X: Matrix): IUnsupervisedModel;
  end;

  /// Интерфейс алгоритма кластеризации.
  /// Наследуется от IUnsupervisedModel.
  /// Предназначен для моделей, разбивающих объекты на кластеры
  /// и возвращающих индекс кластера для каждого объекта
  IClusterer = interface(IUnsupervisedModel)
    function PredictLabels(X: Matrix): array of integer;
  end;
  
  /// Интерфейс древовидной модели машинного обучения
  ITreeModel = interface(ISupervisedModel)
    function FeatureImportances: Vector;
  end;

  /// Интерфейс классификатора.
  /// Наследуется от IModel.
  /// Предназначен для моделей, выполняющих классификацию (предсказание меток классов).
  IClassifier = interface(ISupervisedModel)
    /// Возвращает метки классов
    function PredictLabels(X: Matrix): array of integer;
  end;
  
  /// Интерфейс классификатора, возвращающего вероятности.
  /// Расширяет IClassifier.
  /// Используется для моделей, которые умеют оценивать
  /// вероятность принадлежности к классу.
  /// Позволяет получать значения в диапазоне (0, 1)
  /// вместо только итогового решения.
  IProbabilisticClassifier = interface(IClassifier)
    /// Возвращает матрицу вероятностей размера (nSamples × nClasses).
    /// Столбцы соответствуют классам в порядке внутреннего кодирования модели
    function PredictProba(X: Matrix): Matrix;
    
    /// Возвращает массив меток классов в порядке столбцов PredictProba.
    function GetClasses: array of real;
  end;

  /// Интерфейс регрессионной модели.
  /// Наследуется от IModel.
  /// Предназначен для моделей, предсказывающих числовые значения.
  IRegressor = interface(ISupervisedModel)
  end;
  
  /// Базовый интерфейс преобразования признаков.
  /// Используется для масштабирования, отбора,
  /// уменьшения размерности и других преобразований данных
  ITransformer = interface(IMatrixStep)
    /// Применяет обученное преобразование к данным.
    /// Возвращает новую матрицу признаков.
    function Transform(X: Matrix): Matrix;
    function Clone: ITransformer;
  end;
  
  /// Интерфейс преобразования признаков с учётом целевой переменной.
  /// Используется для методов отбора признаков и других процедур,
  /// в которых при обучении требуется вектор целевых значений
  ISupervisedTransformer = interface(ITransformer)
    /// Обучает преобразование на данных с использованием
    /// как признаков X, так и целевой переменной y.
    /// Запоминает необходимые параметры, которые будут использоваться при Transform.
    function Fit(X: Matrix; y: Vector): ISupervisedTransformer;
  end;
  
  /// Интерфейс преобразования признаков без учёта целевой переменной.
  /// Используется для методов отбора признаков и других процедур,
  /// в которых при обучении не требуется вектор целевых значений
  IUnsupervisedTransformer = interface(ITransformer)
    /// Обучает преобразование на данных с использованием признаков X
    /// Запоминает необходимые параметры, которые будут использоваться при Transform.
    function Fit(X: Matrix): IUnsupervisedTransformer;
  end;
  
implementation  

end.
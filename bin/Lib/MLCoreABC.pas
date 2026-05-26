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

  /// Интерфейс шага конвейера, привязанного к одной колонке DataFrame.
  /// Используется для препроцессоров, выполняющих преобразование конкретной колонки
  /// (например, кодирование категориальных признаков).
  /// Позволяет DataPipeline централизованно контролировать операции над колонками
  /// (в частности, запрещать изменение целевой переменной).  
  IColumnBoundStep = interface
    /// Имя колонки, к которой применяется преобразование.
    /// Должно соответствовать существующей колонке DataFrame.
    /// Используется для валидации (например, запрет преобразования target).
    property ColumnName: string read;
  end;
  
  /// Интерфейс шага конвейера, работающего с несколькими колонками DataFrame.
  /// Используется для препроцессоров, выполняющих преобразования над набором колонок
  /// (например, заполнение пропусков, масштабирование или кодирование нескольких признаков).
  /// Позволяет DataPipeline контролировать операции над колонками
  /// (в частности, предотвращать изменение целевой переменной).
  IColumnsBoundStep = interface
    /// Список колонок, к которым применяется преобразование.
    /// Каждое имя должно соответствовать существующей колонке DataFrame.
    /// Используется для валидации (например, запрет преобразования target).
    property Columns: array of string read;
  end;

  /// Базовый интерфейс модели машинного обучения
  IModel = interface(IMatrixStep)
    function Clone: IModel;
    function Name: string;
    property IsFitted: boolean read;
  end;
  
  /// Модель, реализующая отображение X → y (или аналогичный результат).
  /// Поддерживает предсказание для новых данных
  IPredictiveModel = interface(IModel)
    function Predict(X: Matrix): Vector;
  end;
  
  /// Интерфейс модели с учителем (Supervised Model).
  /// Наследуется от базового интерфейса IModel.
  /// Предназначен для алгоритмов, обучающихся по признакам X
  /// с использованием целевых значений (y)
  ISupervisedModel = interface(IPredictiveModel)
    function Fit(X: Matrix; y: Vector): ISupervisedModel;
  end;
  
  /// Интерфейс модели без учителя (Unsupervised Model).
  /// Наследуется от базового интерфейса IModel.
  /// Предназначен для алгоритмов, обучающихся только по признакам X
  /// без использования целевых значений (y)
  IUnsupervisedModel = interface(IModel)
    function Fit(X: Matrix): IUnsupervisedModel;
  end;

  /// Интерфейс алгоритма кластеризации (unsupervised learning).
  /// Предназначен для моделей, разбивающих объекты на кластеры
  /// без использования целевой переменной.
  /// Основной сценарий использования — FitPredict, возвращающий
  /// индекс кластера для каждого объекта обучающей выборки
  IClusterer = interface(IUnsupervisedModel)
    // function PredictLabels(X: Matrix): array of integer;
    
    /// Выполняет кластеризацию данных и возвращает массив индексов кластеров
    /// (0,1,2,...) для каждого объекта. Значение -1 может использоваться
    /// для обозначения шума (например, в DBSCAN).
    function FitPredict(X: Matrix): array of integer;
  end;
  
  /// Интерфейс кластеризатора, поддерживающего предсказание для новых данных.
  /// Используется для алгоритмов, задающих явное отображение X → cluster
  /// (например, KMeans через ближайший центр кластера)
  IPredictiveClusterer = interface(IClusterer, IPredictiveModel)
    /// Возвращает индекс кластера для каждого объекта из X
    /// без повторного обучения модели.
    /// Требует предварительного вызова Fit или FitPredict.
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
      /// Возвращает внутренние индексы классов (0,1,2,...).
      /// Индекс i соответствует метке GetClassLabels[i].
      function PredictLabels(X: Matrix): array of integer;
      
      /// Возвращает исходные метки классов в порядке внутреннего кодирования.
      function GetClassLabels: array of string;
    end;
  
  IClassifierInternal = interface
    procedure SetClassLabels(classes: array of string);
  end;
  
  /// Интерфейс классификатора, возвращающего вероятности.
  /// Расширяет IClassifier.
  /// Используется для моделей, которые умеют оценивать
  /// вероятность принадлежности к классу.
  /// Позволяет получать значения в диапазоне (0, 1)
  /// вместо только итогового решения.
  IProbabilisticClassifier = interface(IClassifier)
    /// Возвращает матрицу вероятностей классов для всех объектов из X.
    /// Размер результата: nSamples × nClasses, где:
    /// - nSamples — число объектов в X;
    /// - nClasses — число классов модели.
    /// Элемент [i, k] содержит вероятность того, что объект i принадлежит классу k.
    /// Сумма вероятностей в каждой строке равна 1.
    function PredictProba(X: Matrix): Matrix;
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
    function FitTransform(X: Matrix; y: Vector): Matrix;
  end;
  
  /// Интерфейс преобразования признаков без учёта целевой переменной.
  /// Используется для методов отбора признаков и других процедур,
  /// в которых при обучении не требуется вектор целевых значений
  IUnsupervisedTransformer = interface(ITransformer)
    /// Обучает преобразование на данных с использованием признаков X
    /// Запоминает необходимые параметры, которые будут использоваться при Transform.
    function Fit(X: Matrix): IUnsupervisedTransformer;
    function FitTransform(X: Matrix): Matrix;
  end;
  
  IColumnExpander = interface
    function GetExpandedColumns(sourceColumn: string): array of string;
  end;
  
implementation  

end.

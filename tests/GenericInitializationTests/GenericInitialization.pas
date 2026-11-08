type
  SampleRecord = record
    Text: string;
  end;

  Box<T> = class
  public
    Value: T;
    static Shared: T;
    ExplicitValue: T := default(T);

    function ReadLocal: T;
    begin
      var item: T;
      Result := item;
    end;

    function ReadResult: T;
    begin
    end;

    function NewArray: array of T := new T[1];

    function ResizeArray: array of T;
    begin
      var items: array of T;
      System.Array.Resize(items, 1);
      Result := items;
    end;
  end;

  AnotherBox<T> = class
  public
    Value: T;
  end;

procedure Check(condition: boolean; message: string);
begin
  if not condition then
    raise new System.Exception(message);
end;

begin
  var integers := new Box<integer>;
  Check(integers.Value = 0, 'integer field');
  Check(integers.ReadLocal = 0, 'integer local');

  var strings := new Box<string>;
  Check(strings.Value = '', 'string field');
  Check(Box&<string>.Shared = '', 'string static field');
  Check(strings.ReadLocal = '', 'string local');
  Check(strings.ReadResult = '', 'string result');
  Check(System.Object.ReferenceEquals(strings.ExplicitValue, nil), 'explicit default');
  Check(strings.NewArray[0] = '', 'new T[n] initializes string elements');
  Check(strings.ResizeArray[0] = '', 'new element after resizing');

  var records := new Box<SampleRecord>;
  Check(records.Value.Text = '', 'record field');
  var localRecord := records.ReadLocal;
  Check(localRecord.Text = '', 'record local');
  var resultRecord := records.ReadResult;
  Check(resultRecord.Text = '', 'record result');
  var resizedRecords := records.ResizeArray;
  Check(resizedRecords[0].Text = '', 'record after resizing');

  var another := new AnotherBox<string>;
  Check(another.Value = '', 'second generic class');
end.

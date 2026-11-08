uses GenericArrayResizeUnit;

type StringVector = array of string;

var evaluationTrace := 0;

function PickSlot: integer;
begin
  evaluationTrace := 10 * evaluationTrace + 1;
  Result := 0;
end;

function RequestedSize: integer;
begin
  evaluationTrace := 10 * evaluationTrace + 2;
  Result := 3;
end;

procedure Grow<T>(var items: array of T; n: integer);
begin
  System.Array.Resize(items, n);
end;

procedure GrowSlot<T>(var slots: array of array of T);
begin
  System.Array.Resize(slots[PickSlot], RequestedSize);
end;

procedure Check(condition: boolean; message: string);
begin
  if not condition then
    raise new System.Exception(message);
end;

procedure CheckStrings(items: array of string);
begin
  Check((items.Length = 4) and (items[0] = 'keep') and System.Object.ReferenceEquals(items[1], nil),
    'existing strings, including nil, are preserved');
  Check((items[2] = '') and (items[3] = ''), 'new strings are initialized');
end;

procedure CheckRecords(items: array of ResizeEntry);
begin
  Check((items.Length = 4) and (items[0].Text = 'keep') and (items[0].Count = 17) and
    System.Object.ReferenceEquals(items[1].Text, nil), 'existing records are preserved');
  Check((items[2].Text = '') and (items[3].Text = '') and (items[3].Count = 0),
    'new records are initialized');
end;

begin
  var strings := new string[2]('keep', nil);
  Grow(strings, 4);
  CheckStrings(strings);
  Grow(strings, 2);
  Check((strings.Length = 2) and System.Object.ReferenceEquals(strings[1], nil), 'shrink preserves elements');
  Grow(strings, 2);
  Check(System.Object.ReferenceEquals(strings[1], nil), 'same size preserves elements');

  var sample: ResizeEntry;
  sample.Text := 'keep';
  sample.Count := 17;
  var records := new ResizeEntry[2](sample, default(ResizeEntry));
  Grow(records, 4);
  CheckRecords(records);

  var stringResizer := new ModuleResizer<string>;
  strings := new string[2]('keep', nil);
  stringResizer.Grow(strings, 4);
  CheckStrings(strings);
  strings := nil;
  stringResizer.Grow(strings, 1);
  Check((strings.Length = 1) and (strings[0] = ''), 'growing from nil');

  var recordResizer := new ModuleResizer<ResizeEntry>;
  records := new ResizeEntry[2](sample, default(ResizeEntry));
  recordResizer.Grow(records, 4);
  CheckRecords(records);

  var slots := new StringVector[1];
  slots[0] := new string[1]('slot');
  GrowSlot(slots);
  Check(evaluationTrace = 12, 'array address and size evaluated once in order');
  Check((slots[0][0] = 'slot') and (slots[0][1] = '') and (slots[0][2] = ''), 'indexed array is resized');

  var integers := new integer[1](42);
  Grow(integers, 2);
  Check((integers[0] = 42) and (integers[1] = 0), 'unrelated element types retain their defaults');
end.

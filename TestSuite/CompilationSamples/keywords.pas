type
  T = class
    public static procedure &Abstract := exit;
    public static procedure &Operator := exit;
    public static procedure &Partial := exit;
    public static procedure &Lock := exit;
    public static procedure &Implicit := exit;
    public static procedure &Explicit := exit;
    public static procedure &On := exit;
    public static procedure &Overload := exit;
    public static procedure &Reintroduce := exit;
    public static procedure &Override := exit;
    public static procedure &Extensionmethod := exit;
    public static procedure &Virtual := exit;
    public static procedure &Forward := exit;
    public static procedure &Loop := exit;
  end;

begin
  T.Abstract();         //Program2.pas(20) : Встречено 'Abstract', а ожидался оператор
  T.Partial();          //Program2.pas(22) : Встречено 'Partial', а ожидался оператор
  T.Lock();             //Program2.pas(23) : Встречено 'Lock', а ожидался оператор
  T.Implicit();         //Program2.pas(24) : Встречено 'Implicit', а ожидался оператор
  T.Explicit();         //Program2.pas(25) : Встречено 'Explicit', а ожидался оператор
  T.On();               //Program2.pas(26) : Встречено 'On', а ожидался оператор
  T.Overload();         //Program2.pas(27) : Встречено 'Overload', а ожидался оператор
  T.Reintroduce();      //Program2.pas(28) : Встречено 'Reintroduce', а ожидался оператор
  T.Override();         //Program2.pas(29) : Встречено 'Override', а ожидался оператор
  T.Extensionmethod();  //Program2.pas(30) : Встречено 'Extensionmethod', а ожидался оператор
  T.Virtual();          //Program2.pas(31) : Встречено 'Virtual', а ожидался оператор
  T.Forward();          //Program2.pas(32) : Встречено 'Forward', а ожидался оператор
  T.Loop();             //Program2.pas(33) : Встречено 'Loop', а ожидался оператор
end.
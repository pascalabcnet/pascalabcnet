type
  GenericBase<T> = class end;
  
  GenericObject<T> = class end;
  
  PartialContainer<T> = partial class end;
  
// Можно сделать пред-описанием метода, чтоб обойтись без forward
procedure p1<T>(o: PartialContainer<T>); forward;

type
  // Обязательно описать предка после описания p1
  PartialContainer<T> = partial class(GenericBase<GenericObject<T>>) end;
  
procedure p0<T>(o: GenericBase<GenericObject<T>>) := exit;

procedure p1<T>(o: PartialContainer<T>);
begin
  p0(o);
end;

begin
  p1&<byte>(nil);
end.
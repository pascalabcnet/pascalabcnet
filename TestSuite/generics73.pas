type
  GenericBase<T> = class end;
  
  GenericObject<T> = class end;
  
  PartialContainer<T> = partial class end;
  
  GPUCommandContainer<T> = class(GenericBase<T>) end;
  
// Можно сделать пред-описанием метода, чтоб обойтись без forward
procedure p1<T>(o: PartialContainer<T>); where T: record; forward;

type
  // Обязательно описать предка после описания p1
  PartialContainer<T> = partial class(GPUCommandContainer<GenericObject<T>>) end;
  
procedure p0<T>(o: GenericBase<GenericObject<T>>); where T: record;
begin end;

procedure p1<T>(o: PartialContainer<T>); where T: record;
begin
  //Ошибка: Невозможно инстанцировать, так как тип T не является размерным
  p0(o);
end;

begin 
   p1(new PartialContainer<integer>());
end.
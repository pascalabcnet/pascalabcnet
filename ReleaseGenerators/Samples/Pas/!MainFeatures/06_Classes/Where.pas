// Секция Where - ограничение на типы параметров
uses System,System.Collections.Generic;

type
   MyClass<T,T1> = class 
   where T: System.Array,ICloneable;
   where T1: constructor;
     procedure p(obj1: T; var obj2: T1);
     begin
       obj1.Clone();
       obj2 := new T1;
     end;
   end;
   IntArr = array of integer;
   
var 
  m: MyClass<IntArr,integer>;
  //m1: MyClass<integer>; // ошибка

begin
end.
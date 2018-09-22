type
  TClass = static class
    class procedure Test;
    begin
      
    end;
  end;

begin
  TClass{@static class TClass@}.Test();
  System.Math{@static class System.Math@}.Sin(2);
end.
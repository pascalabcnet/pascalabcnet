///Модуль для рисования фракталов семейства "Лист папоротника"
unit Paporotnik;

uses GraphABC;

type

  ///Настройки фрактала
  PaporotnikFractalInitalData = record
    data: array of array of real; 
    P0,P1,P2,P3:real;
  end;
  
  ///Фрактал "Лист папоротника"  
  PaporotnikFractal = class
  private
    data: array of array of real; 
    P0,P1,P2,P3:real;
  public
    constructor(initdata: PaporotnikFractalInitalData);
    begin
      data := initdata.data;
      P0 := initdata.P0;
      P1 := initdata.P1;
      P2 := initdata.P2;
      P3 := initdata.P3;
    end;
    procedure Draw(x0,y0,Iterations,Height,Brightness: integer; fast: boolean);
    begin
      var plotx, ploty, x, y : real;
      var Size := Height/11;
      var Width := Height div 2;
      var dx := Width div 2;
	    var dc := Iterations div Brightness;
      if fast then
        LockDrawing;
      for var i:=1 to Iterations do begin
        var P := Random(100);    
        var rnd := P<P0 ? 0 : P<P1+P0 ? 1 : P<P2+P1+P0 ? 2 : 3;
        plotx := data[rnd,0]*x + data[rnd,1]*y;
        ploty := data[rnd,2]*x + data[rnd,3]*y + data[rnd,5];
        x := plotx;
        y := ploty;
        SetPixel(x0+Round(x*Size) + dx, y0+Height - Round(y*Size), GreenColor(byte(30 + (i div dc))));
      end;
      if fast then
        UnlockDrawing;      
    end;
  end;

end.
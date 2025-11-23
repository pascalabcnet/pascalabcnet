var s: string;

type
  IWindow = interface
    procedure Menu;
  end;
  IRestaurant = interface
    procedure Menu;
  end;
  RestaurantSystem = class(IWindow,IRestaurant)
  public
    procedure Menu; virtual;
    begin
      s := 'RestaurantSystem.Menu';
    end;
    procedure IWindow.Menu;
    begin
      s := 'IWindow.Menu';
    end;
    procedure IRestaurant.Menu;
    begin
      s := 'IRestaurant.Menu';
    end;
  end;
  
begin
  var r := new RestaurantSystem;
  r.Menu;
  assert(s = 'RestaurantSystem.Menu');
  IWindow(r).Menu;
  assert(s = 'IWindow.Menu');
  IRestaurant(r).Menu;
  assert(s = 'IRestaurant.Menu');
end.
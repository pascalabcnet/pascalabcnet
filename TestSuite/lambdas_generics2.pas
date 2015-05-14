uses System, System.Collections.Generic, System.Linq;

type Customer = class
    public
      Code: integer;
      Name: string;
    constructor (Code: integer; Name: string);
    begin
      self.Code := Code;
      self.Name := name;
    end;
end;

type Order = class
    public
      KeyCode: integer;
      Product: string;
    constructor (KeyCode: integer; Product: string);
    begin
      self.KeyCode := KeyCode;
      self.Product := Product;
    end;
end;

type Res1 = class
    public
      Name: string;
      Collection: IEnumerable<Order>;
    constructor (Name: string; Collection: IEnumerable<Order>);
    begin
      self.Name := Name;
      self.Collection := Collection;
    end;
end;

begin 
  var customers := new List<Customer>;
  customers.Add(new Customer(5, 'Sam'));
  customers.Add(new Customer(6, 'Dave'));
  customers.Add(new Customer(7, 'Julia'));
  customers.Add(new Customer(8, 'Sue'));
  
  var orders := new List<Order>;
  orders.Add(new Order(5, 'Book'));
  orders.Add(new Order(6, 'Game'));
  orders.Add(new Order(7, 'Computer'));
  orders.Add(new Order(7, 'Mouse'));
  orders.Add(new Order(8, 'Shirt'));
  orders.Add(new Order(5, 'Underwear'));
  
  var query := customers.GroupJoin(orders,
	    c -> c.Code,
	    o -> o.KeyCode,
	    (c; res) -> new Res1(c.Name, res));
	
	var t := query.Select(x -> begin 
	                             result := x.Name + ' bought: ';
	                             var bought := '';
	                             foreach var item in x.Collection do
		                             bought += item.Product + '   ';
		                           result += bought;
	                           end);
	                           
	foreach var res in t do
	begin
	  writeln(res);
	end;
end.
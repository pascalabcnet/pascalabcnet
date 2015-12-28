unit
  List;

interface

uses
  System;


type
  ListNode<T> = template class
  public
    info: T;
    left, right: ListNode<T>;
    constructor(data: T);
    begin
      info := data;
      left := nil;
      right := nil;
    end;
  end;
  
  ListClass<T> = template class
  public
    start, last: ListNode<T>;
    constructor;
//    var
//      p: ListNode<T>;
    begin
      start := nil;
      last := nil;
    end;
    function IsEmpty: boolean;
    begin
      result := start = nil;
    end;
    procedure PushFront(data: T);
    var
      NewNode: ListNode<T>;
    begin
      NewNode := new ListNode<T>(data);
      if start = nil then
      begin
        start := NewNode;
        last := NewNode;
      end
      else
      begin
        NewNode.right := start;
        start.left := NewNode;
        start := NewNode;
      end;
    end;
    procedure PushBack(data: T);
    var
      NewNode: ListNode<T>;
    begin
      NewNode := new ListNode<T>(data);
      if start = nil then
      begin
        start := NewNode;
        last := NewNode;
      end
      else
      begin
        NewNode.left := last;
        last.right := NewNode;
        last := NewNode;
      end;
    end;
    function PopFront: T;
    begin
      if IsEmpty then
        result := nil //Любой тип может принять такое значение, не правда ли?
                      //Вообще-то здесь нужно выбросить исключение.
      else
      begin
        result := start.info;
        start := start.right;
        if start = nil then
          last := nil
        else
          start.left := nil;
      end;
    end;
    function PopBack: T;
    begin
      if IsEmpty then
        result := nil //Любой тип может принять такое значение, не правда ли?
                      //Вообще-то здесь нужно выбросить исключение.
      else
      begin
        result := last.info;
        last := last.left;
        if last = nil then
          start := nil
        else
          last.right := nil;
      end;
    end;
    procedure PrintAll;
    var
      tek: ListNode<T>;
    begin
      tek := start;
      while tek <> nil do
      begin
        Console.WriteLine(tek.info);
        tek := tek.right;
      end;
    end;
    function MaxElem: ListNode<T>;
    var
      tek: ListNode<T>;
    begin
      if IsEmpty then
        result := nil
      else
      begin
        result := start;
        tek := start.right;
        while tek <> nil do
        begin
          if result.info < tek.info then
            result := tek;
          tek := tek.right;
        end;
      end;
    end;
    procedure Clear;
    begin
      //Чисто не там, где убирают, а там, где мусор убирается сам!
      start := nil; 
      last := nil;
    end;
    procedure Sort; //выбором - по неубыванию
    var
      x: T;
      tek, p, min: ListNode<T>;
    begin
      if not IsEmpty then
      begin
        tek := start;
        while tek.right <> nil do
        begin
          min := tek;
          p := tek.right;
          while p <> nil do
          begin
            if p.info < min.info then
              min := p;
            p := p.right;
          end;
          x := tek.info;
          tek.info := min.info;
          min.info := x;
          tek := tek.right;
        end;
      end;
    end;
  end;

implementation

end.
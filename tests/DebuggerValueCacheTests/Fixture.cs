using System;
class Node { public Node Condition, ThenStat, ElseStat; public int Number=42; public override string ToString() { GC.Collect();return "Node"; } }
class Fixture { public static Node Root; static void Main() { Root=new Node {Condition=new Node(),ThenStat=new Node(),ElseStat=new Node()}; System.Diagnostics.Debugger.Break(); Console.WriteLine(Root.Number); } }

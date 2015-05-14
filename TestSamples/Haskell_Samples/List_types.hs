module Main where

func (a:b:c) = do {
                   (print a);
                }
            
func1 = [1.2,1,3.5]

func2 = ['1','2','m']

func3 = ["b","n","v"]

func4 = [1,0,0]

f = True

main = do {
            (func ['b','n','v','8','6']);
            (print 5.2);
            (print func1);
            (print func2);
            (print func3);
            --print true;
          }
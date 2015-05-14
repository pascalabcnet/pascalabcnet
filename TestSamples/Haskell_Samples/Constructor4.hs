module Main where

zip []   []   = []
zip x:xs y:ys = (x,y) : (zip xs ys)

tail []         =  "Prelude.tail: пустой список"
tail x:xs       =  xs

fib = 1 : 1 : [ (a+b) | (a,b) <- (zip fib (tail fib)) ]    

fib1 = 1 : 1 : [ (a+b) | (a,b) <- (zip [1,1] [1,2])]

fib2 = 1 : 1 : [ (a+b) | (a,b) <- (zip [1,1,2] [1,2,3])]

fib3 = 1 : 1 : [ (a+b) | (a,b) <- (zip [1,1,2,3] [1,2,3,5])]

fib4 = 1 : 1 : [ (a+b) | (a,b) <- (zip [1,1,2,3,5] [1,2,3,5,8])]

fib5 = 1 : 1 : [ (a+b) | (a,b) <- (zip [1,1,2,3,5,8] [1,2,3,5,8,13])]

main = do { 
              (print(zip [1,1] [1,2]));
              (print fib1);
              (print fib2);
              (print fib3);
              (print fib4);
              (print fib5);
          }
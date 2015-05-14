module Main where

g = [ (x+y) | x <- [2.5,3,4], y <- [1,2,3,4,5,6,7,8,9], x<5, y>5]

f = [ x | x <- [[1,2],[2,3],[3,4]]]

lenght [] = 0
lenght h:t = (lenght t) + 1

h = [ (x+y) | x <- [2.5,3,4], y <- [1,2,3,4,5,6,7,8,9], x<5, y>(lenght [1,2,3,4,5])]

q = [(x,y) | x <- [1,2,3], y <- [4,5,6]]

r = [ (x+y+z) | x<-[1,2], y<-[3,4], z<-[5,6]]

main = do { 
              (print g);
              (print f);
              (print(lenght h));
              (print h);
              (print q);
              (print r);
          }
module Main where

g = ("321",2,('3',[5]))

main = do {
              (print g);
              (print ("321",[[2],[[5],[4],[6]]],('3',[5])));
          }
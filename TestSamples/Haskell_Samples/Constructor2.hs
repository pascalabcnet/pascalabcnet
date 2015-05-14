module Main where

r = [ (x+y+z) | (x,y,z)<-[(1,2,5),(3,4,6)]]

main = do { 
              (print r);
          }
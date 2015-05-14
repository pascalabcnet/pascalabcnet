module Main where

r = [ (x+y+z+x1+z1) | ((x,x1),y,(z,z1))<-[((1,11),2,(5,55)),((3,33),4,(6,66))], z1<66]

main = do { 
              (print r);
          }
module Main where

func x = if (x<3) then 
                    do {
                          (print x+z);
                       } 
                  else 
                    do {
                          (print "Hello!");
                       }
where z=123

main = do { 
              (func 1);
              (func 10);
          }
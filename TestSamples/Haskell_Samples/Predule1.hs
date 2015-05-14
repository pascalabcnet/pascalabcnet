module Main where

eq x y = x==y

noteq x y = not(x==y)

max x y  | x <= y    =  y
max x y  | not(x <= y)     =  x
         
min x y  | x <= y    =  x
min x y  | x > y     =  y

succ x = x+1

pred x = x-1

condition x = x>4

until p f x | (p x)       =  x
until p f x | otherwise  =  (until p f (f x))

map f []     = []
map f x:xs = (f x) : (map f xs)

filter p []                       = []
filter p x:xs | (p x)              = x : (filter p xs)
filter p x:xs | otherwise  = (filter p xs)

head []         =  "Prelude.head: пустой список"
head x:xs       =  x

tail []         =  "Prelude.tail: пустой список"
tail (x:xs)       =  xs

length [] = 0
length (h:t) = (length t) + 1

init []           =  "Prelude.init: пустой список"
init x | (length x)==1       =  []
init x | otherwise       =  (head x) : (init(tail x))

f x y = x+y

foldr a x [] = x
foldr a x h:t = (a h (foldr a x t))

foldl a z []     =  z
foldl a z x:xs =  (foldl a (a z x) xs)

take n _      | n <= 0 =  []
take _ []              =  []
take n x:xs          =  x:(take n-1 xs)

drop n xs     | n <= 0 =  xs
drop _ []              =  []
drop n x:xs           =  (drop n-1 xs)

splitAt n xs             =  ((take n xs), (drop n xs))

[] `plusplus` ys   = ys
x:xs `plusplus` ys = x:(xs `plusplus` ys)

main = do {
              (print(eq 2 2));
              (print(noteq 2 2));
              (print(min 8 3));
              (print(max 8 3));
              (print(until condition succ 1)); 
              (print(map succ [1,2,3,4,5]));
              (print(filter condition [10,1,2,5,8,3,9]));
              (print(head [7,4,1]));
              (print(head []));
              (print(tail [7,4,1]));
              (print(tail []));
              (print(init []));
              (print(init [1]));
              (print(init [1,2,3,4,5]));
              (print(foldr f 2 [1,3..10]));
              (print(foldl f 5 [1,3..10]));
              (print(splitAt 10 [1,2,3,4,5,6,4,5,6,7,8]));
              (print((([1,2] `plusplus` [3,4]) `plusplus` [5])));
              (print(condition 5));
              (print(not (condition 5)));
          }
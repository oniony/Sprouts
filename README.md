![S·P·R·O·U·T·S](https://raw.githubusercontent.com/oniony/Sprouts/master/Graphics/Sprouts.png)

What is Sprouts
===============

Oniony Sprouts is a collection of convenient .NET libraries for facilities
not already present in the .NET core libraries.

Each library provides a distinct capability that can be included in a
project individually.

Modules
=======

Pattern Matching
----------------

[Pattern Matching](https://github.com/oniony/Sprouts/blob/master/Modules/PatternMatching/README.md) allows flow control based upon the type and content of objects.
It can be used to replace complex switch statements, overloaded methods and dynamic
invocations for more readable, rationalizable code.

    string description = PatternMatch(node).Returns<string>
                                           .Case<FruitNode>(fruit => $"A {fruit.Name} fruit")
                                           .Case<AnimalNode>(animal => animal.Size == Size.Large, $"A large {animal.Name} animal")
                                           .Default(node => "Something else");

- - - 

© 2016 Paul Ruane

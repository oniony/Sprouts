![S·P·R·O·U·T·S](https://raw.githubusercontent.com/oniony/Sprouts/master/Graphics/Sprouts.png)

What is Sprouts
===============

Oniony Sprouts is a collection of convenient .NET libraries for facilities
not already present in the .NET core libraries.

Each library provides a distinct capability that can be included in a
project individually.

Libraries
=========

Pattern Matching
----------------

Provides a simple pattern matching facility, to allow perform functions
based upon the type or properties of an object. It can be used to replace switch statements
to hide the casting, or to replace dynamic method invocation with a form that is both easier
to read and debug.

    TreeNode node = ...
    string text = node.Match().Returns<string>.Case<FruitNode>(fruit => $"A {fruit.Name} fruit")
                                              .Case<AnimalNode>(animal => animal.Size == Size.Large, $"A large {animal.Name} animal");

- - - 

© 2016 Paul Ruane

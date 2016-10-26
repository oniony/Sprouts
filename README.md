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

Provides a simple pattern matching facility, to allow functions or actions to be performed
based upon the type or properties of an object. It can be used to replace switch statements
in order to hide the casting, or to replace dynamic method invocation with a form that is both easier
to read and debug.

Code of the like:

    TreeNode node = ...
    string description = "Something else";
    if (node is FruitNode)
    {
        FruitNode fruit = (FruitNode) node;
        description = $"A {fruit.Name} fruit";
    }
    else if (node is AnimalNode)
    {
        AnimalNode animal = (AnimalNode) node;
        if (animal.Size == Size.Large)
        {
            description = $"A large {animal.Name} animal";
        }
    }

Can be written using Pattern Matching as:

    TreeNode node = ...
    string description = node.Match()
                             .Returns<string>
                             .Case<FruitNode>(fruit => $"A {fruit.Name} fruit")
                             .Case<AnimalNode>(animal => animal.Size == Size.Large, $"A large {animal.Name} animal")
                             .Default(node => "Something else");

- - - 

© 2016 Paul Ruane

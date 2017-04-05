![S·P·R·O·U·T·S](https://raw.githubusercontent.com/oniony/Sprouts/master/Graphics/Sprouts.png)

Pattern Matching
================

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

    using static Oniony.Sprouts.Core.PatternMatching.PatternMatching;
    ...
    string description = PatternMatch(node).Returns<string>
                                           .Case<FruitNode>(fruit => $"A {fruit.Name} fruit")
                                           .Case<AnimalNode>(animal => animal.Size == Size.Large, $"A large {animal.Name} animal")
                                           .Default(node => "Something else");

(If you do not wish to use the static import, then simply qualify the function call as `PatternMatching.PatternMatch`.)
- - - 

© 2016–2017 Paul Ruane

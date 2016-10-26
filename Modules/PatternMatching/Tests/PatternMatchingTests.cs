using System;

using NUnit.Framework;

namespace Oniony.Sprouts.Core.PatternMatching.Tests
{
    [TestFixture]
    public sealed class PatternMatchingTests
    {
        #region Tests

        [Test]
        public void MatchesValue()
        {
            // set-up

            Animal berty = new Dog("Berty");

            var isBerty = false;
            var isRover = false;
            var isDog = false;
            var isCat = false;
            var isSomethingElse = false;

            // test

            berty.Match().Case(berty, _ => isBerty = true)
                         .Case<Animal>(_ => _.Name == "Rover", _ => isRover = true)
                         .Case<Dog>(_ => isDog = true)
                         .Case<Cat>(_ => isCat = true)
                         .Default(_ => isSomethingElse = true);

            // validate

            Assert.IsTrue(isBerty);
            Assert.IsFalse(isRover);
            Assert.IsFalse(isDog);
            Assert.IsFalse(isCat);
            Assert.IsFalse(isSomethingElse);
        }

        [Test]
        public void MatchesPredicate()
        {
            // set-up

            Animal berty = new Dog("Berty");
            Animal rover = new Dog("Rover");

            var isBerty = false;
            var isRover = false;
            var isDog = false;
            var isCat = false;
            var isSomethingElse = false;

            // test

            rover.Match().Case(berty, _ => isBerty = true)
                         .Case<Animal>(_ => _.Name == "Rover", _ => isRover = true)
                         .Case<Dog>(_ => isDog = true)
                         .Case<Cat>(_ => isCat = true)
                         .Default(_ => isSomethingElse = true);

            // validate

            Assert.IsFalse(isBerty);
            Assert.IsTrue(isRover);
            Assert.IsFalse(isDog);
            Assert.IsFalse(isCat);
            Assert.IsFalse(isSomethingElse);
        }

        [Test]
        public void MatchesType1()
        {
            // set-up

            Animal berty = new Dog("Berty");
            Animal sebastian = new Dog("Sebastian");

            var isBerty = false;
            var isRover = false;
            var isDog = false;
            var isCat = false;
            var isSomethingElse = false;

            // test

            sebastian.Match().Case(berty, _ => isBerty = true)
                             .Case<Animal>(_ => _.Name == "Rover", _ => isRover = true)
                             .Case<Dog>(_ => isDog = true)
                             .Case<Cat>(_ => isCat = true)
                             .Default(_ => isSomethingElse = true);

            // validate

            Assert.IsFalse(isBerty);
            Assert.IsFalse(isRover);
            Assert.IsTrue(isDog);
            Assert.IsFalse(isCat);
            Assert.IsFalse(isSomethingElse);
        }

        [Test]
        public void MatchesType2()
        {
            // set-up

            Animal berty = new Dog("Berty");
            Animal ravichandran = new Cat("Ravichandran");

            var isBerty = false;
            var isRover = false;
            var isDog = false;
            var isCat = false;
            var isSomethingElse = false;

            // test

            ravichandran.Match().Case(berty, _ => isBerty = true)
                                .Case<Animal>(_ => _.Name == "Rover", _ => isRover = true)
                                .Case<Dog>(_ => isDog = true)
                                .Case<Cat>(_ => isCat = true)
                                .Default(_ => isSomethingElse = true);

            // validate

            Assert.IsFalse(isBerty);
            Assert.IsFalse(isRover);
            Assert.IsFalse(isDog);
            Assert.IsTrue(isCat);
            Assert.IsFalse(isSomethingElse);
        }

        [TestCase(                            "dog",       "Berty",     "Berty!")]
        [TestCase(                            "skunk",     "Rover",     "Rover!")]
        [TestCase(                            "dog",       "Arthur",    "a dog called Arthur")]
        [TestCase(                            "cat",       "Sally",     "a cat called Sally")]
        [TestCase(                            "skunk",     "Grace",     "an animal called Grace")]
        public void MatchesTypeProducesResult(string type, string name, string expectedDescription)
        {
            // set-up

            Func<Animal> animalFactory = () => {
                                                   switch (type)
                                                   {
                                                       case "dog": return new Dog(name);
                                                       case "cat": return new Cat(name);
                                                       case "skunk": return new Skunk(name);
                                                       default: throw new Exception("wat?");
                                                   }
                                               };

            var berty = new Dog("Berty");
            Animal animal = animalFactory();

            // test

            string description = animal.Match().Returns<string>().Case(berty, _ => "Berty!")
                                                                 .Case<Animal>(_animal => _animal.Name == "Rover", _animal => "Rover!")
                                                                 .Case<Dog>(dog => "a dog called " + dog.Name)
                                                                 .Case<Cat>(cat => "a cat called " + cat.Name)
                                                                 .Default(_animal => "an animal called " + _animal.Name);

            // validate

            Assert.AreEqual(expectedDescription, description);
        }

        [Test]
        public void MatchesDefault()
        {
            // set-up

            Animal berty = new Dog("Berty");
            Animal henry = new Skunk("Henry");

            var isBerty = false;
            var isRover = false;
            var isDog = false;
            var isCat = false;      
            var isSomethingElse = false;
                                       
            // test

            henry.Match().Case(berty, _ => isBerty = true)
                         .Case<Animal>(_ => _.Name == "Rover", _ => isRover = true)
                         .Case<Dog>(_ => isDog = true)
                         .Case<Cat>(_ => isCat = true)
                         .Default(_ => isSomethingElse = true);

            // validate

            Assert.IsFalse(isBerty);
            Assert.IsFalse(isRover);
            Assert.IsFalse(isDog);
            Assert.IsFalse(isCat);
            Assert.IsTrue(isSomethingElse);
        }

        #endregion

        #region Common

        abstract class Animal : IEquatable<Animal>
        {
            protected Animal(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public bool Equals(Animal other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Name, other.Name);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Animal) obj);
            }

            public override int GetHashCode() => Name?.GetHashCode() ?? 0;
            public static bool operator ==(Animal left, Animal right) => Equals(left, right);
            public static bool operator !=(Animal left, Animal right) => !Equals(left, right);
        }

        private sealed class Dog : Animal
        {
            public Dog(string name) : base(name) {}
        }

        sealed class Cat : Animal
        {
            public Cat(string name) : base(name) {}
        }

        sealed class Skunk : Animal
        {
            public Skunk(string name) : base(name) {}
        }

        #endregion
    }
}

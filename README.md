# AutoFixture.Emitter

A few extensions to the [AutoFixture](https://github.com/AutoFixture/AutoFixture) library that assist in creating objects, mostly data contracts, that implement one or more interfaces.

Use in your tests where to you don't have a concrete class that implements all of the target interfaces or you don't want to reference another assembly just to do so.

## Usage

Create a single object
```csharp
using Eppyjerk.AutoFixture.Emitter;
...
var fixture = new Fixture();
var myObject = fixture.CreateObjectOfTypes(typeof(IHuman), typeof(IRobot));
```

Create many objects
```csharp
using Eppyjerk.AutoFixture.Emitter;
...
var fixture = new Fixture();
var myObjects = fixture.CreateManyObjectsOfTypes(typeof(IHuman), typeof(IRobot));
```

There are also generic overloads for when you just need one interface.
```csharp
var myHuman = fixture.CreateObjectOfType<IHuman>();
var myRobots = fixture.CreateManyObjectsOfType<IRobot>();
```

## License

[MIT](LICENSE.txt)
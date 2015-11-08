using System;

namespace Eppyjerk.AutoFixture.Emitter.Tests
{
    public interface IPerson
    {
        string Name { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        int Age { get; set; }
        DateTime BirthDate { get; set; }
        decimal Weight { get; set; }

        bool IsShort { get; }
        bool Diet { set; }

        ICar FavoriteCar { get; set; }
    }

    public interface ICar
    {
        string Color { get; set; }
        int Doors { get; set; }
        string Name { get; set; }

        IPerson Driver { get; set; }
    }

    public interface IPlane
    {
        int Propellers { get; set; }
        string Name { get; set; }
        decimal Wingspan { get; set; }

        IPerson Pilot { get; set; }
    }
}

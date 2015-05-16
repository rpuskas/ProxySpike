using System.Collections.Generic;
using System.Linq;

namespace LibraryServer
{
    public class FooService
    {
        public FooResult Get(int id)
        {
            return FooRepository.All().Single(i => i.Id == id);
        }

        public IEnumerable<FooResult> Search(FooQuery query)
        {
            return FooRepository.All().Where(i => i.Name == query.Name);
        }

        public class FooQuery
        {
            public string Name;
        }

        public class FooResult
        {
            public FooResult(int id, string name, int age)
            {
                Id = id;
                Name = name;
                Age = age;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }

    public class FooRepository
    {
        public static IEnumerable<FooService.FooResult> All()
        {
            return new[]
            {
                new FooService.FooResult(1, "One", 1),
                new FooService.FooResult(2, "Two", 1),
                new FooService.FooResult(3, "Twins", 1),
                new FooService.FooResult(4, "Twins", 1)
            };
        }
    }
}

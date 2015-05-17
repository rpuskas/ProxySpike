using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace LibraryServer
{
    public class Bootstrapper
    {
        public Bootstrapper(IUnityContainer container)
        {
            var fooRepository = new FooRepository();
            container.RegisterInstance(fooRepository);
        }
    }

    public class FooService
    {
        private readonly FooRepository _fooRepository;

        public FooService(FooRepository fooRepository)
        {
            _fooRepository = fooRepository;
        }

        public FooResult Get(int id)
        {
            return _fooRepository.All().Single(i => i.Id == id);
        }

        public IEnumerable<FooResult> Search(FooQuery query)
        {
            return _fooRepository.All().Where(i => i.Name == query.Name);
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
        public IEnumerable<FooService.FooResult> All()
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

namespace LibraryServer
{
    public class Foo
    {
        public FooResult Get()
        {
            return new FooResult("FooResult", 1);   
        }
    }

    public class FooResult
    {
        public FooResult(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Name { get; set; }
        public int Age { get; set; }
    }
}

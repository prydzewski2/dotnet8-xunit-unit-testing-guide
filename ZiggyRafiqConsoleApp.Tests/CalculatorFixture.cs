namespace ZiggyRafiqConsoleApp
{
    using System.Threading.Tasks;
    public class CalculatorFixture : IDisposable
    {
        public Calculator Calculator { get; private set; }

        public CalculatorFixture()
        {
            // Initialize resources, create instances, etc.
            Calculator = new Calculator();
        }

        public void Dispose()
        {
            // Clean up resources, dispose of instances, etc.
            Calculator.Dispose();
        }
    }
public class Fixture1:IAsyncLifetime 
{
        public int Counter = 0;

        public Fixture1() {
            Console.WriteLine("cf1");
        }

            public async Task InitializeAsync()
                        => await Task.Run(() => { Counter++;
                        Console.WriteLine("f1");});

                                public Task DisposeAsync()
                                            => Task.CompletedTask;
}

public class Fixture2:IClassFixture<Fixture1>, IAsyncLifetime 
{
        public Fixture1 F1;

            public Fixture2(Fixture1 f1) {
                F1 = f1;
                Console.WriteLine("cf2");
                }
            public async Task InitializeAsync()
                        => await Task.Run(() => {Console.WriteLine("f2");});

                                public Task DisposeAsync()
                                            => Task.CompletedTask;
}

public class Fixture3: IClassFixture<Fixture1>, IAsyncLifetime {
    public Fixture1 F1;
    public int Counter = 0;
    public Fixture3(Fixture1 f1){
        F1=f1;
        Counter=f1.Counter;
        Console.WriteLine("cf3");
    }

    public async Task InitializeAsync()=> await Task.Run(()=>{Counter++;
    Console.WriteLine("f3");});
    public Task DisposeAsync()=> Task.CompletedTask;
}

public class Fixture4: IAsyncLifetime{
public Fixture3? F3;
public int Counter = 0;
public Fixture4() {
    
    //Counter = f3.Counter;
    Console.WriteLine("cf4");
}

public async Task InitializeAsync()=>await Task.Run(()=>{Counter++;
Console.WriteLine("f4");});
public Task DisposeAsync()=> Task.CompletedTask;
}

[CollectionDefinition(nameof(MyCollection))]
public class MyCollection : ICollectionFixture<Fixture1>
{ }

[CollectionDefinition(nameof(MyCollection2))]
public class MyCollection2 : ICollectionFixture<Fixture1>
{ }

public abstract class Base {
   public Fixture4 f4;
    public Base(Fixture3 f3, Fixture4 f4)
    {
        this.f4=f4;
        this.f4.F3 = f3;
        this.f4.Counter = f3.Counter;
    }

    //public async Task InitializeAsync()=> await f4.InitializeAsync();
    //public Task DisposeAsync() => f4.DisposeAsync();
}
[Collection(nameof(MyCollection))]
public class UnitTest1 : Base, IClassFixture<Fixture2>, IClassFixture<Fixture3>, IClassFixture<Fixture4>
{
        Fixture2 f2;
        Fixture3 f3;
        //Fixture4 f4;

            public UnitTest1(Fixture3 f3,Fixture2 f2, Fixture4 f4):base(f3, f4)
            {
this.f2 = f2;
this.f3= f3;
//this.f4=f4;
            }
            [Fact]
            public void Test2()
            {
                Console.WriteLine("U1Test2");
                Assert.Equal(2, this.f4.Counter);
                this.f4.Counter++;
                Assert.Equal(3, this.f4.Counter);
            }

                [Fact]
                    public void Test3()
                        {
                            Console.WriteLine("U1Test3");
                                            Assert.Equal(1, this.f2.F1.Counter);
                                            Assert.Equal(1, this.f3.F1.Counter);
                                            Assert.Equal(2, this.f4.F3.Counter);
                                            Assert.Equal(2, this.f4.Counter);
                        }
}

[Collection(nameof(MyCollection2))]
public class test2: Base, IClassFixture<Fixture3>,IClassFixture<Fixture2>,IClassFixture<Fixture4>
{
    Fixture3 f3;
    Fixture2 f2;

    public test2(Fixture2 f2, Fixture3 f3, Fixture4 f4):base(f3, f4)
    {
        this.f3 = f3;
        this.f2 =f2;
    }

    [Fact]
    public void Test()
    {
        Thread.Sleep(3000);
        Console.WriteLine("U2Test");
        Assert.Equal(1, this.f2.F1.Counter);
        Assert.Equal(1, this.f3.F1.Counter);
        Assert.Equal(2, this.f4.F3.Counter);
        Assert.Equal(2, this.f4.Counter);
    }
}}



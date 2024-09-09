namespace Spectre.Console.Testing;

/// <summary>
/// This is a utility class for implementors of
/// <see cref="ITypeRegistrar"/> and corresponding <see cref="ITypeResolver"/>.
/// </summary>
public sealed class TypeRegistrarBaseTests
{
    private readonly Func<ITypeRegistrar> _registrarFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeRegistrarBaseTests"/> class.
    /// </summary>
    /// <param name="registrarFactory">The factory to create a new, clean <see cref="ITypeRegistrar"/> to be used for each test.</param>
    public TypeRegistrarBaseTests(Func<ITypeRegistrar> registrarFactory)
    {
        _registrarFactory = registrarFactory;
    }

    /// <summary>
    /// Runs all tests.
    /// </summary>
    /// <exception cref="TestFailedException">This exception is raised, if a test fails.</exception>
    public void RunAllTests()
    {
        var testCases = new[]
        {
                RegistrationsCanBeResolved,
                InstanceRegistrationsCanBeResolved,
                LazyRegistrationsCanBeResolved,
                ResolvingNotRegisteredServiceReturnsNull,
                ResolvingNullTypeReturnsNull,
                ResolvingSingleInstanceOfMultipleRegistrationsResolvesTheFirstOne,
                ResolvingAnEnumerableOfInstancesDoesNotReturnNull,
                ResolvingAnEnumerableOfInstancesOfMultipleRegistrationsResolvesAllRegistrations,
        };

        foreach (var test in testCases)
        {
            test(_registrarFactory());
        }
    }

    private void ResolvingAnEnumerableOfInstancesDoesNotReturnNull(ITypeRegistrar registrar)
    {
        // Given
        var resolver = registrar.Build();

        // When
        var actual = resolver.Resolve(typeof(IEnumerable<IMockService>)) as IEnumerable<IMockService>;

        // Then
        if (actual == null)
        {
            throw new TestFailedException(
                "Expected an IEnumerable never to resolve to null.");
        }
    }

    private void ResolvingAnEnumerableOfInstancesOfMultipleRegistrationsResolvesAllRegistrations(ITypeRegistrar registrar)
    {
        // Given
        var theLastRegistration = new AnotherMockService("last");
        registrar.RegisterLazy(typeof(IMockService), () => new AnotherMockService("first"));
        registrar.Register(typeof(IMockService), typeof(MockService));
        registrar.RegisterInstance(typeof(IMockService), theLastRegistration);
        var resolver = registrar.Build();

        // When
        var actual = (resolver.Resolve(typeof(IEnumerable<IMockService>)) as IEnumerable<IMockService>)!.ToList();

        // Then
        if (actual.Count != 3)
        {
            throw new TestFailedException(
                "Expected the resolver to resolve a list with exactly 3 elements.");
        }

        if (actual.Count(x => x.GetType() == typeof(AnotherMockService)) != 2)
        {
            throw new TestFailedException(
                $"Expected the resolver to resolve a list with exactly 2 elements of type {nameof(AnotherMockService)}.");
        }

        if (actual.Count(x => x.GetType() == typeof(MockService)) != 1)
        {
            throw new TestFailedException(
                $"Expected the resolver to resolve a list with exactly one element of type {nameof(MockService)}.");
        }

        if (!actual.Contains(theLastRegistration))
        {
            throw new TestFailedException(
                "Expected the resolver to resolve the known instance that was registered.");
        }
    }

    private void ResolvingSingleInstanceOfMultipleRegistrationsResolvesTheFirstOne(ITypeRegistrar registrar)
    {
        // Given
        var theLastRegistration = new AnotherMockService("last");
        registrar.RegisterLazy(typeof(IMockService), () => new AnotherMockService("first"));
        registrar.Register(typeof(IMockService), typeof(MockService));
        registrar.RegisterInstance(typeof(IMockService), theLastRegistration);
        var resolver = registrar.Build();

        // When
        var actual = resolver.Resolve(typeof(IMockService));

        // Then
        if (!ReferenceEquals(actual, theLastRegistration))
        {
            throw new TestFailedException(
                "Expected the resolver to resolve the first registered instance of multiple registrations.");
        }
    }

    private static void ResolvingNullTypeReturnsNull(ITypeRegistrar registrar)
    {
        // Given no registration
        var resolver = registrar.Build();

        try
        {
            // When
            var actual = resolver.Resolve(null);

            // Then
            if (actual != null)
            {
                throw new TestFailedException(
                    $"Expected the resolver to resolve null, since null was requested as the service type. Actually resolved {actual.GetType().Name}.");
            }
        }
        catch (Exception ex)
        {
            throw new TestFailedException(
                $"Expected the resolver not to throw, but caught {ex.GetType().Name}.", ex);
        }
    }

    private static void ResolvingNotRegisteredServiceReturnsNull(ITypeRegistrar registrar)
    {
        // Given no registration
        var resolver = registrar.Build();

        try
        {
            // When
            var actual = resolver.Resolve(typeof(IMockService));

            // Then
            if (actual != null)
            {
                throw new TestFailedException(
                    $"Expected the resolver to resolve null, since no service was registered. Actually resolved {actual.GetType().Name}.");
            }
        }
        catch (Exception ex)
        {
            throw new TestFailedException(
                $"Expected the resolver not to throw, but caught {ex.GetType().Name}.", ex);
        }
    }

    private static void RegistrationsCanBeResolved(ITypeRegistrar registrar)
    {
        // Given
        registrar.Register(typeof(IMockService), typeof(MockService));
        var resolver = registrar.Build();

        // When
        var actual = resolver.Resolve(typeof(IMockService));

        // Then
        if (actual == null)
        {
            throw new TestFailedException(
                $"Expected the resolver to resolve an instance of {nameof(MockService)}. Actually resolved null.");
        }

        if (actual is not MockService)
        {
            throw new TestFailedException(
                $"Expected the resolver to resolve an instance of {nameof(MockService)}. Actually resolved {actual.GetType().Name}.");
        }
    }

    private static void InstanceRegistrationsCanBeResolved(ITypeRegistrar registrar)
    {
        // Given
        var instance = new MockService();
        registrar.RegisterInstance(typeof(IMockService), instance);
        var resolver = registrar.Build();

        // When
        var actual = resolver.Resolve(typeof(IMockService));

        // Then
        if (!ReferenceEquals(actual, instance))
        {
            throw new TestFailedException(
                "Expected the resolver to resolve exactly the registered instance.");
        }
    }

    private static void LazyRegistrationsCanBeResolved(ITypeRegistrar registrar)
    {
        // Given
        var instance = new MockService();
        var factoryCalled = false;
        registrar.RegisterLazy(typeof(IMockService), () =>
        {
            factoryCalled = true;
            return instance;
        });
        var resolver = registrar.Build();

        // When
        var actual = resolver.Resolve(typeof(IMockService));

        // Then
        if (!factoryCalled)
        {
            throw new TestFailedException(
                "Expected the factory to be called, to resolve the lazy registration.");
        }

        if (!ReferenceEquals(actual, instance))
        {
            throw new TestFailedException(
                "Expected the resolver to return exactly the result of the lazy-registered factory.");
        }
    }

    /// <summary>
    /// internal use only.
    /// </summary>
    private interface IMockService
    {
    }

    private class MockService : IMockService
    {
    }

    private class AnotherMockService : IMockService
    {
        public AnotherMockService(string ignore)
        {
        }
    }

    /// <summary>
    /// Exception, to be raised when a test fails.
    /// </summary>
    public sealed class TestFailedException : Exception
    {
        /// <inheritdoc cref="Exception" />
        public TestFailedException(string message)
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception" />
        public TestFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
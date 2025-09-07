using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;

namespace Fx.Convert.Tests.FramewoTrk
{
    public abstract class Test<TClass>
        where TClass : class
    {
        // Holds the “greediest” constructor and a compiled factory delegate
        private static readonly ConstructorInfo _ctor;
        private static readonly Func<IDictionary<Type, object>, TClass> _factory;

        // Buffer for dependency instances (mocks or primitives)
        private readonly ConcurrentDictionary<Type, object> _container
            = new ConcurrentDictionary<Type, object>();

        private readonly object _lock = new();

        protected TClass Instance { get; private set; }

        static Test()
        {
            // 1. Pick the constructor with the most parameters
            _ctor = typeof(TClass)
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .First();

            // 2. Build an expression: (dict) => new TClass(
            //        (Param0Type) dict[Param0Type],
            //        …
            //    )
            var dictParam = Expression.Parameter(typeof(IDictionary<Type, object>), "dict");
            var args = _ctor.GetParameters()
                .Select(p =>
                    Expression.Convert(
                        Expression.Call(
                            dictParam,
                            typeof(IDictionary<Type, object>).GetMethod("get_Item")!,
                            Expression.Constant(p.ParameterType, typeof(Type))
                        ),
                        p.ParameterType
                    )
                )
                .ToArray();

            var newExpr = Expression.New(_ctor, args);
            _factory = Expression
                .Lambda<Func<IDictionary<Type, object>, TClass>>(newExpr, dictParam)
                .Compile();
        }

        protected Test()
        {
            RebuildInstance();
        }

        /// <summary>
        /// Returns the (mocked or primitive) dependency of type TDependency.
        /// Automatically creates it on first request if missing.
        /// </summary>
        protected TDependency Get<TDependency>()
        {
            var type = typeof(TDependency);
            if (_container.TryGetValue(type, out var existing))
                return (TDependency)existing;

            // auto-create missing dependency
            var created = CreateDependency(type);
            _container[type] = created;
            RebuildInstance();
            return (TDependency)created;
        }

        /// <summary>
        /// Overrides a dependency and rebuilds the SUT.
        /// </summary>
        protected void Set<TDependency>(TDependency value)
        {
            var type = typeof(TDependency);
            if (!_ctor.GetParameters().Any(p => p.ParameterType == type))
                throw new InvalidOperationException(
                    $"Type {type.FullName} is not a constructor parameter of {typeof(TClass).FullName}");

            _container[type] = value!;
            RebuildInstance();
        }

        /// <summary>
        /// Recreates the SUT using the compiled factory and current container.
        /// </summary>
        private void RebuildInstance()
        {
            lock (_lock)
            {
                // Ensure every ctor parameter has an entry
                foreach (var param in _ctor.GetParameters())
                {
                    _container.GetOrAdd(param.ParameterType, CreateDependency);
                }

                Instance = _factory(_container);
            }
        }

        /// <summary>
        /// Creates a single dummy dependency:
        /// - primitives via Activator  
        /// - interfaces & delegates via NSubstitute.For  
        /// - concrete classes via partial substitute
        /// </summary>
        private static object CreateDependency(Type type)
        {
            if (type.IsPrimitive)
                return Activator.CreateInstance(type)!;

            if (type.IsInterface || typeof(Delegate).IsAssignableFrom(type))
                return Substitute.For(new[] { type }, Array.Empty<object>());

            return SubstitutionContext
                .Current
                .SubstituteFactory
                .CreatePartial(new[] { type }, Array.Empty<object>());
        }
    }
}

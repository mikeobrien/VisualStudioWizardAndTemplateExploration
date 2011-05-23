using System;

namespace VisualStudio
{
    public static class Maybe
    {
        public static Maybe<T> Just<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> Nothing<T>()
        {
            return new Maybe<T>();
        }
    }

    public static class MaybeMonadicExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return value == null ? Maybe.Nothing<T>() : Maybe.Just(value);
        }

        public static Maybe<B> Bind<A, B>(this Maybe<A> a, Func<A, Maybe<B>> func)
        {
            return a != null && a.HasValue ? func(a.Value) : Maybe.Nothing<B>();
        }
    }

    public class Maybe<T>
    {
        public Maybe()
        {
            Value = default(T);
            HasValue = false;
        }

        public Maybe(T value)
        {
            Value = value;
            HasValue = true;
        }

        public T Value { get; private set; }
        public bool HasValue { get; private set; }
    }
}

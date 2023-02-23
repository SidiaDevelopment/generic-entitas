using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EntityFramework
{
    public class MatcherComparer : IEqualityComparer<Matcher>
    {
        public bool Equals(Matcher x, Matcher y)
        {
            if (x == null) return false;
            if (y == null) return false;
            return x.Components.SequenceEqual(y.Components);
        }

        public int GetHashCode(Matcher obj)
        {
            return obj.Components.Aggregate(0, (i, type) => i + type.GetHashCode()).GetHashCode();
        }
    }
    
    public class Matcher
    {
        Type[] _components;

        public Type[] Components => _components;
        public Matcher(Type[] components)
        {
            _components = components;
        }

        public override int GetHashCode()
        {
            return (_components != null ? _components.GetHashCode() : 0);
        }

        public bool Matches(Entity entity)
        {
            return entity.HasComponents(_components);
        }
    }
}
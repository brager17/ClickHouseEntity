using System.Collections.Generic;
using System.Linq;

namespace DbContext
{
    public class SimpleTypeBuilderAdapter : IComplexEntityBinder
    {
        private readonly ISingleObjectBinder _singleObjectBinder;

        public SimpleTypeBuilderAdapter(ISingleObjectBinder singleObjectBinder)
        {
            _singleObjectBinder = singleObjectBinder;
        }

        public T Handle<T>(IEnumerable<Cell> cells) => _singleObjectBinder.Handle<T>(cells.Single());
    }
}
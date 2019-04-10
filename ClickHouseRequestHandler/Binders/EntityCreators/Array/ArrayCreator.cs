using System;
using System.Collections;
using ExpressionTreeVisitor;

namespace DbContext
{
    [Info("Optimize code")]
    public class ArrayCreator : IPrimitiveTypeOrPrimitiveArrayBinder
    {
        public T Handle<T>(NameValue nameValue)
        {
            var array = (object[]) nameValue.Value;
            var arrayType = typeof(T).GetElementType();
            var typedArray = (IList) Array.CreateInstance(arrayType, array.Length);
            for (var i = 0; i < array.Length; i++)
                typedArray[i] = array[i];

            var cast = (T) typedArray;
            return cast;
        }
    }
}
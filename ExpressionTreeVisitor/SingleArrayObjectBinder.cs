using System;
using System.Collections;

namespace DbContext
{
    public class SingleArrayObjectBinder : ISingleObjectBinder
    {
        public T Handle<T>(NameValue nameValue)
        {
            try
            {
                var array = (object[]) nameValue.Value;
                var arrayType = typeof(T).GetElementType();
                var typedArray = (IList) Array.CreateInstance(arrayType, array.Length);
                for (var i = 0; i < array.Length; i++)
                    typedArray[i] = array[i];

                var cast = (T) typedArray;
                return cast;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
    }
}
using System.Reflection;
using Shared.Domain;

namespace Customer.Tests
{
    public static class ModelsGetSetTest
    {

        //[ClassData(typeof(ModelTestDataGenerator))]
        //[Theory, AutoMoqData]
        public static void GettersGetWithoutError<T>(T model)
            where T : BaseEntity
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < properties.Length; i++)
            {
                var prop = properties[i];
                if (prop.CanRead)
                {
                    var value = prop.GetValue(model); 
                } 
            }
        }

        //[ClassData(typeof(ModelTestDataGenerator))]
        //[Theory]
        public static void SettersSetWithoutError<T>(T model)
            where T : BaseEntity
        {
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < properties.Length; i++)
            {
                var prop = properties[i];
                if (prop.CanWrite)
                {
                    prop.SetValue(model, null);
                }

            }
        }

        //public class ModelTestDataGenerator : IEnumerable<object[]>
        //{
        //    private readonly List<object[]> _data = new List<object[]>
        //    {
        //        new object[] { new UserRole() },
        //        new object[] { new User() },
        //        new object[] { new Role() }
        //    };

        //    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        //}
    }
}
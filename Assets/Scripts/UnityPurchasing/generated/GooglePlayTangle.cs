// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("/HSNTEftGGeMfpCsb2PjR02Foj/Z0mZHMXcayv0vJMlWzf/7xknJGORSu3o/X2TJKcD92mZ5ZlXt5+hUQ2tfqMusdOBWDz7kLFGzMYqDSRe/PDI9Db88Nz+/PDw9jpNjyq4o6wQ7jvq0+NufTKRhnB/O2UfnoOpkgutNApf0UpQ3GUdeyyUxqX7bywxL/GC3CQTj/i3mMItwIjfWodd1NiV0B5/2mUzsZn1SaBsXDnAZa3o9x/qGB4aawd3GRNKx3bwrBWr5xIvpfwAkwQwioAVvvmnD8mwYPOuh/w2/PB8NMDs0F7t1u8owPDw8OD0+dObIEfezn0xwboociy5SoyC2bLK6ofIO8/wrCWsmbT9vwG18aax+RWeXRieLXZ2mjj8+PD08");
        private static int[] order = new int[] { 6,2,8,7,8,5,9,11,10,11,12,11,12,13,14 };
        private static int key = 61;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}

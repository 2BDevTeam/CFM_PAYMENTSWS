namespace LENMEDWS.Extensions
{
    public static class StaticConvertExtension
    {
        public static Decimal toDecimal(this string value)
        {
            try { return Convert.ToDecimal(value); }

            catch (Exception e) { return 000; }

        }
    }
}

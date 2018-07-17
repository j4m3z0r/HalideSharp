namespace HalideSharp
{
    public partial class HSVar
    {
        // HACK: returning false from both operator true and operator false should force us to never short-circuit,
        // meaning that && will always invoke operator&, which calls C++'s operator&& in the wrapper. In short, this
        // allows for the common (x && y) style conditionals.
        public static bool operator true(HSVar v)
        {
            return false;
        }

        public static bool operator false(HSVar v)
        {
            return false;
        }

        
    }
}
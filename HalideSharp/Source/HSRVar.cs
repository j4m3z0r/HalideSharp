using System;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace HalideSharp
{
    public class HSRVar : HSObject
    {
        [DllImport(Constants.LibName)]
        private static extern IntPtr RVar_New();
        public HSRVar() : base()
        {
            _cppobj = RVar_New();
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr RVar_New_String([MarshalAs(Constants.StringType)] string name);
        private string _name;
        public HSRVar(string n)
        {
            _name = n;
            _cppobj = RVar_New_String(n);
        }

        [DllImport(Constants.LibName)]
        private static extern void RVar_Delete(IntPtr o);
        ~HSRVar()
        {
            RVar_Delete(_cppobj);
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr RVar_Min(IntPtr self);
        public HSExpr Min()
        {
            var result = new HSExpr(RVar_Min(HSUtil.CArg(this)));
            result.AddRef(this);
            return result;
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr RVar_Extent(IntPtr self);
        public HSExpr Extent()
        {
            var result = new HSExpr(RVar_Extent(HSUtil.CArg(this)));
            result.AddRef(this);
            return result;
        }

        // TODO: read this back from C++ so that we pick up auto-generated names
        public string Name => _name;

        [DllImport(Constants.LibName)]
        private static extern IntPtr Expr_New_RVar(IntPtr rvar);
        public static implicit operator HSExpr(HSRVar v)
        {
            var result = new HSExpr(Expr_New_RVar(HSUtil.CArg(v)));
            result.AddRef(v);
            return result;
        }
    }
}
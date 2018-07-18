using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace HalideSharp
{
    public class HS
    {
        [DllImport(Constants.LibName, EntryPoint = "cast_to_float")]
        private static extern IntPtr CastToFloat(IntPtr expr);
        
        [DllImport(Constants.LibName, EntryPoint = "cast_to_byte")]
        private static extern IntPtr CastToByte(IntPtr expr);
        
        [DllImport(Constants.LibName, EntryPoint = "cast_to_ushort")]
        private static extern IntPtr CastToUshort(IntPtr expr);
        
        public static HSExpr Cast<T>(HSExpr expr)
        {
            IntPtr newExpr;
            if (typeof(T) == typeof(float))
            {
                newExpr = CastToFloat(HSUtil.CArg(expr));
            }
            else if (typeof(T) == typeof(byte))
            {
                newExpr = CastToByte(HSUtil.CArg(expr));
            }
            else if (typeof(T) == typeof(ushort))
            {
                newExpr = CastToUshort(HSUtil.CArg(expr));
            }
            else
            {
                throw new NotImplementedException($"Casting not implemented for type {typeof(T)}");
            }
            
            return new HSExpr(newExpr);
        }

        [DllImport(Constants.LibName, EntryPoint = "min_expr_float")]
        private static extern IntPtr MinExprFloat(IntPtr expr, float f);

        public static HSExpr Min(HSExpr expr, float f)
        {
            // Min uses move semantics.
            var newExpr = MinExprFloat(HSUtil.CArg(expr), HSUtil.CArg(f));
            return new HSExpr(newExpr);
        }

        [DllImport(Constants.LibName, EntryPoint = "print_objects_when")]
        private static extern IntPtr PrintObjectsWhen(IntPtr condition, int numObjects, SharedEnums.HSObjectType[] objTypes, IntPtr[] objects);
        
        public static HSExpr PrintWhen(HSExpr when, params object[] args)
        {
            // Construct a pair of parallel arrays representing the argument list: one for denoting the types of the
            // arguments, the other for pointers to the arguments themselves.
            var typeList = new SharedEnums.HSObjectType[args.Length];
            var objList = new IntPtr[args.Length];
            var garbage = new List<GCHandle>();

            try
            {
                for (var i = 0; i < args.Length; i++)
                {
                    var a = args[i];
                    if (a is string s)
                    {
                        // Convert to null-terminated UTF8 string. FIXME: get the encoding from the Constants class.
                        var bytes = Encoding.UTF8.GetBytes(s + '\0');
                        var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                        garbage.Add(handle);

                        typeList[i] = SharedEnums.HSObjectType.HS_String;
                        objList[i] = handle.AddrOfPinnedObject();
                    }
                    else if (a is HSVar @var)
                    {
                        typeList[i] = SharedEnums.HSObjectType.HS_Var;
                        objList[i] = HSUtil.CArg(@var);
                    }
                    else if (a is HSExpr expr)
                    {
                        typeList[i] = SharedEnums.HSObjectType.HS_Expr;
                        objList[i] = HSUtil.CArg(expr);
                    }
                    else
                    {
                        throw new NotImplementedException($"Cannot print objects {a} at index {i}");
                    }
                }

                if (ReferenceEquals(when, null))
                {
                    return new HSExpr(PrintObjectsWhen(IntPtr.Zero, args.Length, typeList, objList));
                }
                else
                {
                    return new HSExpr(PrintObjectsWhen(HSUtil.CArg(when), args.Length, typeList, objList));
                }

            }
            finally
            {
                foreach (var h in garbage)
                {
                    h.Free();
                }
            }
        }

        public static HSExpr Print(params object[] args)
        {
            return PrintWhen(null, args);
        }

        [DllImport(Constants.LibName, EntryPoint = "clamp_expr_int_int")]
        private static extern IntPtr ClampExprIntInt(IntPtr e, int min, int max);

        public static HSExpr Clamp(HSExpr e, int min, int max)
        {
            return new HSExpr(ClampExprIntInt(HSUtil.CArg(e), HSUtil.CArg(min), HSUtil.CArg(max)));
        }

        [DllImport(Constants.LibName, EntryPoint = "clamp_var_int_int")]
        private static extern IntPtr ClampVarIntInt(IntPtr v, int min, int max);
        
        public static HSExpr Clamp(HSVar v, int min, int max)
        {
            return new HSExpr(ClampVarIntInt(HSUtil.CArg(v), HSUtil.CArg(min), HSUtil.CArg(max)));
        }

        [DllImport(Constants.LibName)]
        private static extern IntPtr Global_GetHostTarget();

        public static HSTarget GetHostTarget()
        {
            return new HSTarget(Global_GetHostTarget());
        }

    }
}

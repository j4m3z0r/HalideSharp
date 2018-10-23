#include <Halide.h>
#include <vector>
#include <string>

#include "magicmacros.h"

#define LANGUAGE_C 1
#include "SharedEnums.cs"
#undef LANGUAGE_C

using namespace Halide;

// TODO: update these functions to the new naming scheme.
#define GEN_CAST(CSTYPE, CPPTYPE) \
    extern "C" Expr* Global_CastTo ## CSTYPE(Expr *expr) { \
        auto casted = cast<CPPTYPE>(*expr); \
        return new Expr(casted); \
    }
GEN(GEN_CAST)

extern "C" Expr* print_objects_when(Expr* condition, int numObjects, enum HSObjectType *types, void **objects) {
    std::vector<Expr> args;
    for(int i = 0; i < numObjects; i++) {
        void *obj = objects[i];

        switch(types[i]) {
            case HS_String:
                args.push_back(Expr((const char *) obj));
                break;

            case HS_Var:
                args.push_back(*((Var*) obj));
                break;

            case HS_Expr:
                args.push_back(*((Expr*) obj));
                break;
        }
    }

    if(condition == nullptr) {
        return new Expr(print(args));
    } else {
        return new Expr(print_when(*condition, args));
    }
}

extern "C" Expr* Global_Clamp_ExprExprExpr(Expr* e, Expr *min, Expr *max) { return new Expr(clamp(*e, *min, *max)); }

extern "C" Target* Global_GetHostTarget() { return new Target(get_host_target()); }

// Halide has a set of variables defined in its header that can be used anywhere. We expose them here.
extern "C" Var* Global_Var0() { return &_0; }
extern "C" Var* Global_Var1() { return &_1; }
extern "C" Var* Global_Var2() { return &_2; }
extern "C" Var* Global_Var3() { return &_3; }
extern "C" Var* Global_Var4() { return &_4; }
extern "C" Var* Global_Var5() { return &_5; }
extern "C" Var* Global_Var6() { return &_6; }
extern "C" Var* Global_Var7() { return &_7; }
extern "C" Var* Global_Var8() { return &_8; }
extern "C" Var* Global_Var9() { return &_9; }

extern "C" Expr* Global_Select_ExprExprExpr(Expr* condition, Expr* trueVal, Expr* falseVal)
{
    return new Expr(select(*condition, *trueVal, *falseVal));
}

extern "C" Expr* Global_Select_ExprExprExprExprVarargs(Expr* c0, Expr* v0, Expr* c1, Expr* v1, int numArgs, Expr** args)
{
    // Unlike Func::Reorder, there is no variant of select() that accepts a
    // vector of arguments -- presumably so that the requirement for an odd
    // number of arguments can be asserted at compile time. We don't have a
    // super great way to represent a requirement that there be an odd number
    // of arguments in C#, so we just throw an exception at runtime in C# if
    // the user passes an invalid number of arguments.
    switch(numArgs) {
        case 1: return new Expr(select(*c0, *v0, *c1, *v1, *args[0]));
        case 3: return new Expr(select(*c0, *v0, *c1, *v1, *args[0], *args[1], *args[2]));
        case 5: return new Expr(select(*c0, *v0, *c1, *v1, *args[0], *args[1], *args[2], *args[3], *args[4]));
        case 7: return new Expr(select(*c0, *v0, *c1, *v1, *args[0], *args[1], *args[2], *args[3], *args[4], *args[5], *args[6]));
        case 9: return new Expr(select(*c0, *v0, *c1, *v1, *args[0], *args[1], *args[2], *args[3], *args[4], *args[5], *args[6], *args[7], *args[8]));
    }

    return nullptr;
}


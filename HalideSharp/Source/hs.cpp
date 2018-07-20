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

extern "C" Expr* min_expr_float(Expr* expr, float f) {
    return new Expr(min(*expr, f));
}

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

extern "C" Expr* clamp_expr_int_int(Expr* e, int min, int max) { return new Expr(clamp(*e, min, max)); }
extern "C" Expr* clamp_var_int_int(Var *v, int min, int max) { return new Expr(clamp(*v, min, max)); }

extern "C" Target* Global_GetHostTarget() { return new Target(get_host_target()); }

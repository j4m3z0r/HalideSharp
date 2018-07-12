#ifndef __MAGIC_MACROS_H__
#define __MAGIC_MACROS_H__

#define PERMUTE_ARGS_1D(MACRO) \
    MACRO(Expr) \
    MACRO(Var)

#define PERMUTE_ARGS_2D(MACRO) \
    MACRO(Expr, Expr) \
    MACRO(Expr, Var) \
    MACRO(Var, Expr) \
    MACRO(Var, Var)

#define PERMUTE_ARGS_3D(MACRO) \
    MACRO(Expr, Expr, Expr) \
    MACRO(Expr, Expr, Var) \
    MACRO(Expr, Var, Expr) \
    MACRO(Expr, Var, Var) \
    MACRO(Var, Expr, Expr) \
    MACRO(Var, Expr, Var) \
    MACRO(Var, Var, Expr) \
    MACRO(Var, Var, Var)

#endif

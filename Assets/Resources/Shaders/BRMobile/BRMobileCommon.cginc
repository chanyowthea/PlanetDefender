#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
#define BRFOG(coord, color)  	coord.x = clamp(coord.x, unity_FogColor.a, 1.0f); UNITY_APPLY_FOG(coord, color);
#else
#define BRFOG(coord, color)
#endif

#define ConstantBrightness 1.2
#define LightmapMinDiffuse 0.6
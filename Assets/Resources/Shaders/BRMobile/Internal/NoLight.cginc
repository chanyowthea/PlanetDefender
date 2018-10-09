half4 LightingNoLight (SurfaceOutput s, half3 lightDir, half atten) {
  half4 c;
  c.rgb = s.Albedo * _LightColor0.rgb * 0.03; 
  c.a = s.Alpha;
  return c;
}
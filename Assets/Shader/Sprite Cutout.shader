Shader "Sprite/Cutout" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
 _CutOff ("Cut Off", Float) = 0.3
}
SubShader { 
 Pass {
  Cull Off
  AlphaTest Greater [_CutOff]
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary, texture alpha * primary alpha }
 }
}
}
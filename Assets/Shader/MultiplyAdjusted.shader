Shader "Mobile/MultiplyAdjusted" {
Properties {
 _Color ("Tint Color", Color) = (1,1,1,1)
 _MainTex ("Particle Texture", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="True" "RenderType"="Transparent" }
  BindChannels {
   Bind "color", Color
   Bind "texcoord", TexCoord
  }
  ZWrite Off
  Cull Off
  Blend Zero SrcColor
  SetTexture [_MainTex] { ConstantColor [_Color] combine texture + constant }
 }
}
}
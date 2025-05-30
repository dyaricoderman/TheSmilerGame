Shader "Custom/RenderVideoBackground" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="overlay+1" "RenderType"="Overlay" }
 Pass {
  Tags { "QUEUE"="overlay+1" "RenderType"="Overlay" }
  SetTexture [_MainTex] { combine texture }
 }
}
Fallback "Diffuse"
}
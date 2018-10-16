#version 440 core

in vec2 UV;

out vec3 color;

uniform sampler2D colorBuffer;
uniform sampler2D velocityMap;

vec4 motionBlur(sampler2D color, sampler2D motion, vec2 uv, float intensity)
{
  vec2 speed = 2. * texture2D(motion, uv).rg;
  speed = vec2(-1,-1) + speed;
  vec2 offset = intensity * speed;
  vec3 c = vec3(0.);
 
  float inc = 0.1;
  float weight = 0.;
  for (float i = 0.; i <= 1.; i += inc)
  {
    c += texture2D(color, uv + i * offset).rgb;
    weight += 1.;
  }
  c /= weight;
  return vec4(c, 1.);
}
 

void main(){
vec3 fc = vec3(0,0,0);

   fc = motionBlur(colorBuffer,velocityMap,UV,0.1).rgb; 

   // fc = texture2D(velocityMap,UV).rgb;




    color = fc;
}
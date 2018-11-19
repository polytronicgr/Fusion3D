#version 440 core




// Interpolated values from the vertex shaders

in vec3 fragPos;
in vec2 texCoords;
in vec2 speed;

// Ouput data
out vec3 colorout;



vec3 getSpeedColor()
{
  vec3 sc;
  sc.x = 0.5 + speed.x * 0.5;
  sc.y = 0.5 + speed.y * 0.5;

  
  return sc;
}
 
void main(){
 
   vec3 sc = getSpeedColor();

    colorout = sc;
}

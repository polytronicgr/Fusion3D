#version 330 core



// Interpolated values from the vertex shaders

in vec3 fragPos;
in vec2 texCoords;


// Ouput data
out vec4 colorout;

// Values that stay constant for the whole mesh.
uniform sampler2D tC;



  

void main(){
 
 
    colorout = texture2D(tC,texCoords).rgba;




 
}

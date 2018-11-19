#version 330 core



// Interpolated values from the vertex shaders

in vec3 fragPos;
in vec2 texCoords;


// Ouput data
out vec4 colorout;

// Values that stay constant for the whole mesh.
uniform sampler2D tC;



void main(){
 
   
    
    colorout = texture(tC,texCoords).rgba;
    //if(colorout.r>0.1)
    //{
     //   colorout.rgb = vec3(1,1,1);
    //}

 


return;
 





 
}
